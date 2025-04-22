namespace Servicos.CalculoImposto.Core.BaseEntities
{
    public abstract class AggregateRoot : IEntityBase
    {
        private List<IDomainEvent> _events = new List<IDomainEvent>();

        protected AggregateRoot()
        {
            CriadoEm = DateTime.Now;
            ModificadoEm = DateTime.Now;
        }

        public IReadOnlyCollection<IDomainEvent> Events => _events;

        public int Id { get; protected set; }

        public DateTime CriadoEm { get; private set; }

        public DateTime ModificadoEm { get; private set; }

        public void RaiseEvent(IDomainEvent @event)
        {
            _events.Add(@event);
        }

        public void ClearEvents()
        {
            _events.Clear();
        }

        protected void AtualizarModificadoEm()
        {
            ModificadoEm = DateTime.Now;
        }
    }
}