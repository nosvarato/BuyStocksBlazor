using BuyStocksBlazor.Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuyStocksBlazor.Shared.Validation
{
    public class TotalValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ErrorMessage = string.Empty;
            EditStock temp = (EditStock)validationContext.ObjectInstance;
            if (temp.Total >= temp.CurrentBalance)
            {
                ErrorMessage = "Purchase exceeds your account balance";
                return new ValidationResult(ErrorMessage);
            }
            else if (temp.Amount <= 0)
            {
                ErrorMessage = "Amount to purchase must be greater then 0";
                return new ValidationResult(ErrorMessage);
            }
            else
            {
                return ValidationResult.Success;
            }
           
        }
    }
}
