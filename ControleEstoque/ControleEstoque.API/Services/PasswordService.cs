using BCrypt.Net;

namespace ControleEstoque.API.Services
{
    public class PasswordService : IPasswordService
    {
        /// <summary>
        /// Gera um hash seguro da senha usando BCrypt
        /// </summary>
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        /// <summary>
        /// Verifica se a senha corresponde ao hash armazenado
        /// </summary>
        public bool VerifyPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
