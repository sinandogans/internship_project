using Entities.Concrete;
using FluentValidation;
using ServiceLayer.Utilities.Validation.FluentValidation;

namespace ServiceLayer.Utilities.Validation
{
    public class TicketValidationManager
    {
        TicketValidator _validator;

        public TicketValidationManager(TicketValidator validator)
        {
            _validator = validator;
        }

        public void Validate(Ticket ticket)
        {
            var validationResult = _validator.Validate(ticket);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}