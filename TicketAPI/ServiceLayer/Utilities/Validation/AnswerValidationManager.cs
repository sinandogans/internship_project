using Entities.Concrete;
using FluentValidation;
using ServiceLayer.Utilities.Validation.FluentValidation;

namespace ServiceLayer.Utilities.Validation
{
    public class AnswerValidationManager
    {
        AnswerValidator _validator;

        public AnswerValidationManager(AnswerValidator validator)
        {
            _validator = validator;
        }

        public void Validate(Answer answer)
        {
            var validationResult = _validator.Validate(answer);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}