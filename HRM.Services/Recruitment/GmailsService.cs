using Google.Apis.Gmail.v1.Data;
using Google.Apis.Gmail.v1;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using Gmail = HRM.Data.Entities.Gmail;
using System.Configuration;
using HRM.Repositories.Helper;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Recruitment
{
	public interface IGmailsService
	{
		Task<ApiResponse<IEnumerable<GmailResult>>> GetAllGmail();
	}
	public class GmailsService : IGmailsService
	{
		private readonly IBaseRepository<Gmail> _baseRepository;
		private readonly IValidator<GmailUpsert> _gmailUpsertValidator;
		private readonly IMapper _mapper;
		public GmailsService(
			IBaseRepository<Gmail> baseRepository,
			IValidator<GmailUpsert> webUpsertValidator,
			IMapper mapper)
		{
			_baseRepository = baseRepository;
			_gmailUpsertValidator = webUpsertValidator;
			_mapper = mapper;
		}

		public async Task<ApiResponse<IEnumerable<GmailResult>>> GetAllGmail()
		{
			try
			{
				// Lấy danh sách email từ Gmail API
				GmailService service = GmailHelper.GetService();
				UsersResource.MessagesResource.ListRequest listRequest = service.Users.Messages.List(Convert.ToString(ConfigurationManager.AppSettings["HostAddress"]));
				listRequest.LabelIds = "INBOX";
				listRequest.IncludeSpamTrash = false;
				listRequest.Q = "is:unread"; // Chỉ lấy các email chưa đọc

				ListMessagesResponse response = listRequest.Execute();

				if (response.Messages != null && response.Messages.Count > 0)
				{
					// Nếu có email, xử lý từng email và lưu vào database
					foreach (var message in response.Messages)
					{
						var msgRequest = service.Users.Messages.Get(Convert.ToString(ConfigurationManager.AppSettings["HostAddress"]), message.Id);
						var msgContent = msgRequest.Execute();

						string from = "";
						string subject = "";
						string body = "";

						foreach (var header in msgContent.Payload.Headers)
						{
							if (header.Name == "From")
								from = header.Value;
							if (header.Name == "Subject")
								subject = header.Value;
						}

						if (msgContent.Payload.Parts == null && msgContent.Payload.Body != null)
						{
							body = msgContent.Payload.Body.Data;
						}
						else
						{
							body = GmailHelper.Base64Decode(msgContent.Payload.Parts[0].Body.Data);
						}

						// Lưu email vào cơ sở dữ liệu
						Gmail email = new Gmail
						{
							From = from,
							To = Convert.ToString(ConfigurationManager.AppSettings["HostAddress"]),
							Body = body,
							MailDateTime = DateTime.Now
						};

						await _baseRepository.AddAsync(email);
					}

					await _baseRepository.SaveChangeAsync();
				}

				// Trả về kết quả sau khi đã xử lý
				return new ApiResponse<IEnumerable<GmailResult>>
				{
					Metadata = _mapper.Map<IEnumerable<GmailResult>>(await _baseRepository.GetAllQueryAble().ToListAsync()),
					IsSuccess = true
				};
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		private string DecodeBase64(string base64EncodedData)
		{
			var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}
	}
}
