using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ParrotWingsApp.Data.Model;
using ParrotWingsApp.Data.Services;
using ParrotWingsApp.WebApi.Helpers;

namespace ParrotWingsApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionsService _transactionsService;

        public TransactionsController(TransactionsService transactionService, UsersService usersService)
        {
            _transactionsService = transactionService;
        }

        [HttpGet]
        [Authorize]
        public IEnumerable<TransactionDto> GetTransactions()
        {
            var userId = HttpContext.User.Identity.Name;
            return _transactionsService.GetTransactionHistory(Guid.Parse(userId));
        }

        [HttpPost("addtransaction")]
        [Authorize]
        public IActionResult AddTransactions(TransactionDto newTransaction)
        {
            try
            {
                var userId = HttpContext.User.Identity.Name;
                var amount = newTransaction.Amount;

                _transactionsService.AddTransaction(Guid.Parse(userId),newTransaction.RecipientId,amount);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new { errorText = e.Message });
            }


        }
    }
}
