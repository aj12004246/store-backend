using Microsoft.AspNetCore.Mvc;
using store_be.Models;
using System.Reflection;

namespace store_be.Services.AccountService
{
    public class AccountService : IAccountService
    {


        private readonly DataContext _context;

        public AccountService(DataContext context)
        {
            _context = context; 
        }


        //Get All Accounts
        public async Task<List<Account>> GetAllAccounts()
        {
            var account = await _context.Accounts.ToListAsync();
            return account;
        }

        //Get Account by ID
        public async Task<Account?> GetAccountById(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account is null)
            {
                throw new Exception("Account not found");
            }
            
            return account;
        }

        //Get Account by Email
        public async Task<Account?> GetAccountByEmail(string email)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(account => account.Email == email);
            if (account is null)
            {
                throw new Exception("Account not found.");
            }
            
            return account;
        }

        //Get Account by Email and Password
        public async Task<Account?> GetAccountByEmailAndPassword(string email, string password)
        {
            var account = await _context.Accounts.FirstOrDefaultAsync(account => account.Email == email && account.Password == password);
            if (account != null)
                return account;

            throw new Exception("Username taken");
        }


        //Add account
        public async Task AddAccount(Account account)
        {
            var emailExists =  await _context.Accounts.FirstOrDefaultAsync(a => a.Email == account.Email);
            if (emailExists is null)
            { 
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return;
            }

            throw new Exception();

        }

        //Update an Account but Check if Other Accounts Exist prior
        public async Task UpdateAccount(int id, Account requested)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account is null)
            {
                throw new Exception();
            }

            var emailExists = await _context.Accounts.FirstOrDefaultAsync(e => e.Email.Equals(requested.Email));

            var passwordExists = await _context.Accounts.FirstOrDefaultAsync(e => e.Id == requested.Id && e.Password.Equals(requested.Password));



            if (emailExists == null || passwordExists == null)
            {
                account.Email = requested.Email;
                account.Password = requested.Password;
                account.Role = requested.Role;
                account.Id = requested.Id;
                account.Carts = account.Carts;

                await _context.SaveChangesAsync();

                return;
            }

            throw new Exception();
        }

        //Delete Account
        public async Task DeleteAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                throw new Exception();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return;
        }


    }
}
