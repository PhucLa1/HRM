using HRM.Repositories.Dtos.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Helper
{
	public class HandlePostForm
	{
		public static string CreatePostContent(JobPostingResult job)
		{
			var commentary = $@"🚀 **TUYỂN DỤNG VỊ TRÍ: {job.PositionName.ToUpper()}** 🚀\n\n" +
					 $@"✨ Chúng tôi đang tìm kiếm một lập trình viên tài năng để gia nhập **đội ngũ phát triển phần mềm** của chúng tôi!\n\n" +
					 $@"📌 *Mô tả công việc:* {job.Description}\n\n" +
					 $@"📅 *Ngày đăng tuyển:* {job.PostingDate:dd/MM/yyyy}\n" +
					 $@"📅 *Ngày hết hạn:* {job.ExpirationDate:dd/MM/yyyy}\n\n" +
					 $@"💼 *Yêu cầu kinh nghiệm:* {job.ExperienceRequired}\n\n" +
					 $@"💰 *Mức lương:* {job.SalaryRangeMin:C0} - {job.SalaryRangeMax:C0}\n\n" +
					 $@"📍 *Vị trí làm việc:* {job.Location}\n\n" +
					 $@"👤 *Người liên hệ:* {job.EmployeeName}\n\n" +
					 $@"🔗 Ứng tuyển ngay tại: [https://www.youtube.com/](https://www.youtube.com/)\n\n" +
					 $@"**🌟 Gia nhập chúng tôi để cùng nhau tạo nên những giá trị tuyệt vời!**";
			return commentary;
		}
		//public static string CreatePostContent(
		//	string PositionName,
		//	string Description,
		//	string Location,
		//	string PostingDate,
		//	string ExpirationDate,
		//	int SalaryRangeMin,
		//	int SalaryRangeMax,
		//	string ExperienceRequired,
		//	string EmployeeName)
		//{
		//	var commentary = $@"🚀 **TUYỂN DỤNG VỊ TRÍ: {PositionName.ToUpper()}** 🚀\n\n" +
		//			 $@"✨ Chúng tôi đang tìm kiếm một lập trình viên tài năng để gia nhập **đội ngũ phát triển phần mềm** của chúng tôi!\n\n" +
		//			 $@"📌 *Mô tả công việc:* {Description}\n\n" +
		//			 $@"📅 *Ngày đăng tuyển:* {PostingDate:dd/MM/yyyy}\n" +
		//			 $@"📅 *Ngày hết hạn:* {ExpirationDate:dd/MM/yyyy}\n\n" +
		//			 $@"💼 *Yêu cầu kinh nghiệm:* {ExperienceRequired}\n\n" +
		//			 $@"💰 *Mức lương:* {SalaryRangeMin:C0} - {SalaryRangeMax:C0}\n\n" +
		//			 $@"📍 *Vị trí làm việc:* {Location}\n\n" +
		//			 $@"👤 *Người liên hệ:* {EmployeeName}\n\n" +
		//			 $@"🔗 Ứng tuyển ngay tại: [https://www.youtube.com/](https://www.youtube.com/)\n\n" +
		//			 $@"**🌟 Gia nhập chúng tôi để cùng nhau tạo nên những giá trị tuyệt vời!**";
		//	return commentary;
		//}
	}
}
