using MediatR;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicos.CalculoImposto.Application.Queries.PegarPeloProdutoId
{
    public class PegarPeloPedidoIdHandler : IRequestHandler<PegarPeloPedidoIdQuery, RespostaPadronizadaModel>
    {
        private readonly IPedidoTributadoRepository _pedidoTributadoRepository;

        public PegarPeloPedidoIdHandler(IPedidoTributadoRepository pedidoTributadoRepository)
        {
            _pedidoTributadoRepository = pedidoTributadoRepository;
        }

        public async Task<RespostaPadronizadaModel> Handle(PegarPeloPedidoIdQuery request, CancellationToken cancellationToken)
        {
            var pedidoComImposto = await _pedidoTributadoRepository.PegarPeloPedidoIdAsNoTrackingAsync(request.PedidoId);

            if (pedidoComImposto is null)
                return RespostaPadronizadaModel.ComErros("O pedido não foi localizado.");

            return RespostaPadronizadaModel.ComSucesso(new PedidoTributadoModel(pedidoComImposto.Id, pedidoComImposto.PedidoId, pedidoComImposto.ClienteId, pedidoComImposto.Imposto, PedidoItemModel.ConverterParaModels(pedidoComImposto.Itens), pedidoComImposto.Status));
        }
    }
}