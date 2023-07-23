using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VirtualWallet.DataAccess.Models
{
	public class Wallet : Entity
	{
		public int Id { get; set; }

		public int UserId { get; set; }

        [JsonIgnore]
        public List<Balance> Balances { get; set; }

        [JsonIgnore]
        public List<Transfer> Transfers { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}