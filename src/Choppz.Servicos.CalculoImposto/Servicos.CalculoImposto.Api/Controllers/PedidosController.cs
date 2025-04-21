using MediatR;
using Microsoft.AspNetCore.Mvc;
using Servicos.CalculoImposto.Application.Commands.CalcularImposto;
using Servicos.CalculoImposto.Application.Models;
using Servicos.CalculoImposto.Application.Queries.PegarPeloProdutoId;
using Servicos.CalculoImposto.Application.Queries.PegarTodos;

namespace Servicos.CalculoImposto.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PedidosController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CalcularImpostoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadronizadaModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CalcularImposto([FromBody] CalcularImpostoCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.ResultadoValidacao || !result.Sucesso)
                return BadRequest(result.Data);

            return CreatedAtAction(nameof(CalcularImposto), new { id = result.Data }, result.Data);
        }

        [HttpGet("{pedidoId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(PedidoTributadoModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadronizadaModel), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PegarPeloPedidoId([FromRoute] int pedidoId)
        {
            var result = await _mediator.Send(new PegarPeloPedidoIdQuery(pedidoId));

            if (!result.ResultadoValidacao)
                return BadRequest(result.Data);

            if (result.ResultadoValidacao && !result.Sucesso)
                return NotFound(result.Data);

            return Ok(result.Data);
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ResultadoPaginadoModel<PedidoTributadoModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(RespostaPadronizadaModel), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PegarTodos([FromQuery] RequestPaginadoModel request)
        {
            var result = await _mediator.Send(new PegarTodosQuery(request.Status, request.Pagina, request.ItensPorPagina));

            if (!result.ResultadoValidacao)
                return BadRequest(result.Data);

            return Ok(result.Data);
        }
    }
}