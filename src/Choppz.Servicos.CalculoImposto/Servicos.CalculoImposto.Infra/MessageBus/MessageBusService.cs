using Servicos.CalculoImposto.Core.Abstractions.Repositories;
using Servicos.CalculoImposto.Core.Entities.OutboxMessage;
using System.Net.Http.Json;
using System.Text.Json;

namespace Servicos.CalculoImposto.Infra.MessageBus
{
    public class MessageBusService : IMessageBusService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        
        public async Task<bool> PublishAsync(OutboxMessage message)
        {
            await Task.Delay(100);

            var random = new Random();
            var chanceDeErro = random.NextDouble();

            if (chanceDeErro < 0.05)
                throw new Exception("Falha ao publicar a mensagem.");

            return await SimulateFakeQueue(message);
        }

        private async Task<bool> SimulateFakeQueue(OutboxMessage message)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "https://usewebhook.com/5af31b5c19eb767aba6c31f1131b115e",
                message,
                new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                });

            return response.IsSuccessStatusCode;
        }
    }
}