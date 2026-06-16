using ControleEstoque.API.Models;

namespace ControleEstoque.API.Services
{
    public interface ITokenService
    {
        string GerarToken(Usuario usuario);
    }
}
