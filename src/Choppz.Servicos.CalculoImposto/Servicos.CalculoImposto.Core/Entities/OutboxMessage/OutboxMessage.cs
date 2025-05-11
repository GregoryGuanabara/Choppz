using Servicos.CalculoImposto.Core.Abstractions.DomainException;
using Servicos.CalculoImposto.Core.Abstractions.Validators;
using Servicos.CalculoImposto.Core.BaseEntities;
using Servicos.CalculoImposto.Core.Enums;
using Servicos.CalculoImposto.Core.Validators;

namespace Servicos.CalculoImposto.Core.Entities.OutboxMessage
{
    public class OutboxMessage : AggregateRoot, IValidatable
    {
        public OutboxMessage(string tipoDoEvento, string payload)
        {
            TipoDoEvento = tipoDoEvento;
            Payload = payload;
            Status = EOutboxMessageStatus.Pendente;
            Validate();
        }

        public string TipoDoEvento { get; private set; }
        public string Payload { get; private set; }
        public EOutboxMessageStatus Status { get; private set; }
        public DateTime? ProcessadoEm { get; private set; }

        public void SetarComoProcessado()
        {
            ProcessadoEm = DateTime.Now;
            Status = EOutboxMessageStatus.Processado;
            AtualizarModificadoEm();
        }

        public void SetarComoFalha()
        {
            Status = EOutboxMessageStatus.Falhou;
            AtualizarModificadoEm();
        }

        public void Validate()
        {
            var validator = new OutboxMessageValidator();
            var result = validator.Validate(this);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                    throw new DomainException(error.ErrorMessage);
            }
        }
    }
}