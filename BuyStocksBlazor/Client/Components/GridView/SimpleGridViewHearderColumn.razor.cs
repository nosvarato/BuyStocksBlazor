using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Client.Components.GridView
{
    public partial class SimpleGridViewHearderColumn<TItem>
    {
        [CascadingParameter(Name = "Grid")]
        public SimpleGridView<TItem> Grid { get; set; }

        /// <summary>
        /// Column can be sorted
        /// </summary>
        [Parameter]
        public bool Sortable { get; set; }

        [Parameter]
        public string Title { get; set; }

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
