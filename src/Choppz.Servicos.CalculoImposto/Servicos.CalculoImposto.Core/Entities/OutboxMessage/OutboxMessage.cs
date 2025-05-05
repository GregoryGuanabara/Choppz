using Servicos.CalculoImposto.Core.BaseEntities;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Core.Entities.OutboxMessage
{
    public class OutboxMessage : AggregateRoot
    {
        public OutboxMessage(string tipoDoEvento, string payload)
        {
            if (string.IsNullOrWhiteSpace(tipoDoEvento))
                throw new ArgumentException("Tipo do evento não pode ser nulo ou vazio.", nameof(tipoDoEvento));

            if (string.IsNullOrWhiteSpace(payload))
                throw new ArgumentException("Payload não pode ser nulo ou vazio.", nameof(payload));

            TipoDoEvento = tipoDoEvento;
            Payload = payload;
            Status = EOutboxMessageStatus.Pendente;
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

    }
}