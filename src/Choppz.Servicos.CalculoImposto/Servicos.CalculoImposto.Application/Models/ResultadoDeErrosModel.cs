namespace Servicos.CalculoImposto.Application.Models
{
    public sealed record ResultadoDeErrosModel
    {
        public ResultadoDeErrosModel(List<ErroModel> erros, string mensagem)
        {
            Erros = erros;
            Mensagem = mensagem;
        }

        public ResultadoDeErrosModel(ErroModel erros, string mensagem)
        {
            Erros = new List<ErroModel>() { erros };
            Mensagem = mensagem;
        }

        public ResultadoDeErrosModel(string mensagem)
        {
            Mensagem = mensagem;
            Erros = new List<ErroModel>();
        }

        public List<ErroModel> Erros { get; set; }

        public string Mensagem { get; set; }
    }
}