using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Client.Components.GridView
{
    public partial class GridView<TItem>
    {
        [Parameter]
        public PagingList<TItem> Items { get; set; }

        [Parameter]
        public RenderFragment GridHeaderColumns { get; set; }

        [Parameter]
        public RenderFragment<TItem> GridRow { get; set; }
        [Parameter]
        public RenderFragment HeaderButtonsAlways { get; set; }
        [Parameter]
        public RenderFragment  HeaderButtons { get; set; }
        [Parameter]
        public bool ShowCustomSearch { get; set; }
        [Parameter]
        public RenderFragment CustomSearch { get; set; }
        [Parameter]
        public bool DisableSearchHeader { get; set; } = false;
        [Parameter]
        public bool DisableSchoolHeader { get; set; } = false;
        [Parameter]
        public EventCallback<SearchValues> SearchChanged { get; set; }
        [Parameter]
        public EventCallback<SearchValues> ExportChanged { get; set; }

        public List<GridHeaderColumn<TItem>> HeaderColumns { get; } = new List<GridHeaderColumn<TItem>>();
        private int _selectedSchool = 0;

        public void AddColumn(GridHeaderColumn<TItem> column)
        {
            column.Grid = this;
            HeaderColumns.Add(column);
            StateHasChanged();
        }
#pragma warning disable IDE1006 // Naming Styles
        public string filter { get; private set; } = "";
#pragma warning restore IDE1006 // Naming Styles
        public string _filter { get; private set; } = "";
        public async void Enter(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await SearchClicked();
            }
        }
        async Task SortingChanged(string _sortExpression)
        {
            var search = new SearchValues
            {
                Filter = filter,
                Page = (Items.PageIndex > Items.PageCount ? 1 : Items.PageIndex),
                SortExpression = _sortExpression 
            };
            await SearchChanged.InvokeAsync(search);
        }

        async Task SchoolChanged(int _page)
        {
            _selectedSchool = _page;
            _filter = "";
            filter = "";
            var search = new SearchValues
            {
                Filter = filter,
                Page = (1),
                SortExpression = Items.SortExpression
            };
            await SearchChanged.InvokeAsync(search);

        }

        async Task PagingChanged(int _page)
        {
            var search = new SearchValues
            {
                Filter = filter,
                Page = (_page > Items.PageCount ? 1 : _page),
                SortExpression = Items.SortExpression
            };
            await SearchChanged.InvokeAsync(search);

        }
        async Task SearchClicked()
        {
            filter = _filter;
            var search = new SearchValues
            {
                Filter = filter,
                Page = (Items.PageIndex > Items.PageCount ? 1 : Items.PageIndex),
                SortExpression = Items.SortExpression
            };
            await SearchChanged.InvokeAsync(search);

        }
        async Task ExportClicked()
        {
            filter = _filter;
            var search = new SearchValues
            {
                Filter = filter,
                Page = (Items.PageIndex > Items.PageCount ? 1 : Items.PageIndex),
                SortExpression = Items.SortExpression
            };
            
            await ExportChanged.InvokeAsync(search);

        }
    }
}
