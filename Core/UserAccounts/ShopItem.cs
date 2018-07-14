using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot.Core.UserAccounts
{
	public class ShopItem
	{
		public string itemName { get; set; }
		public uint price { get; set; }
		public string rolename { get; set; } = "";
	}
}
