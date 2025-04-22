namespace Servicos.CalculoImposto.Application.Models
{
    public sealed record ErroModel
    {
        public ErroModel(string propriedade, string mensagem)
        {
            Propriedade = propriedade;
            Mensagem = mensagem;
        }

        public string Propriedade { get; set; }
        public string Mensagem { get; set; }
    }
}