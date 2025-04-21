using Servicos.CalculoImposto.Core.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicos.CalculoImposto.Core.Entities.OutboxMessage
{
    public class OutboxMessage : AggregateRoot
    {
        public OutboxMessage(string tipoDoEvento, string payload)
        {
            TipoDoEvento = tipoDoEvento;
            Payload = payload;
            Status = EOutboxMessageStatus.Pendente;
        }

        public string TipoDoEvento { get; private set; }
        public string Payload { get; private set; }
        public EOutboxMessageStatus Status { get; set; }
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
