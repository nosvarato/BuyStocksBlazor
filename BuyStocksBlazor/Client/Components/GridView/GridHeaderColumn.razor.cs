using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Client.Components.GridView
{
    public partial class GridHeaderColumn<TItem>
    {
        [CascadingParameter(Name = "Grid")]
        public GridView<TItem> Grid { get; set; }

        /// <summary>
        /// Column can be sorted
        /// </summary>
        [Parameter]
        public bool Sortable { get; set; }

        [Parameter]
        public bool TemplateColumn { get; set; }
        [Parameter] 
        public string Title { get; set; }
        [Parameter]
        public RenderFragment TitleTemplate { get; set; }

        public string Description => GetDisplayName(Title);

        private string GetDisplayName(string propertyname)
        {
            MemberInfo myprop = typeof(TItem).GetProperty(propertyname) as MemberInfo;
            if (myprop == null)
            {
                return propertyname;
            }
            var dd = myprop.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
            return dd?.Name ?? propertyname;
        }

        protected override void OnInitialized()
        {
            Grid.AddColumn(this);
        }
    }
}
