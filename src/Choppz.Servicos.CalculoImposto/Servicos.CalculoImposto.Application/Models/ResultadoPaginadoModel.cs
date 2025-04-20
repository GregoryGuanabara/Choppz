namespace Servicos.CalculoImposto.Application.Models
{
    public sealed record ResultadoPaginadoModel<T>
    {
        public ResultadoPaginadoModel(IEnumerable<T> itens, int totalDeItens, int totaldePaginas, int paginaAtual)
        {
            Itens = itens;
            TotalDeItens = totalDeItens;
            TotaldePaginas = totaldePaginas;
            PaginaAtual = paginaAtual;
        }

        public IEnumerable<T> Itens { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalDeItens { get; set; }
        public int TotaldePaginas { get; set; }
    }
}