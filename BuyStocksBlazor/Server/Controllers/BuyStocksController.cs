using BuyStocksBlazor.Server.Data;
using BuyStocksBlazor.Shared.Models;
using BuyStocksBlazor.Shared.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Paging;
using System.Security.Claims;

namespace BuyStocksBlazor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BuyStocksController : ControllerBase
    {
        
        private readonly ApplicationDBContext _applicationDBContext;

        public BuyStocksController(ApplicationDBContext applicationDBContext)
        {
            
            _applicationDBContext = applicationDBContext;
        }
        /// <summary>
        /// Return list of stocks for export
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public  IEnumerable<Stock> GetExportFiltered(string? filter)
        {
            
            IQueryable<Stock> qry = _applicationDBContext.Stocks;
            if (!string.IsNullOrWhiteSpace(filter))
            {

                qry = qry.Where(p => p.Symbol.ToUpper().Contains(filter.ToUpper()) || p.CompanyName.ToUpper().Contains(filter.ToUpper()));
            }
            return qry.ToList();
        }
        /// <summary>
        /// Returns paged list of Stocks
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ReturnModel> GetPagedAsync([FromBody] Filter filter)
        {
            
            IQueryable<Stock> qry = _applicationDBContext.Stocks;
            if (!string.IsNullOrWhiteSpace(filter.FilterValue))
            {

                qry = qry.Where(p => p.Symbol.ToUpper().Contains(filter.FilterValue.ToUpper()) || p.CompanyName.ToUpper().Contains(filter.FilterValue.ToUpper()));
            }
            var tempmodel = await PagingList.CreateAsync(qry, 15, filter.Page, filter.SortExpression, "Symbol");

            var model = new ReturnModel()
            {
                DefaultSortExpression = tempmodel.DefaultSortExpression,
                PageIndex = tempmodel.PageIndex,
                PageCount = tempmodel.PageCount,
                SortExpression = tempmodel.SortExpression,
                TotalRecordCount = tempmodel.TotalRecordCount,
                Values = tempmodel.ToList(),
                SortExpressionParameterName = tempmodel.SortExpressionParameterName
            };
            return model;
        }
        /// <summary>
        /// Create new stock purchase and returns History object
        /// </summary>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StockPurchase>> AddBuyStock([FromBody]EditStock newStock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var temp = new StockPurchase()
            {
                AmountPurchased = newStock.Amount,
                Purchased = DateTime.Now,
                StockId= newStock.CurrentStock.StockId,
                UserId = userId,
                PricePerStock = newStock.CurrentStock.CurrentPrice
            };
            try
            {
                
             
                
                var res = _applicationDBContext.Add(temp);
                var user = await _applicationDBContext.AccountBalances.Where(x => x.UserId == userId).FirstAsync();
                if (user != null)
                {
                    user.CurrentBalance -= (temp.AmountPurchased * newStock.CurrentStock.CurrentPrice);
                    _applicationDBContext.Update(user);
                }
                await _applicationDBContext.SaveChangesAsync();
                var user1 = await _applicationDBContext.AccountBalances.Where(x => x.UserId == userId).FirstAsync();
            }
            catch (Exception)
            {

                return BadRequest("Saving Failed");
            }
            temp.StockPurchased = newStock.CurrentStock;
             return temp;
            
        }

    }
}
