using MediatR;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Core.Abstractions.CacheService;
using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.Abstractions.UnitOfWork;
using Servicos.CalculoImposto.Core.Entities.PedidoTributado;
using Servicos.CalculoImposto.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicos.CalculoImposto.Application.Commands.CalcularImposto
{
    public class CalcularImpostoHandler : IRequestHandler<CalcularImpostoCommand, RespostaPadronizadaModel>
    {
        private readonly IPedidoTributadoRepository _pedidoTributadoRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public CalcularImpostoHandler(IPedidoTributadoRepository pedidoComImposto,
                                      IUnitOfWork unitOfWork,
                                      ICacheService cacheService)
        {
            _pedidoTributadoRepository = pedidoComImposto;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task<RespostaPadronizadaModel> Handle(CalcularImpostoCommand request, CancellationToken cancellationToken)
        {
            string cacheKey = $"Pedido-{request.PedidoId}";

            if (_cacheService.Existe(cacheKey) && await _pedidoTributadoRepository.ExistePedidoAsync(request.PedidoId))
                return RespostaPadronizadaModel.ComErros("O Pedido ja existe.");

            //Todo: Implementar o calculo do imposto aqui
            //Todo: Salvar a entidade no banco de dados aqui
            //Todo: Implementar o evento de pedido tributado aqui

            return RespostaPadronizadaModel.ComSucesso(new { });
        }
    }
}
