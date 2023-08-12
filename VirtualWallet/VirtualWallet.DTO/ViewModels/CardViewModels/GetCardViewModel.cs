using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.Dto.ViewModels.CardViewModels
{
    public class GetCardViewModel
    {
        public int Id { get; set; }

        public long CardNumber { get; set; }

        public string CardHolder { get; set; }

        public string ExpirationMonth { get; set; }

        public string ExpirationYear { get; set; }

        public int CheckNumber { get; set; }
    }
}
