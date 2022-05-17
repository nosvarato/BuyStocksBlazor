using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyStocksBlazor.Server.Data;
using BuyStocksBlazor.Server.Models;
using BuyStocksBlazor.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuyStocksBlazor.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDBContext _applicationDBContext;
        

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDBContext applicationDBContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationDBContext = applicationDBContext;
            
            
        }
        /// <summary>
        /// User Login.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return BadRequest("User does not exist");
            var singInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!singInResult.Succeeded) return BadRequest("Invalid password");
            await _signInManager.SignInAsync(user, request.RememberMe);
            return Ok();
        }
        /// <summary>
        /// User register
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterRequest parameters)
        {
            var user = new ApplicationUser();
            user.UserName = parameters.UserName;
            user.CurrentBalance = 0;
            
            var result = await _userManager.CreateAsync(user, parameters.Password);
            if (!result.Succeeded) return BadRequest(result.Errors.FirstOrDefault()?.Description);
            
            var tempUser = await _userManager.FindByNameAsync(user.UserName);
            var newaccount = new AccountBalance()
            {
                CurrentBalance = 5000000,
                UserId = tempUser.Id
            };
            var res = _applicationDBContext.Add(newaccount);
            await _applicationDBContext.SaveChangesAsync();
            return await Login(new LoginRequest
            {
                UserName = parameters.UserName,
                Password = parameters.Password
            });
        }
        /// <summary>
        /// Logout
        /// </summary>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
        /// <summary>
        /// Returns current logged in user (fallback).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<CurrentUser> CurrentUserInfo1()
        {
            var current = new CurrentUser
            {
                
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                Claims = User.Claims
                .ToDictionary(c => c.Type, c => c.Value)
            };
            
            
            try
            {
                if (current.UserName != null)
                {
                    var tempUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    var cuser = await _applicationDBContext.AccountBalances.Where(x => x.UserId == tempUser.Id).FirstAsync();
                    //await _context.Entry(cuser).ReloadAsync();
                    current.CurrentBalance = cuser.CurrentBalance;
                }
            }
            catch (Exception)
            {
            }
          
            
            return current;
        }
    }
}