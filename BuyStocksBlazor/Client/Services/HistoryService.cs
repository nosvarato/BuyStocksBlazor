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
    public class HistoryService : IDisposable
    {
        internal IJSRuntime _jSRuntime;
        private readonly CustomStateProvider _customStateProvider;
        private readonly HttpClient _httpClient;
        internal string _filter = "";
        internal bool _isNew = false;

        public bool ShowDetails { get; set; }
        public string? ModalHeader { get; internal set; }
        public PagingList<StockPurchase>? Model { get; private set; }
        public StockPurchase DetailsCurrent { get; set; }
        public HistoryService(IJSRuntime jSRuntime, CustomStateProvider customStateProvider, HttpClient httpClient)
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
        public void ShowModalDetail(StockPurchase item)
        {
            DetailsCurrent = item;
            ShowDetails = true;

            ModalHeader = $"Stock Purchased";


        }
        public async Task Get(string filter, int page = 1, string sortExpression = "-Purchased")
        {
            Filter filt = new Filter()
            {
                FilterValue = filter,
                Page = page,
                SortExpression = sortExpression
            };
            var result = await _httpClient.PostAsJsonAsync("api/History/GetPaged", filt);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            var contents = await result.Content.ReadAsStringAsync();
            result.EnsureSuccessStatusCode();
            var temp = JsonConvert.DeserializeObject<ReturnModel>(contents);

            var tempvalues = JsonConvert.SerializeObject(temp.Values);
            Model = PagingList.Create(JsonConvert.DeserializeObject<List<StockPurchase>>(tempvalues), 15, temp.PageIndex, temp.TotalRecordCount, temp.SortExpression, temp.DefaultSortExpression);



        }
        public async Task Export(string? filter)
        {
            string api = "api/History/GetExportFiltered";
            if (!string.IsNullOrWhiteSpace(filter))
            {
                api += $"?filter={filter}";
            }
            var result = await _httpClient.GetAsync(api);
            if (result.StatusCode == System.Net.HttpStatusCode.BadRequest) throw new Exception(await result.Content.ReadAsStringAsync());
            var contents = await result.Content.ReadAsStringAsync();
            result.EnsureSuccessStatusCode();
            var export = JsonConvert.DeserializeObject<List<HistoryExport>>(contents);
            await _jSRuntime.ExportExcel(export, "History Export.xlsx");
        }
        public void Dispose()
        {
            Model = null;
            GC.SuppressFinalize(this);
        }
    }
}
