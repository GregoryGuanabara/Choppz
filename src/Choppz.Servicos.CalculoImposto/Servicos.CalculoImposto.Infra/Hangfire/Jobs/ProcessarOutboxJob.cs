using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.Abstractions.UnitOfWork;
using Servicos.CalculoImposto.Core.Enums;

namespace Servicos.CalculoImposto.Infra.Hangfire.Jobs
{
    internal sealed class ProcessarOutboxJob
    {
        private readonly IOutboxMessageRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProcessarOutboxJob(IOutboxMessageRepository outboxRepository,
                                  IUnitOfWork unitOfWork)
        {
            _outboxRepository = outboxRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Executar()
        {
            var mensagens = await _outboxRepository.PegarTodosAsync(EOutboxMessageStatus.Pendente);

            foreach (var mensagem in mensagens)
            {
                try
                {
                    //await _messageBusService.PublishAsync(mensagem);
                    mensagem.SetarComoProcessado();
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