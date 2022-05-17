using BuyStocksBlazor.Server.Data;
using BuyStocksBlazor.Shared.Models;
using BuyStocksBlazor.Shared.Models.Export;
using BuyStocksBlazor.Shared.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paging;
using System.Security.Claims;

namespace BuyStocksBlazor.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HistoryController : ControllerBase
    {
        private readonly ApplicationDBContext _applicationDBContext;
        public HistoryController(ApplicationDBContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }
        /// <summary>
        /// Returns list of purchase history for exporting
        /// </summary>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IEnumerable<HistoryExport> GetExportFiltered(string? filter)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IQueryable<StockPurchase> qry = _applicationDBContext.StockPurchases.Where(x => x.UserId == userId).Include(x => x.StockPurchased);
            if (!string.IsNullOrWhiteSpace(filter))
            {

                qry = qry.Where(p => p.StockPurchased.Symbol.ToUpper().Contains(filter.ToUpper()) || p.StockPurchased.CompanyName.ToUpper().Contains(filter.ToUpper()));
            }
            var res = qry.Select(x => new HistoryExport
            {
                Symbol = x.StockPurchased.Symbol,
                CompanyName = x.StockPurchased.CompanyName,
                AmountPurchased = x.AmountPurchased,
                PricePerStock = x.StockPurchased.CurrentPrice,
                Purchased = String.Format("{0:g}", x.Purchased),
                Total = x.AmountPurchased * x.StockPurchased.CurrentPrice
            });
            return res.ToList();
        }
        /// <summary>
        /// Returns paged list of purchase history.
        /// </summary>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ReturnModel> GetPagedAsync([FromBody] Filter filter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IQueryable<StockPurchase> qry = _applicationDBContext.StockPurchases.Where(x=>x.UserId == userId).Include(x=>x.StockPurchased);
            if (!string.IsNullOrWhiteSpace(filter.FilterValue))
            {

                qry = qry.Where(p => p.StockPurchased.Symbol.ToUpper().Contains(filter.FilterValue.ToUpper()) || p.StockPurchased.CompanyName.ToUpper().Contains(filter.FilterValue.ToUpper()));
            }
            var tempmodel = await PagingList.CreateAsync(qry, 15, filter.Page, filter.SortExpression, "-Purchased");

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
    }
}
