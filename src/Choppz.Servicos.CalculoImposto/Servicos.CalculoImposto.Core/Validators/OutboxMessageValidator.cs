using FluentValidation;
using Servicos.CalculoImposto.Core.Entities.OutboxMessage;

namespace Servicos.CalculoImposto.Core.Validators
{
    public class OutboxMessageValidator : AbstractValidator<OutboxMessage>
    {
        public OutboxMessageValidator()
        {
            RuleFor(message => message.TipoDoEvento)
                .NotEmpty().WithMessage("Tipo do evento não pode ser nulo ou vazio.");

            RuleFor(message => message.Payload)
                .NotEmpty().WithMessage("Payload não pode ser nulo ou vazio.");
        }
    }
}