using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Application.UseCases.CalcularImposto;
using Servicos.CalculoImposto.Infra.Persistence;
using Servicos.CalculoImposto.IntegrationTests.Builders;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Servicos.CalculoImposto.IntegrationTests.Controllers
{
    public class PedidosControllerIntegrationTests : IAsyncLifetime
    {
        private readonly HttpClient _client;
        private readonly ApplicationDbContext _dbContext;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Faker _faker;
        private readonly JsonSerializerOptions _jsonOptions;

        public PedidosControllerIntegrationTests()
        {
            _factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.UseEnvironment("Test");

                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll<DbContextOptions<ApplicationDbContext>>();

                        services.AddDbContext<ApplicationDbContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb_" + Guid.NewGuid());
                            options.EnableSensitiveDataLogging();
                        });
                    });
                });

            _client = _factory.CreateClient();
            _faker = new Faker("pt_BR");

            var scope = _factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }

        public async Task DisposeAsync()
        {
            await _dbContext.Database.EnsureDeletedAsync();
            await _dbContext.DisposeAsync();
            _client.Dispose();
            _factory.Dispose();
        }

        [Fact]
        public async Task GET_PegarPeloPedidoId_DeveRetornar200OK()
        {
            // Arrange
            var pedidoId = 999;
            var command = new CalcularImpostoCommand(pedidoId, _faker.Random.Int(1, 10), PedidoItemModelBuilder.CriarItensValidos());
            var createResponse = await _client.PostAsJsonAsync("/api/pedidos", command, _jsonOptions);
            createResponse.EnsureSuccessStatusCode();

            // Act
            var response = await _client.GetAsync($"/api/pedidos/{pedidoId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var conteudo = await response.Content.ReadFromJsonAsync<PedidoTributadoModel>(_jsonOptions);
            conteudo.Should().NotBeNull();
            conteudo!.PedidoId.Should().Be(pedidoId);
        }

        [Fact]
        public async Task GET_PegarTodos_DeveRetornar200OKComAListaVazia()
        {
            _dbContext.PedidosTributados.RemoveRange(_dbContext.PedidosTributados);
            await _dbContext.SaveChangesAsync();

            var response = await _client.GetAsync("/api/pedidos");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var conteudo = await response.Content.ReadFromJsonAsync<ResultadoPaginadoModel<List<PedidoTributadoModel>>>(_jsonOptions);
            conteudo.Should().NotBeNull();
            conteudo.Itens.Should().BeEmpty();
        }

        [Fact]
        public async Task POST_CalcularImposto_DeveRetornar201Created()
        {
            // Arrange
            var command = new CalcularImpostoCommand(
                _faker.Random.Int(1, 1000),
                _faker.Random.Int(1, 10),
                PedidoItemModelBuilder.CriarItensValidos());

            // Act
            var response = await _client.PostAsJsonAsync("/api/pedidos", command, _jsonOptions);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var conteudo = await response.Content.ReadFromJsonAsync<CalcularImpostoResponse>(_jsonOptions);
            conteudo.Should().NotBeNull();
            conteudo!.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task POST_CalcularImposto_DeveRetornar400BadRequest()
        {
            // Arrange
            var command = new CalcularImpostoCommand(
                0,
                0,
                new List<PedidoItemModel>());

            // Act
            var response = await _client.PostAsJsonAsync("/api/pedidos", command, _jsonOptions);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Should().NotBeNull();
        }

        public async Task InitializeAsync() => await _dbContext.Database.EnsureCreatedAsync();

    }
}