using Entities.Concrete;
using FluentValidation;

namespace ServiceLayer.Utilities.Validation.FluentValidation
{
    public class TicketValidator : AbstractValidator<Ticket>
    {
        public TicketValidator()
        {
            RuleFor(t => t.Body).NotEmpty();
            RuleFor(t => t.Subject).NotEmpty();
        }
    }
}
