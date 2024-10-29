using FluentValidation;

namespace CRM.DTOs.Account
{
    public class RegisterDTO
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

    }
    public class RegisterValidator : AbstractValidator<RegisterDTO>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Email is required!");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required!")
                .MinimumLength(8).WithMessage("Minimum 8 character allowed!")
                .MaximumLength(25).WithMessage("Maxsimum password length has reached!");
        } 
    }
}
