using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using BuyStocksBlazor.Client.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Client.Components.Validators
{
    public class DIDataAnnotationsValidator : DataAnnotationsValidator
    {
        [Inject]
        protected IServiceProvider ServiceProvider { get; set; }
        [CascadingParameter] private EditContext DICurrentEditContext { get; set; }

        
        protected override void OnInitialized()
        {
            if (DICurrentEditContext == null)
            {
                throw new InvalidOperationException($"{nameof(DataAnnotationsValidator)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. For example, you can use {nameof(DataAnnotationsValidator)} " +
                    $"inside an EditForm.");
            }
           // DICurrentEditContext.EnableDataAnnotationsValidation();
           DICurrentEditContext.AddDataAnnotationsValidationWithDI(ServiceProvider);
        }
        protected override void OnParametersSet()
        {
            
        }
    }
}
