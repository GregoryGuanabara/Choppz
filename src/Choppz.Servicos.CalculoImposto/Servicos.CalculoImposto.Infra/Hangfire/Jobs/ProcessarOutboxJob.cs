using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.Abstractions.UnitOfWork;
using Servicos.CalculoImposto.Core.Enums;
using Servicos.CalculoImposto.Infra.MessageBus;

namespace Servicos.CalculoImposto.Infra.Hangfire.Jobs
{
    internal sealed class ProcessarOutboxJob
    {
        private readonly IMessageBusService _messageBusService;
        private readonly IOutboxMessageRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProcessarOutboxJob(IOutboxMessageRepository outboxRepository,
                                  IUnitOfWork unitOfWork,
                                  IMessageBusService messageBusService)
        {
            _outboxRepository = outboxRepository;
            _unitOfWork = unitOfWork;
            _messageBusService = messageBusService;
        }

        public async Task Executar()
        {
            var mensagens = await _outboxRepository.PegarTodosAsync(EOutboxMessageStatus.Pendente);

            foreach (var mensagem in mensagens)
            {
                try
                {
                    mensagem.SetarComoProcessado();
                    await _messageBusService.PublishAsync(mensagem);
                }
                catch (Exception ex)
                {
                    mensagem.SetarComoFalha();

                    // Logar a (ex) se necessário
                    Console.WriteLine(ex);
                }
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}