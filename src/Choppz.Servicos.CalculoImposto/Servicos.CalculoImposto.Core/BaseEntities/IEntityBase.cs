using Servicos.CalculoImposto.Core.Abstractions.Validators;

namespace Servicos.CalculoImposto.Core.BaseEntities
{
    public interface IEntityBase 
    {
        protected int Id { get; }
    }
}