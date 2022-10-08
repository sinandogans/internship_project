using Entities.Concrete;
using FluentValidation;
using ServiceLayer.Utilities.Validation.FluentValidation;

namespace ServiceLayer.Utilities.Validation
{
    public class UserValidationManager
    {
        UserValidator _validator;

        public UserValidationManager(UserValidator validator)
        {
            _validator = validator;
        }

        public void Validate(User user)
        {
            var validationResult = _validator.Validate(user);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}
