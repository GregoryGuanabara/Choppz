using Servicos.CalculoImposto.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicos.CalculoImposto.Application.Models
{
    public class PedidoTributadoModel
    {
        public PedidoTributadoModel(int id, int pedidoId, int clienteId, decimal imposto, List<PedidoItemModel> itens, EPedidoTributadoStatus status)
        {
            Id = id;
            PedidoId = pedidoId;
            ClienteId = clienteId;
            Imposto = imposto;
            Itens = itens;
            Status = status.ToString();
        }

        public int Id { get; set; }
        public int PedidoId { get; set; }
        public int ClienteId { get; set; }
        public decimal Imposto { get; set; }
        public List<PedidoItemModel> Itens { get; set; }
        public string Status { get; set; }
    }
}
