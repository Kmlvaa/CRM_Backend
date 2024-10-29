using FluentValidation;

namespace CRM.DTOs.Account
{
    public class LoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required!")
                .MinimumLength(8).WithMessage("Minimum 8 character allowed!")
                .MaximumLength(25).WithMessage("Maxsimum password length has reached!");
        }
    }
}
