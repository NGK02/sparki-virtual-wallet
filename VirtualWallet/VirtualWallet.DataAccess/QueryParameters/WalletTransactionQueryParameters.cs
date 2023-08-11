﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.DataAccess.QueryParameters
{
    public class WalletTransactionQueryParameters : QueryParameters
    {
        public string SenderUsername { get; set; }
        public string RecipientUsername { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
    }
}
