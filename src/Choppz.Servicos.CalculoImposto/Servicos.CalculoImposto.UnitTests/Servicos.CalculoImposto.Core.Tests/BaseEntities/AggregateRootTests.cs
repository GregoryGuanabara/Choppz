using Bogus;
using FluentAssertions;
using NSubstitute;
using Servicos.CalculoImposto.Core.BaseEntities;

namespace Servicos.CalculoImposto.Core.Tests.BaseEntities
{
    public class AggregateRootTests
    {
        private readonly TestableAggregateRoot _aggregateRoot;
        private readonly IDomainEvent _domainEvent;

        public AggregateRootTests()
        {
            _aggregateRoot = new TestableAggregateRoot();
            _domainEvent = Substitute.For<IDomainEvent>();
        }

        private class TestableAggregateRoot : AggregateRoot
        {
            public void PublicAtualizarModificadoEm() => base.AtualizarModificadoEm();
        }

        [Fact]
        public void Constructor_DeveInicializarCorretamente()
        {
            // Assert
            _aggregateRoot.Id.Should().Be(0);
            _aggregateRoot.CriadoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            _aggregateRoot.ModificadoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
            _aggregateRoot.Events.Should().BeEmpty();
        }

        [Fact]
        public void RaiseEvent_DeveAdicionarEventoNaLista()
        {
            // Act
            _aggregateRoot.RaiseEvent(_domainEvent);

            // Assert
            _aggregateRoot.Events.Should().ContainSingle();
            _aggregateRoot.Events.First().Should().Be(_domainEvent);
        }

        [Fact]
        public void ClearEvents_DeveLimparTodosOsEventos()
        {
            // Arrange
            _aggregateRoot.RaiseEvent(_domainEvent);
            _aggregateRoot.RaiseEvent(Substitute.For<IDomainEvent>());

            // Act
            _aggregateRoot.ClearEvents();

            // Assert
            _aggregateRoot.Events.Should().BeEmpty();
        }

        [Fact]
        public void AtualizarModificadoEm_DeveAtualizarDataModificacao()
        {
            // Arrange
            var dataOriginal = _aggregateRoot.ModificadoEm;

            // Act
            _aggregateRoot.PublicAtualizarModificadoEm();

            // Assert
            _aggregateRoot.ModificadoEm.Should().BeAfter(dataOriginal);
            _aggregateRoot.ModificadoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Events_DeveRetornarColecaoSomenteLeitura()
        {
            // Act
            var events = _aggregateRoot.Events;

            // Assert
            events.Should().NotBeNull();
            events.Should().BeAssignableTo<IReadOnlyCollection<IDomainEvent>>();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void RaiseEvent_MultiplosEventos_DeveManterTodos(int quantidadeEventos)
        {
            // Arrange
            var events = Enumerable.Range(0, quantidadeEventos)
                .Select(_ => Substitute.For<IDomainEvent>())
                .ToList();

            // Act
            events.ForEach(e => _aggregateRoot.RaiseEvent(e));

            // Assert
            _aggregateRoot.Events.Should().HaveCount(quantidadeEventos);
            _aggregateRoot.Events.Should().BeEquivalentTo(events);
        }
    }
}