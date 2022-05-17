using BuyStocksBlazor.Server.Data;
using BuyStocksBlazor.Server.Models;
using BuyStocksBlazor.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuyStocksBlazor.Server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        private readonly ApplicationDBContext _applicationDBContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(ApplicationDBContext applicationDBContext, UserManager<ApplicationUser> userManager)
        {
            
            _applicationDBContext = applicationDBContext;
            _userManager = userManager;
        }
        /// <summary>
        /// Returns current logged in user.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<CurrentUser> CurrentUserInfo()
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
