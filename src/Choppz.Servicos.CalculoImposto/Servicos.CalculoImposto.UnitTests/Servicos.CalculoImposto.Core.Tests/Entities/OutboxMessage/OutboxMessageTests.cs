using Bogus;
using FluentAssertions;
using Servicos.CalculoImposto.Core.Entities.OutboxMessage;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Core.Tests.Entities
{
    public class OutboxMessageTests
    {
        private readonly Faker _faker = new Faker();

        [Fact]
        public void Constructor_DeveInicializarCorretamente()
        {
            // Arrange
            var tipoEvento = _faker.Lorem.Word();
            var payload = _faker.Lorem.Text();

            // Act
            var message = new OutboxMessage(tipoEvento, payload);

            // Assert
            message.TipoDoEvento.Should().Be(tipoEvento);
            message.Payload.Should().Be(payload);
            message.Status.Should().Be(EOutboxMessageStatus.Pendente);
            message.ProcessadoEm.Should().BeNull();
            message.ModificadoEm.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_ComTipoEventoInvalido_DeveLancarException(string tipoInvalido)
        {
            // Arrange
            var payload = _faker.Lorem.Text();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new OutboxMessage(tipoInvalido, payload));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Constructor_ComPayloadInvalido_DeveLancarException(string payloadInvalido)
        {
            // Arrange
            var tipoEvento = _faker.Lorem.Word();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new OutboxMessage(tipoEvento, payloadInvalido));
        }

        [Fact]
        public void SetarComoProcessado_DeveAtualizarStatusEData()
        {
            // Arrange
            var message = new OutboxMessage(_faker.Lorem.Word(), _faker.Lorem.Text());
            var dataOriginal = message.ModificadoEm;

            // Act
            message.SetarComoProcessado();

            // Assert
            message.Status.Should().Be(EOutboxMessageStatus.Processado);
            message.ProcessadoEm.Should().NotBeNull();
            message.ModificadoEm.Should().BeAfter(dataOriginal);
        }

        [Fact]
        public void SetarComoFalha_DeveAtualizarStatus()
        {
            // Arrange
            var message = new OutboxMessage(_faker.Lorem.Word(), _faker.Lorem.Text());
            var dataOriginal = message.ModificadoEm;

            // Act
            message.SetarComoFalha();

            // Assert
            message.Status.Should().Be(EOutboxMessageStatus.Falhou);
            message.ProcessadoEm.Should().BeNull();
            message.ModificadoEm.Should().BeAfter(dataOriginal);
        }

        [Fact]
        public void Propriedades_DevemTerAcessibilidadeCorreta()
        {
            // Arrange
            var type = typeof(OutboxMessage);

            // Assert
            type.GetProperty(nameof(OutboxMessage.TipoDoEvento))!.SetMethod!.IsPrivate.Should().BeTrue();
            type.GetProperty(nameof(OutboxMessage.Payload))!.SetMethod!.IsPrivate.Should().BeTrue();
            type.GetProperty(nameof(OutboxMessage.ProcessadoEm))!.SetMethod!.IsPrivate.Should().BeTrue();
            type.GetProperty(nameof(OutboxMessage.Status))!.SetMethod!.IsPrivate.Should().BeTrue();
        }
    }
}