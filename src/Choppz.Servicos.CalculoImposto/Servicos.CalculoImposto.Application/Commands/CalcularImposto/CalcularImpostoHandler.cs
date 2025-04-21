using MediatR;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Core.Abstractions.CacheService;
using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.Abstractions.Services;
using Servicos.CalculoImposto.Core.Abstractions.UnitOfWork;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;
using Servicos.CalculoImposto.Core.Events;
using Servicos.CalculoImposto.Infra.Persistence.Repositories;

namespace Servicos.CalculoImposto.Application.Commands.CalcularImposto
{
    public class CalcularImpostoHandler : IRequestHandler<CalcularImpostoCommand, RespostaPadronizadaModel>
    {
        private readonly ICacheService _cacheService;
        private readonly IImpostoService _impostoService;
        private readonly IPedidoTributadoRepository _pedidoTributadoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CalcularImpostoHandler(IPedidoTributadoRepository pedidoComImposto,
                                      IUnitOfWork unitOfWork,
                                      ICacheService cacheService,
                                      IImpostoService impostoService)
        {
            _pedidoTributadoRepository = pedidoComImposto;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
            _impostoService = impostoService;
        }

        public async Task<RespostaPadronizadaModel> Handle(CalcularImpostoCommand request, CancellationToken cancellationToken)
        {
            string cacheKey = $"Pedido-{request.PedidoId}";

            if (_cacheService.Existe(cacheKey) && await _pedidoTributadoRepository.ExistePedidoAsync(request.PedidoId))
                return RespostaPadronizadaModel.ComErros("O Pedido ja existe.");

            var imposto = _impostoService.CalcularImposto(request.CalcularValorTotal());

            var pedidoTributado = new PedidoTributado(request.PedidoId, request.ClienteId, imposto, PedidoItemModel.ConverterParaEntidade(request.Itens));

            await _pedidoTributadoRepository.InserirAsync(pedidoTributado);

            pedidoTributado.RaiseEvent(new PedidoTributadoSalvo(pedidoTributado.Id, pedidoTributado.PedidoId, pedidoTributado.ClienteId, pedidoTributado.Imposto, PedidoItemModel.ConverterParaDTO(request.Itens), pedidoTributado.Status));

            await _unitOfWork.SaveChangesAsync();

            return RespostaPadronizadaModel.ComSucesso(new CalcularImpostoResponse(pedidoTributado.Id, pedidoTributado.Status));
        }
    }
}