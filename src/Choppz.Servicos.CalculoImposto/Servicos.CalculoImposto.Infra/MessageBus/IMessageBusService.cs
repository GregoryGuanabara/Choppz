using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicos.CalculoImposto.Infra.MessageBus
{
    internal interface IMessageBusService
    {
        Task<bool> PublishAsync<T>(T message) where T : class;
    }
}
