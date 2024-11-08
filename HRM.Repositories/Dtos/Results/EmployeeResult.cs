using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Results
{
    public class EmployeeResult
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public int Tenure { get; set; }
        public bool Gender { get; set; }
        public string Address { get; set; } = "";
        public string CountrySide { get; set; } = "";
        public string NationalID { get; set; } = "";
        public DateTime NationalStartDate { get; set; }
        public string NationalAddress { get; set; } = "";
        public string Level { get; set; } = "";
        public string Major { get; set; } = "";
        public int PositionId { get; set; } 
        public string PositionName { get; set; } = "";
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = "";
        public string Avatar { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string Email { get; set; } = "";

    }
}
