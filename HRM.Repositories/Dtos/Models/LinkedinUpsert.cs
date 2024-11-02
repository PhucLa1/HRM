using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Models
{
	// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
	public class ComLinkedinUgcShareContent
	{
		[JsonProperty("shareCommentary")]
		public ShareCommentary shareCommentary { get; set; }

		[JsonProperty("shareMediaCategory")]
		public string? shareMediaCategory { get; set; }
	}

	public class LinkedinUpsert
	{
		[JsonProperty("author")]
		public string author { get; set; }

		[JsonProperty("lifecycleState")]
		public string lifecycleState { get; set; }

		[JsonProperty("specificContent")]
		public SpecificContent specificContent { get; set; }

		[JsonProperty("visibility")]
		public Visibility visibility { get; set; }
	}

	public class ShareCommentary
	{
		[JsonProperty("text")]
		public string text { get; set; }
	}

	public class SpecificContent
	{
		[JsonProperty("com.linkedin.ugc.ShareContent")]
		public ComLinkedinUgcShareContent comlinkedinugcShareContent { get; set; }
	}

	public class Visibility
	{
		[JsonProperty("com.linkedin.ugc.MemberNetworkVisibility")]
		public string comlinkedinugcMemberNetworkVisibility { get; set; }
	}


}
