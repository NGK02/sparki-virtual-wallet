using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Dto.ViewModels.CustomAttributes
{
    public class ValidMonthAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string monthString && !string.IsNullOrWhiteSpace(monthString))
            {
                if (int.TryParse(monthString, out int monthNumber) && monthNumber >= 1 && monthNumber <= 12)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Invalid month.");
        }
    }
}