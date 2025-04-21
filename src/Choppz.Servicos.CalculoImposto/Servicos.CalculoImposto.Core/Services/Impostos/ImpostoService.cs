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
        private readonly IImpostoStrategyFactory _factory;

        public ImpostoService(IImpostoStrategyFactory factory)
        {
            _factory = factory;
        }

        public decimal CalcularImposto(decimal valorTotalItens)
        {
            if (valorTotalItens < 0)
                throw new ArgumentException(nameof(valorTotalItens),"O valor total dos itens não pode ser negativo");

            var strategy = _factory.ObterStrategy();
            return strategy.Calcular(valorTotalItens);
        }
    }
}
