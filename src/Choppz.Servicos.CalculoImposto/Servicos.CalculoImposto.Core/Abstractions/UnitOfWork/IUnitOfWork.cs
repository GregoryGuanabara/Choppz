namespace Servicos.CalculoImposto.Core.Abstractions.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}