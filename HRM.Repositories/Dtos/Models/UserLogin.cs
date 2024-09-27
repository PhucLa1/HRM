using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class UserLogin
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class UserLoginValidator : AbstractValidator<UserLogin>
    {
        public UserLoginValidator()
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
