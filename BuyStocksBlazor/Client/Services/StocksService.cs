using BuyStocksBlazor.Client.Extensions;
using BuyStocksBlazor.Shared.Models;
using BuyStocksBlazor.Shared.Models.Export;
using BuyStocksBlazor.Shared.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Paging;
using System.Net.Http.Json;

namespace BuyStocksBlazor.Client.Services
{

    [Authorize]
    public class StocksService : IDisposable
    {
        internal IJSRuntime _jSRuntime;
        private readonly CustomStateProvider _customStateProvider;
        private readonly HttpClient _httpClient;
        internal string _filter = "";
        internal bool _isNew = false;

        public bool ShowDetails { get; set; }
        public bool ShowEdit { get; set; }
        public string? ModalHeader { get; internal set; }
        public PagingList<Stock>? Model { get; private set; }

        public EditStock? Current { get; private set; }
        public StockPurchase? DetailsCurrent { get; set; }
        public StocksService(IJSRuntime jSRuntime, CustomStateProvider customStateProvider, HttpClient httpClient)
        {
            _jSRuntime = jSRuntime;
            _customStateProvider = customStateProvider;
            _httpClient = httpClient;
        }
     

        public async Task CloseModalDetails()
        {

            DetailsCurrent = null;
            ShowDetails = false;
            await Get(_filter, Model.PageIndex, Model.SortExpression);
        }
        public void ShowModalEdit(Stock item)
        {
            Current = new EditStock()
            {
                Amount = 0,
                CurrentBalance = _customStateProvider.LoggedInUser.CurrentBalance,
                CurrentStock = item
            };
            ShowEdit = true;
           
             ModalHeader = $"Buying";
            _isNew = false;
            
        }
        public void ShowModalDetail(StockPurchase item)
        {
            DetailsCurrent = item;
            ShowDetails = true;

            ModalHeader = $"Stock Purchased";
            

        }
        public async Task CloseModalEdit()
        {

            Current = null;
            ShowEdit = false;
            

        }

        public async Task Get(string filter, int page = 1, string sortExpression = "Symbol")
        {
            Filter filt = new Filter()
            {
                FilterValue = filter,
                Page = page,
                SortExpression = sortExpression
            };
            var result = await _httpClient.PostAsJsonAsync("api/BuyStocks/GetPaged", filt);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            var contents = await result.Content.ReadAsStringAsync();
            result.EnsureSuccessStatusCode();
            var temp = JsonConvert.DeserializeObject<ReturnModel>(contents);
           
            var tempvalues = JsonConvert.SerializeObject(temp.Values);
            Model = PagingList.Create(JsonConvert.DeserializeObject<List<Stock>>(tempvalues),15,temp.PageIndex,temp.TotalRecordCount,temp.SortExpression,temp.DefaultSortExpression);



        }
        public async Task EditSave(EditStock item)
        {
            var result = await _httpClient.PostAsJsonAsync("api/BuyStocks/AddBuyStock",item);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            var newlyadded = JsonConvert.DeserializeObject<StockPurchase>(await result.Content.ReadAsStringAsync());
            await _customStateProvider.GetCurrentUser(true);
            await CloseModalEdit();
            ShowModalDetail(newlyadded);


        }



        public async Task Export(string? filter)
        {
            string api = "api/BuyStocks/GetExportFiltered";
            if (!string.IsNullOrWhiteSpace(filter))
            {
                api += $"?filter={filter}";
            }
            var result = await _httpClient.GetAsync(api);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            var contents = await result.Content.ReadAsStringAsync();
            result.EnsureSuccessStatusCode();
            var export = JsonConvert.DeserializeObject<List<StockExport>>(contents);
            await _jSRuntime.ExportExcel(export, "Stock Export.xlsx");
        }
        public void Dispose()
        {
            Model = null;
            GC.SuppressFinalize(this);
        }

    }
}