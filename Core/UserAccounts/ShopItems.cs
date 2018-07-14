using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot.Core.UserAccounts
{
	public static class ShopItems
	{
		private static List<ShopItem> shopItems;

		private static string ShopFile = "Resources/ShopItems.json";
		static ShopItems()
		{
			if (DataStorage.SaveExists(ShopFile))
			{
				shopItems = DataStorage.LoadItem(ShopFile).ToList();
			}
			else
			{
				shopItems = new List<ShopItem>();
				SaveItem();
			}
		}

		public static void SaveItem()
		{
			DataStorage.SaveItem(shopItems, ShopFile);
		}

		public static void RemoveItem(ShopItem item)
		{
			shopItems.Remove(item);
		}

		public static ShopItem GetItem(ShopItem item)
		{

			return GetOrCreateAccount(item);
		}

		private static ShopItem GetOrCreateAccount(ShopItem item)
		{
			var result = from a in shopItems
						 where a.itemName == item.itemName
						 where a.price == item.price
						 select a;

			var account = result.FirstOrDefault();
			if (account == null)
			{
				account = CreateItem(item);
			}
			return account;
		}

		private static ShopItem CreateItem(ShopItem id)
		{
			var newAccount = new ShopItem()
			{
				itemName = id.itemName,
				price = id.price,
				rolename = id.rolename
				
				
			};
			shopItems.Add(newAccount);
			SaveItem();
			return newAccount;
		}
	}
}
