namespace Servicos.CalculoImposto.Infra.MessageBus
{
    internal class MessageBusService : IMessageBusService
    {
        public async Task<bool> PublishAsync<T>(T message) where T : class
        {
            await Task.Delay(100);

            var random = new Random();
            var chanceDeErro = random.NextDouble();

            if (chanceDeErro < 0.05)
                throw new Exception("Falha ao publicar a mensagem.");

            return true;
        }
    }
}