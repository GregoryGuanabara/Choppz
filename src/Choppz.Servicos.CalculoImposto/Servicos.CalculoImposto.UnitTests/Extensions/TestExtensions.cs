using Servicos.CalculoImposto.Core.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicos.CalculoImposto.UnitTests.Extensions
{
    internal static class TestExtensions
    {
        internal static void SetId<T>(this T entity, int id) where T : AggregateRoot
        {
            var property = typeof(AggregateRoot).GetProperty("Id");
            property?.SetValue(entity, id);
        }
    }
}
