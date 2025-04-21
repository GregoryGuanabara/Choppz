using MediatR;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Core.Abstractions.CacheService;
using Servicos.CalculoImposto.Core.Events;

namespace Servicos.CalculoImposto.Application.Notifications
{
    public sealed class CachearPedidoResponseHandler : INotificationHandler<PedidoTributadoSalvoEvent>
    {
        private readonly ICacheService _cacheService;

        public CachearPedidoResponseHandler(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        public Task Handle(PedidoTributadoSalvoEvent notification, CancellationToken cancellationToken)
        {
            var pedidoResponse = new PedidoTributadoModel(notification.PedidoComImpostoId, notification.PedidoId, notification.ClienteId, notification.Imposto, PedidoItemModel.ConverterParaModels(notification.Itens), notification.Status);

            var objetoParaCachear = RespostaPadronizadaModel.ComSucesso(pedidoResponse);

            string cacheKey = $"Pedido-{pedidoResponse.PedidoId}";

            _cacheService.Inserir(cacheKey, objetoParaCachear, TimeSpan.FromMinutes(10));

            return Task.CompletedTask;
        }
    }
}
