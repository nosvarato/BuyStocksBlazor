using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Client.Components.GridView
{
    public partial class SimpleGridView<TItem>
    {
        [Parameter]
        public List<TItem> Items { get; set; }

        [Parameter]
        public RenderFragment GridHeaderColumns { get; set; }

        [Parameter]
        public RenderFragment<TItem> GridRow { get; set; }

        [Parameter]
        public String TitleHeader { get; set; }
        public List<SimpleGridViewHearderColumn<TItem>> HeaderColumns { get; } = new List<SimpleGridViewHearderColumn<TItem>>();


        public void AddColumn(SimpleGridViewHearderColumn<TItem> column)
        {
            column.Grid = this;
            HeaderColumns.Add(column);
            StateHasChanged();
        }
    }
}
