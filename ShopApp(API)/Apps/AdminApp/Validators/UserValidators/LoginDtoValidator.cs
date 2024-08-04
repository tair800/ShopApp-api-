using FluentValidation;
using ShopApp_API_.Apps.AdminApp.Dtos.UserDto;

namespace ShopApp_API_.Apps.AdminApp.Validators.UserValidators
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(c => c.UserName)
                .NotNull()
                .MaximumLength(30);

            RuleFor(c => c.Email)
               .NotNull()
               .EmailAddress()
               .MaximumLength(30);

            RuleFor(c => c.Password)
                .NotNull()
                .MaximumLength(12)
                .MinimumLength(6);

        }
    }
}
