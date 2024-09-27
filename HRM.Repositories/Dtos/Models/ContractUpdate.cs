﻿using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class ContractUpdate
    {
        public string? Name { get; set; } //2
        public DateTime DateOfBirth { get; set; }//2
        public bool Gender { get; set; }//2
        public string? Address { get; set; }//2
        public string? CountrySide { get; set; }//2
        public string? NationalID { get; set; }//2
        public DateTime NationalStartDate { get; set; }//2
        public string? NationalAddress { get; set; }//2
        public string? Level { get; set; }//2
        public string? Major { get; set; }//2
    }
    public class ContractUpdateValidator : AbstractValidator<ContractUpdate>
    {
        public ContractUpdateValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Tên nhân viên hợp đồng không được bỏ trống.")
                .MaximumLength(100)
                .WithMessage("Tên không được dài quá 100 kí tự.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty()
                .WithMessage("Ngày sinh của nhân viên hợp đồng không được bỏ trống.")
                .LessThan(DateTime.Now.AddYears(-18))
                .WithMessage("Nhân viên hợp đồng phải lớn hơn 18 tuổi.");

            RuleFor(x => x.Gender)
                .NotNull()
                .WithMessage("Giới tính không được bỏ trống.");

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Địa chỉ nhà của nhân viên hợp đồng không được bỏ trống.")
                .MaximumLength(200)
                .WithMessage("Địa chỉ nhà của nhân viên hợp đồng không được dài hơn 200 kí tự.");

            RuleFor(x => x.CountrySide)
                .NotEmpty()
                .WithMessage("Quê quán của nhân viên hợp đồng không được bỏ trống.")
                .MaximumLength(100)
                .WithMessage("Quê quán của nhân viên hợp đồng không được dài hơn 100 kí tự.");

            RuleFor(x => x.NationalID)
                .NotEmpty()
                .WithMessage("Căn cước công dân của nhân viên hợp đồng không được bỏ trống.")
                .Length(9, 12)
                .WithMessage("Căn cước công dân của nhân viên hợp đồng phải dài từ 9 đến 12 kí tự.");

            RuleFor(x => x.NationalStartDate)
                .NotEmpty()
                .WithMessage("Ngày lấy căn cước công dân của nhân viên hợp đồng không được bỏ trống.")
                .LessThan(DateTime.Now)
                .WithMessage("Ngày lấy căn cước công dân của nhân viên hợp đồng phải trong quá khứ.");

            RuleFor(x => x.NationalAddress)
                .NotEmpty()
                .WithMessage("Địa điểm làm căn cước công dân của nhân viên hợp đồng không được bỏ trống.");

            RuleFor(x => x.Level)
                .NotEmpty()
                .WithMessage("Trình độ của nhân viên hợp đồng không được bỏ trống.")
                .MaximumLength(50)
                .WithMessage("Trình độ của nhân viên hợp đồng không được dài hơn 50 kí tự.");

            RuleFor(x => x.Major)
                .NotEmpty()
                .WithMessage("Chuyên ngành của nhân viên hợp đồng không được bỏ trống.")
                .MaximumLength(100)
                .WithMessage("Chuyên ngành của nhân viên hợp đồng không được dài quá 100 chữ.");
        }
    }
}
