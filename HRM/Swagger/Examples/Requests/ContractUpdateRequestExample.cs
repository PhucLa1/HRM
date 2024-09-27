using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class ContractUpdateRequestExample : IExamplesProvider<ContractUpdate>
    {
        public ContractUpdate GetExamples()
        {
            return new ContractUpdate
            {
                Name = "Lã Hồng Phúc",
                DateOfBirth = new DateTime(2000,3,3),
                Gender = true,
                Address = "26/231 Trần Nguyên Hãn, Lê Chân, Hải Phòng",
                CountrySide = "Ngũ Phúc, Kiến Thụy",
                NationalAddress = "Cục kiểm soát Hải Phòng",
                NationalID = "031203000167",
                Level = "Thạc sĩ",
                Major = "Công nghệ thông tin"
            };
        }
    }
}
