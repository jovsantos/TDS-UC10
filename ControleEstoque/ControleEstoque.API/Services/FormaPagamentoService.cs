using ControleEstoque.API.Data;
using ControleEstoque.API.DTOs;
using ControleEstoque.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleEstoque.API.Services
{
    public class FormaPagamentoService : IFormaPagamentoService
    {
        private readonly AppDbContext _context;

        public FormaPagamentoService(AppDbContext context)
        {
            _context = context;

        }
        public async Task<IEnumerable<FormaPagamentoDto>> ObterTodosAsync() //
        {
            return await _context.FormasPagamento
                 .Include(fp => fp.Id)
                 .AsNoTracking()
                 .Select(fp => new FormaPagamentoDto
                 {
                     Id = fp.Id,
                     Descricao = fp.Descricao,
                     Ativo = fp.Ativo
                 })
                 .ToListAsync();
        }

        public async Task<FormaPagamentoDto?> ObterPorIdAsync(int id)
        {
            var formaPagamento = await _context.FormasPagamento
                .Include(fp => fp.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync(fp => fp.Id == id);

            if (formaPagamento == null) 
                return null;

            return new FormaPagamentoDto
            {
                Id = formaPagamento.Id,
                Descricao = formaPagamento.Descricao,
                Ativo = formaPagamento.Ativo,
                Pedidos = formaPagamento.Pedidos.Select(p => new PedidoDto
                {
                    Id = p.Id,
                    Descricao = p.Descricao,
                    DataPedido = p.DataPedido,
                    Status = p.Status,
                    ClienteId = p.ClienteId
                }).ToList()
            };

        }
        public async Task<FormaPagamentoDto> CriarAsync(CriarFormaPagamentoDto dto)
        {
            var formaPagamento = new FormaPagamento
            {
                Descricao = dto.Descricao,
                Ativo = dto.Ativo
            };

            _context.FormasPagamento.
                Add(formaPagamento);
            await _context.SaveChangesAsync();

            return new FormaPagamentoDto
            {
                Id = formaPagamento.Id,
                Descricao = formaPagamento.Descricao,
                Ativo = formaPagamento.Ativo
            };
        }

        public async Task AtualizarAsync(AtualizarFormaPagamentoDto dto)
        {
            var formaPagamento = await _context.FormasPagamento.
                FindAsync(dto.Id);
            if (formaPagamento != null)
            {
                var formaPagamentoExiste = await _context.FormasPagamento.
                    AnyAsync(fp => fp.Id == dto.Id);
                if (!formaPagamentoExiste)
                {
                    throw new ArgumentException("Essa forma de pagamento não existe");
                }

                formaPagamento.Descricao = dto.Descricao;
                formaPagamento.Ativo = dto.Ativo;

                _context.FormasPagamento.
                    Update(formaPagamento);
                await _context.SaveChangesAsync();
            }
        }
        public async Task RemoverAsync(int id)
        {
            var formaPagamento = await _context.FormasPagamento.
                FindAsync(id);
            if (formaPagamento == null) 
                return;
            _context.FormasPagamento.
                Remove(formaPagamento);
            await _context.SaveChangesAsync();
        }

    }


}