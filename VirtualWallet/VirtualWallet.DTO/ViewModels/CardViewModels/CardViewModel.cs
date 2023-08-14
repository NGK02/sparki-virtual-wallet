using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.Dto.ViewModels.CustomAttributes;

namespace VirtualWallet.Dto.ViewModels.CardViewModels
{
    //Да се преименува
    public class CardViewModel
    {
        [Required]
        [Range(100, 999, ErrorMessage = "The {0} must be a 3-digit number.")]
        public int CheckNumber { get; set; }

        [Required]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "The {0} must be a 16-digit number.")]
        public long CardNumber { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "The {0} must be between {2} and {1} characters long.")]
        public string CardHolder { get; set; }

        [Required]
        [ValidMonth]
        public string ExpirationMonth { get; set; }

        [Required]
        [ValidYear]
        public string ExpirationYear { get; set; }
    }
}