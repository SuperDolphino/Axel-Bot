using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot.Core.UserAccounts
{
	public class UserAccount
	{
		public ulong ID { get; set; }

		public uint Money { get; set; }

		public uint XP { get; set; }

		public List<ShopItem> boughtItems { get; set; }
	}
}
