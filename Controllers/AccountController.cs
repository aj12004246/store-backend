using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using store_be.Models;
using store_be.Services.AccountService;


namespace optum_fs_asp_22_team_1_be.Controllers
{
    [EnableCors("localhost")]
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase   
    {
        private readonly IAccountService _accountService;


        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        //GET ALL ACCOUNTS
        [HttpGet]
        public async Task<ActionResult<List<Account>>> GetAllAccounts() { 
            return  await _accountService.GetAllAccounts();
        }


        //GET SINGLE ACCOUNT BY ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Account>> GetAccountById(int id)
        {
            try
            {
                var result = await _accountService.GetAccountById(id);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound("Account not found.");
            }
            
            
        }

       
        //GET SINGLE ACCOUNT BY EMAIL
        [HttpGet("{email}")]
        public async Task<ActionResult<Account>> GetAccountbyEmail(string email)
        {
            try
            {
                var result = await _accountService.GetAccountByEmail(email);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound("Account not found.");
            }
            
        }

       
        //GET SINGLE ACCOUNT BY EMAIL AND PASSWORD
        [HttpGet("{email}/{password}")]
        public async Task<ActionResult<Account>> GetAccountbyEmailAndPassword(string email, string password)
        {
            try
            {
                var result = await _accountService.GetAccountByEmailAndPassword(email, password);
                return Ok(result);
            }
            catch (Exception)
            {
                return NotFound("Username/Password Incorrect");
            }
    
            
        }
        

        //POST METHOD
        [HttpPost]
        public async Task<ActionResult> AddAccount(Account account)
        {
            try
            {
                await _accountService.AddAccount(account);
                return Ok();
            }
            catch (Exception)
            { 
                return Conflict();
            }          
        }

    

        //UPDATE ACCOUNT BY ID
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAccount(int id, Account requested)
        {
            try
            {
                await _accountService.UpdateAccount(id, requested);
                return Ok();
            }
            catch (Exception)
            {

                return NotFound("Account not found.");
            }
            
        }



        //DELETE ACCOUNT
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Account>>> DeleteAccount(int id)
        {
            try
            {
                await _accountService.DeleteAccount(id);
                return Ok();
            }
            catch (Exception)
            {
                return NotFound("Account not found.");
            }
        }

    }
}





