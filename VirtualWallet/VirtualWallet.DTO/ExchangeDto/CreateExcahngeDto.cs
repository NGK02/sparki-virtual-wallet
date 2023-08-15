using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Dto.ExchangeDto
{
    public class CreateExcahngeDto
    {
        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        [Range(0.01, (double)decimal.MaxValue, ErrorMessage = "The {0} must be greater than 0.")]
        public decimal Amount { get; set; }

    }
}
