using System.Text.Json.Serialization;

namespace Servicos.CalculoImposto.Application.Models
{


    public sealed record RespostaPadronizadaModel

    {
        public object? Data { get; }
        public bool Sucesso { get; }
        public string? Mensagem { get; }

        [JsonIgnore]
        public bool ResultadoValidacao { get; }

        private RespostaPadronizadaModel(object? data, bool sucesso, string mensagem)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
            Data = data;
            ResultadoValidacao = true;
        }

        private RespostaPadronizadaModel(object? data, bool sucesso, bool resultadoValidacao)
        {
            Sucesso = sucesso;
            Data = data;
            ResultadoValidacao = resultadoValidacao;
        }

        public static RespostaPadronizadaModel ComSucesso(object data, string mensagem = "Operação concluída com sucesso.") =>
            new(data, true, mensagem);

        public static RespostaPadronizadaModel ComErros(string mensagem) =>
          new(new ResultadoDeErrosModel(mensagem), false, true);

        public static RespostaPadronizadaModel ComErros(List<ErroModel> data, string message = "Falha ao processar a operação.") =>
            new(new ResultadoDeErrosModel(data, message), false, false);
    }
}