using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class AccountUpdate
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }


    public class AccountUpdateValidator : AbstractValidator<AccountUpdate>
    {
        public AccountUpdateValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("User name không được bỏ trống");
            RuleFor(x => x.Password)
                   .NotEmpty()
                   .WithMessage("Mật khẩu không được để trống");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email không được bỏ trống")
                .EmailAddress()
                .WithMessage("Email không đúng định dạng");
        }
    }
}
