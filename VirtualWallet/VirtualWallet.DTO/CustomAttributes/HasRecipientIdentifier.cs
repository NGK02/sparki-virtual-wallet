using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Dto.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class HasRecipientIdentifier : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string recipientUsername = (string)validationContext.ObjectType.GetProperty("RecipientUsername").GetValue(validationContext.ObjectInstance, null);

            string recipientEmail = (string)validationContext.ObjectType.GetProperty("RecipientEmail").GetValue(validationContext.ObjectInstance, null);

            string recipientPhoneNumber = (string)validationContext.ObjectType.GetProperty("RecipientPhoneNumber").GetValue(validationContext.ObjectInstance, null);

            //check at least one has a value
            if (string.IsNullOrEmpty(recipientUsername) && string.IsNullOrEmpty(recipientEmail) && string.IsNullOrEmpty(recipientPhoneNumber))
                return new ValidationResult("At least one recipient identifier is required!!");

            return ValidationResult.Success;
        }
    }
}
