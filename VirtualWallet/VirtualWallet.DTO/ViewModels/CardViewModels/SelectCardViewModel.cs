﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Dto.ViewModels.CardViewModels
{
    public class SelectCardViewModel
    {
        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required]
        [Range(100, 999, ErrorMessage = "The {0} must be a 3-digit number.")]
        public int CheckNumber { get; set; }

        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "The {0} must be a 16-digit number.")]
        public long CardNumber { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "The {0} must be between {2} and {1} characters long.")]
        public string CardHolder { get; set; }
    }
}