using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class AdminLogin
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
    public class AdminLoginValidator : AbstractValidator<AdminLogin>
    {
        public AdminLoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email không được bỏ trống")
                .EmailAddress()
                .WithMessage("Email không đúng định dạng");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Mật khẩu không được để trống");
        }
    }
}
