namespace ControleEstoque.API.DTOs
{
    public class FormaPagamentoDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public List<PedidoDto> Pedidos { get; set; } = new();
    }
    public class AtualizarFormaPagamentoDto
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
    }
    public class CriarFormaPagamentoDto
    {
        public string Descricao { get; set; }
        public bool Ativo { get; set; } = true;
    }

}