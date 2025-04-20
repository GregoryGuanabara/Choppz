namespace Servicos.CalculoImposto.Core.Abstractions.CacheService
{
    public interface ICacheService
    {
        T? Pegar<T>(string key);

        void Inserir<T>(string key, T value, TimeSpan expiration);

        void Remover(string key);

        bool Existe(string key);
    }
}