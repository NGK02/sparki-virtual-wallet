using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Dto.CustomAttributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class PasswordRequirementsAttribute : ValidationAttribute
	{
		public override bool IsValid(object value)
		{
			if (value == null)
				return true;

			var password = value.ToString();

			if (password.Length < 8)
			{
				ErrorMessage = "Password must be at least 8 characters long.";
				return false;
			}

			if (!password.Any(char.IsUpper))
			{
				ErrorMessage = "Password must contain at least one uppercase letter.";
				return false;
			}

			if (!password.Any(char.IsDigit))
			{
				ErrorMessage = "Password must contain at least one digit.";
				return false;
			}

			var specialSymbols = new char[] { '+', '-', '*', '&', '^', '…', '!', '@', '#', '$', '%', '_', '=', '{', '}', '[', ']', '|', '\\', '/', ':', ';', '"', '\'', '<', '>', ',', '.', '?', '~' };
			bool containsSpecialSymbol = false;
			foreach (char symbol in specialSymbols)
			{
				if (password.Contains(symbol))
				{
					containsSpecialSymbol = true;
					break;
				}
			}

			if (!containsSpecialSymbol)
			{
				ErrorMessage = "Password must contain at least one special symbol!";
				return false;
			}

			return true;
		}
	}
}
