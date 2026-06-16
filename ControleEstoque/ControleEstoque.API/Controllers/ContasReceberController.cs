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
    public class ContasReceberController : ControllerBase
    {
        private readonly IContaReceberService _contaReceberService;

        public ContasReceberController(IContaReceberService contaReceberService)
        {
            _contaReceberService = contaReceberService;
        }

        [HttpGet]
        [Authorize(Roles = "Gerente,Caixa")]
        public async Task<IActionResult> GetAll()
        {
            var contas = await _contaReceberService.ObterTodosAsync();
            return Ok(contas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var conta = await _contaReceberService.ObterPorIdAsync(id);
            if (conta == null) return NotFound();

            //if (User.IsInRole("Cliente")) // isso é outra forma de fazer a msm coisa
            if(User.FindFirst(ClaimTypes.Role)?.Value == "Cliente")
            {
                var clienteId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(clienteId != conta.ClienteId.ToString()) return Unauthorized();
            }                       

            return Ok(conta);
        }

        [HttpPost]
        [Authorize(Roles = "Gerente,Caixa")]
        public async Task<IActionResult> Create([FromBody] CriarContaReceberDto dto)
        {
            var novaConta = await _contaReceberService.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = novaConta.Id }, novaConta);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Gerente,Caixa")]
        public async Task<IActionResult> Update(int id, [FromBody] AtualizarContaReceberDto dto)
        {
            if (id != dto.Id) return BadRequest("O ID da rota difere do ID da conta a receber.");
            
            await _contaReceberService.AtualizarAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Gerente,Caixa")]
        public async Task<IActionResult> Delete(int id)
        {
            await _contaReceberService.RemoverAsync(id);
            return NoContent();
        }
    }
}