using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class ProfileDetailResponseExample : IExamplesProvider<ApiResponse<ProfileDetail>>
    {

        public ApiResponse<ProfileDetail> GetExamples()
        {
            return new ApiResponse<ProfileDetail>
            {
                Metadata = new ProfileDetail
                {
                    UserName = "john_doe",
                    Password = "securepassword123",
                    Email = "johndoe@example.com",
                    TypeContract = "Full-time",
                    DepartmentName = "IT Department",
                    PositionName = "Software Engineer",
                    ContractTypeName = "Permanent",
                    Name = "John Doe",
                    DOB = new DateTime(1990, 5, 15),
                    Address = "123 Main Street, Springfield",
                    Gender = "Male",
                    Countryside = "Springfield",
                    NationalId = "123456789",
                    Level = "Bachelor's Degree",
                    Major = "Computer Science",
                    BaseSalary = 50000.00,
                    BaseInsurance = 5000.00,
                    RequiredDays = 22,
                    RequiredHours = 160,
                    WageDaily = 200.00,
                    WageHourly = 25.00,
                    Factor = 1.5,
                    FireUrlBase = "https://example.com/files/base_document.pdf",
                    FileUrlSigned = "https://example.com/files/signed_document.pdf"
                },
                IsSuccess = true
            };
        }
    }
}
