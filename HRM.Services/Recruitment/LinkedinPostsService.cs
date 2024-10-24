using Google.Apis.Gmail.v1.Data;
using HRM.Repositories.Dtos.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Recruitment
{
	public interface ILinkedInPostService
	{
		Task<bool> PostToLinkedIn(string message, string token);
		Task PostToLinkedIn2();
	}

	public class LinkedinPostsService : ILinkedInPostService
	{
		private const string LinkedInApiUrl = "https://api.linkedin.com/v2/ugcPosts";

		public async Task<bool> PostToLinkedIn(string message, string token)
		{
			try
			{
				using (var client = new HttpClient())
				{
					// Setup token authorization
					client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
					client.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");

					// Create post content
					var postContent = new
					{
						author = "urn:li:person:0JVkfQ3Pfq",  // Replace with the user's LinkedIn ID
						lifecycleState = "PUBLISHED",
						specificContent = new
						{
							comLinkedinUgcShareContent = new
							{
								shareCommentary = new
								{
									text = message
								},
								shareMediaCategory = "NONE"
							}
						},
						visibility = new
						{
							comLinkedinUgcMemberNetworkVisibility = "PUBLIC"
						}
					};

					// Convert post content to JSON
					var jsonContent = JsonConvert.SerializeObject(postContent);
					var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

					// Send POST request to LinkedIn API
					var response = await client.PostAsync(LinkedInApiUrl, content);

					// Check if the post was successful
					return response.IsSuccessStatusCode;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error posting to LinkedIn: {ex.Message}");
				return false;
			}
		}

		public async Task PostToLinkedIn2()
		{

			var client = new HttpClient();
			var request = new HttpRequestMessage(HttpMethod.Post, "https://api.linkedin.com/v2/ugcPosts");
			request.Headers.Add("Authorization", "Bearer AQUucvRfeLJHhB2D8jY-7e1S9f38J4P_ehFGzJ72N5A_fnj0BP1P6FhlC-aXMLTkHOR7bQ4u8axEvN-aUHp0HabEyDzFtb_0Hc_3yhKbvJYB_KhVjTb6loK0J7jN3wZOdxqtk7niHuFB-ZKf0wtDlHPi0yiRTiUCWeXBIcll5Hm7HbwKrOBrqJrjpY12salaYJRI_6eVO8uLDfP_KfT1fgk0nzt0XqBR-eAl11d7pcN17R8x9XdD9hKIAOVjx93DIAkXaunA1N5OfT5jWWBdsE-K1wZq70D_MXdO4PCCFgeZx1J-z4OgWsDYSKG8mal958J9avL3Ngprs_gCl2Ks1t6-BC9gAg");
			request.Headers.Add("Cookie", "lidc=\"b=VB75:s=V:r=V:a=V:p=V:g=4267:u=6:x=1:i=1729533423:t=1729565558:v=2:sig=AQGU7iba_pNaxkID-E7_Q-ZqKgouG-lk\"; bcookie=\"v=2&529a09be-5c93-4dbe-8cbb-c454c0566956\"");
			var linkedinUpsert = new LinkedinUpsert()
			{
				author = "urn:li:person:0JVkfQ3Pfq",
				lifecycleState = "PUBLISHED",
				specificContent = new SpecificContent()
				{
					comlinkedinugcShareContent = new ComLinkedinUgcShareContent()
					{
						shareCommentary = new ShareCommentary()
						{
							text = "Hello World! This is my something else Post!"
						},
						shareMediaCategory = "NONE"
					}
				},
				visibility = new Visibility()
				{
					comlinkedinugcMemberNetworkVisibility = "PUBLIC"
				}
			};
			var content = new StringContent(JsonConvert.SerializeObject(linkedinUpsert), null, "application/json");
			request.Content = content;
			var response = await client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			Console.WriteLine(await response.Content.ReadAsStringAsync());

		}
	}
}
