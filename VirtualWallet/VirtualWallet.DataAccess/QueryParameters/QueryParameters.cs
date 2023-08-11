using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualWallet.DataAccess.QueryParameters
{
    public abstract class QueryParameters
    {
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
    }
}
