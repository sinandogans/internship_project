using Entities.Concrete;
using FluentValidation;

namespace ServiceLayer.Utilities.Validation.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Email).NotEmpty();
            RuleFor(u => u.Password).NotEmpty();
            RuleFor(u => u.Birthdate).NotEmpty();
            RuleFor(u => u.Username).NotEmpty();
        }
    }
}
