using Entities.Concrete;
using FluentValidation;

namespace ServiceLayer.Utilities.Validation.FluentValidation
{
    public class AnswerValidator : AbstractValidator<Answer>
    {
        public AnswerValidator()
        {
            RuleFor(a => a.Body).NotEmpty();
        }
    }
}
