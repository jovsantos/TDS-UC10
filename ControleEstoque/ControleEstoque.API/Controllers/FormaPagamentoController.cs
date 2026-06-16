using ControleEstoque.API.DTOs;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControleEstoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class FormaPagamentoController : ControllerBase
    {
        private readonly IFormaPagamentoService _formaPagamentoService;

        public FormaPagamentoController(IFormaPagamentoService formaPagamentoService)
        {
            _formaPagamentoService = formaPagamentoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodosAsync()
        {
            var formasPagamento = await _formaPagamentoService.
                ObterTodosAsync();
            return Ok(formasPagamento);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var formaPagamento = await _formaPagamentoService.
                ObterPorIdAsync(id);
            if (formaPagamento == null) 
                return NotFound();
            if (User.FindFirst(ClaimTypes.Role)?.Value == "Gerente") 
            {
                var pedidos = await _formaPagamentoService
                    .ObterPorIdAsync(id);
                if (pedidos == null) 
                    
                    return NotFound();

            }

            return Ok(formaPagamento);
        }

        [HttpPost]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Criar([FromBody] CriarFormaPagamentoDto dto)
        {
            var formaPagamento = await _formaPagamentoService.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = formaPagamento.Id }, formaPagamento);
        }

        [HttpPut]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarFormaPagamentoDto dto)
        {
            if (id != dto.Id) return BadRequest("ID não correspondente");

            await _formaPagamentoService.
                AtualizarAsync(dto);
            return NoContent();

        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Remover(int id)
        {
            await _formaPagamentoService.RemoverAsync(id);
            return NoContent();
        }
    }
}