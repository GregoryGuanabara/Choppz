using MediatR;
using System.Transactions;

namespace Servicos.CalculoImposto.Core.BaseEntities
{
    public interface IDomainEvent : INotification
    {
        DateTime OcorridoEm { get; }
    }
}