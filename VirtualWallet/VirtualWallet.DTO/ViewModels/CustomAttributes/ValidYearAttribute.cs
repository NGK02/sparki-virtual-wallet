using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Dto.ViewModels.CustomAttributes
{
    public class ValidYearAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string yearString && !string.IsNullOrWhiteSpace(yearString))
            {
                if (int.TryParse(yearString, out int yearNumber) && yearNumber >= DateTime.Now.Year)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult("Invalid year.");
        }
    }
}