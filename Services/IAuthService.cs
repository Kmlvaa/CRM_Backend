using CRM.DTOs.Account;

namespace CRM.Services
{
    public interface IAuthService
    {
        Task<(int, string, string)> Login(LoginDTO dto);
        Task<(int, string)> Register(RegisterDTO dto, string role);
    }
}
