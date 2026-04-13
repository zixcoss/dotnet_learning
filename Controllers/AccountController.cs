using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using dotnet_learning.DTOs.Account;
using dotnet_learning.Entities;
using dotnet_learning.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
//using dotnet_learning.Models;

namespace dotnet_learning.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IAccountService AccountService { get; }
        public AccountController(IAccountService accountService)
        {
            AccountService = accountService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Register(RegisterRequest request)
        {
            try
            {
                var account = request.Adapt<Account>();
                await AccountService.Register(account);
                return StatusCode((int)HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Login(LoginRequest request)
        {
            var account = await AccountService.Login(request.Username, request.Password);
            if(account == null)
            {
                return Unauthorized();
            }       
            return Ok(new {token = AccountService.GenerateToken(account)});
        }
        
        
    }
}