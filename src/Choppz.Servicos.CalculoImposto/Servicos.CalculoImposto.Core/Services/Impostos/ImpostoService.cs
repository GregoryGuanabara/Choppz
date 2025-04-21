using Servicos.CalculoImposto.Core.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicos.CalculoImposto.Core.Services.Impostos
{
    public sealed class ImpostoService : IImpostoService
    {
        private readonly ImpostoStrategyFactory _factory;

        public ImpostoService(ImpostoStrategyFactory factory)
        {
            _factory = factory;
        }

        public decimal CalcularImposto(decimal valorTotalItens)
        {
            var strategy = _factory.ObterStrategy();
            return strategy.Calcular(valorTotalItens);
        }
    }
}
