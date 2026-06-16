using System.ComponentModel.DataAnnotations;
using ControleEstoque.API.Models;

namespace ControleEstoque.API
{
    public class FormaPagamento
    {
        [Key]
        public int Id { get; set; }
        public bool Ativo { get; set; }
        [Required, StringLength(50)]
        public string Descricao { get; set; }
        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}
