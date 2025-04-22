using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.Abstractions.UnitOfWork;
using Servicos.CalculoImposto.Core.Entities.OutboxMessage;
using Servicos.CalculoImposto.Core.Enums;
using Servicos.CalculoImposto.Infra.Hangfire.Jobs;
using Servicos.CalculoImposto.Infra.MessageBus;

namespace Servicos.CalculoImposto.Infra.Tests.Services
{
    public class ProcessarOutboxJobTests
    {
        private readonly ProcessarOutboxJob _job;
        private readonly IOutboxMessageRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageBusService _messageBusService;

        public ProcessarOutboxJobTests()
        {
            _outboxRepository = Substitute.For<IOutboxMessageRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _messageBusService = Substitute.For<IMessageBusService>();
            _job = new ProcessarOutboxJob(_outboxRepository, _unitOfWork, _messageBusService);
        }

        [Fact]
        public async Task Executar_DeveProcessarTodasMensagensPendentes()
        {
            // Arrange
            var mensagensPendentes = new List<OutboxMessage>
            {
                new OutboxMessage("Tipo1", "Payload1"),
                new OutboxMessage("Tipo2", "Payload2")
            };

            _outboxRepository.PegarTodosAsync(EOutboxMessageStatus.Pendente)
                .Returns(mensagensPendentes);

            // Act
            await _job.Executar();

            // Assert
            await _messageBusService.Received(1).PublishAsync(mensagensPendentes[0]);
            await _messageBusService.Received(1).PublishAsync(mensagensPendentes[1]);
            await _unitOfWork.Received(1).SaveChangesAsync();

            mensagensPendentes[0].Status.Should().Be(EOutboxMessageStatus.Processado);
            mensagensPendentes[1].Status.Should().Be(EOutboxMessageStatus.Processado);
        }

        [Fact]
        public async Task Executar_DeveMarcarComoFalha_QuandoOcorrerErro()
        {
            // Arrange
            var mensagemComErro = new OutboxMessage("TipoErro", "PayloadErro");
            var mensagemSucesso = new OutboxMessage("TipoSucesso", "PayloadSucesso");

            var mensagensPendentes = new List<OutboxMessage> { mensagemComErro, mensagemSucesso };

            _outboxRepository.PegarTodosAsync(EOutboxMessageStatus.Pendente)
                .Returns(mensagensPendentes);

            _messageBusService.PublishAsync(mensagemComErro)
                .Throws(new Exception("Erro simulado"));

            // Act
            await _job.Executar();

            // Assert
            mensagemComErro.Status.Should().Be(EOutboxMessageStatus.Falhou);
            mensagemSucesso.Status.Should().Be(EOutboxMessageStatus.Processado);

            await _messageBusService.Received(1).PublishAsync(mensagemComErro);
            await _messageBusService.Received(1).PublishAsync(mensagemSucesso);
            await _unitOfWork.Received(1).SaveChangesAsync();
        }

        [Fact]
        public async Task Executar_NaoDeveFazerNada_QuandoNaoHouverMensagensPendentes()
        {
            // Arrange
            _outboxRepository.PegarTodosAsync(EOutboxMessageStatus.Pendente)
                .Returns(new List<OutboxMessage>());

            // Act
            await _job.Executar();

            // Assert
            await _messageBusService.DidNotReceiveWithAnyArgs().PublishAsync(default!);
            await _unitOfWork.DidNotReceive().SaveChangesAsync();
        }

        [Fact]
        public async Task Executar_DeveSalvarMudancasAposProcessarTodasMensagens()
        {
            // Arrange
            var mensagens = new List<OutboxMessage>
            {
                new OutboxMessage("Tipo1", "Payload1"),
                new OutboxMessage("Tipo2", "Payload2")
            };

            _outboxRepository.PegarTodosAsync(EOutboxMessageStatus.Pendente)
                .Returns(mensagens);

            var saveChangesCalled = false;
            _unitOfWork.When(u => u.SaveChangesAsync())
                .Do(_ => saveChangesCalled = true);

            // Act
            await _job.Executar();

            // Assert
            saveChangesCalled.Should().BeTrue();
            await _unitOfWork.Received(1).SaveChangesAsync();
        }
    }
}