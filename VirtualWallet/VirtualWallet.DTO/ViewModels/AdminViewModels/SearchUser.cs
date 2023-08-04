using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualWallet.DataAccess.Models;

namespace VirtualWallet.Dto.ViewModels.AdminViewModels
{
    public class SearchUser
    {
		public int? Page { get; set; }
		public string SearchOption { get; set; }

        public string SearchOptionValue { get; set; }

        public List<User> users { get; set; } = new List<User>();
    }
}
