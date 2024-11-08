using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace HRM.Repositories.Helper
{
    public static class GmailHelper
    {
        static string[] Scopes = { GmailService.Scope.MailGoogleCom };
        static string ApplicationName = "Gmail API Application";
        public static IConfiguration? Configuration { get; set; }
		public static void Initialize(IConfiguration configuration)
		{
			Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
		}
		public static GmailService GetService()
        {
			
			UserCredential credential;
            using (FileStream stream = new FileStream(Configuration["GoogleApi:ClientInfo"],
                FileMode.Open, FileAccess.Read))
            {
                string FolderPath = Configuration["GoogleApi:CredentialsInfo"];
                string FilePath = Path.Combine(FolderPath, "APITokenCredentials");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
					GoogleClientSecrets.Load(stream).Secrets,
					Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(FilePath, true)).Result;
            }
            // Create Gmail API service.
            GmailService service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }
        public static string MsgNestedParts(IList<MessagePart> Parts)
        {
            string str = string.Empty;
            if (Parts.Count() < 0)
            {
                return string.Empty;
            }
            else
            {
                IList<MessagePart> PlainTestMail = Parts.Where(x => x.MimeType == "text/plain").ToList();
                IList<MessagePart> AttachmentMail = Parts.Where(x => x.MimeType == "multipart/alternative").ToList();

                if (PlainTestMail.Count() > 0)
                {
                    foreach (MessagePart EachPart in PlainTestMail)
                    {
                        if (EachPart.Parts == null)
                        {
                            if (EachPart.Body != null && EachPart.Body.Data != null)
                            {
                                str += EachPart.Body.Data;
                            }
                        }
                        else
                        {
                            return MsgNestedParts(EachPart.Parts);
                        }
                    }
                }
                if (AttachmentMail.Count() > 0)
                {
                    foreach (MessagePart EachPart in AttachmentMail)
                    {
                        if (EachPart.Parts == null)
                        {
                            if (EachPart.Body != null && EachPart.Body.Data != null)
                            {
                                str += EachPart.Body.Data;
                            }
                        }
                        else
                        {
                            return MsgNestedParts(EachPart.Parts);
                        }
                    }
                }
                return str;
            }
        }
        public static List<string> GetAttachments(string userId, string messageId, string outputDir)
        {
            try
            {
                List<string> FileName = new List<string>();
                GmailService GServices = GetService();
                Message message = GServices.Users.Messages.Get(userId, messageId).Execute();
                IList<MessagePart> parts = message.Payload.Parts;

                foreach (MessagePart part in parts)
                {
                    if (!string.IsNullOrEmpty(part.Filename))
                    {
                        string attId = part.Body.AttachmentId;
                        MessagePartBody attachPart = GServices.Users.Messages.Attachments.Get(userId, messageId, attId).Execute();

                        byte[] data = Base64ToByte(attachPart.Data);
                        File.WriteAllBytes(Path.Combine(outputDir, part.Filename), data);
                        FileName.Add(part.Filename);
                    }
                }
                return FileName;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
                return null;
            }
        }
        public static string Base64Decode(string Base64Test)
        {
            string EncodTxt = Base64Test.Replace("-", "+").Replace("_", "/");

            // STEP-2: Fixed invalid length of Base64Test
            switch (EncodTxt.Length % 4)
            {
                case 2: EncodTxt += "=="; break;
                case 3: EncodTxt += "="; break;
            }

            byte[] ByteArray = Convert.FromBase64String(EncodTxt);
            return Encoding.UTF8.GetString(ByteArray);
        }
        public static byte[] Base64ToByte(string Base64Test)
        {
            string decodedString = Base64Decode(Base64Test);
            return Encoding.UTF8.GetBytes(decodedString);
        }
        public static void MsgMarkAsRead(string HostEmailAddress, string MsgId)
        {
            //MESSAGE MARKS AS READ AFTER READING MESSAGE
            ModifyMessageRequest mods = new ModifyMessageRequest();
            mods.AddLabelIds = null;
            mods.RemoveLabelIds = new List<string> { "UNREAD" };
            GetService().Users.Messages.Modify(mods, HostEmailAddress, MsgId).Execute();
        }
    }
}
