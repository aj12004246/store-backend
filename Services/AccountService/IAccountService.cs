using Microsoft.Extensions.Primitives;
using store_be.Models;

namespace store_be.Services.AccountService
{
    public interface IAccountService
    {
        Task<List<Account>> GetAllAccounts();
        Task<Account?> GetAccountById(int id);
        Task<Account?> GetAccountByEmail(string email);
        Task<Account?> GetAccountByEmailAndPassword(string email, string password);
        Task AddAccount(Account account);
        Task UpdateAccount(int id, Account requested);
        Task DeleteAccount(int id);
    }
}
