using Discord_Bot.Core.UserAccounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Discord_Bot.Core
{
	public static class DataStorage
	{
		public static void SaveUserAccounts(IEnumerable<UserAccount> accounts, string filepath)
		{
			string json = JsonConvert.SerializeObject(accounts);
			File.WriteAllText(filepath, json);
		}

		public static IEnumerable<UserAccount> LoadUserAccount(string filepath)
		{
			if (!File.Exists(filepath)) return null;
			string json = File.ReadAllText(filepath);
			return JsonConvert.DeserializeObject<List<UserAccount>>(json);
		}

		//all of this is mine 

		public static bool SaveExists(string filepath)
		{
			return File.Exists(filepath);
		}

		public static IEnumerable<ShopItem> ListItems(string filepath)
		{
			if (!File.Exists(filepath)) return null;
			string json = File.ReadAllText(filepath);
			var list = JsonConvert.DeserializeObject<IEnumerable<ShopItem>>(json);
			return list;
		}

		public static void SaveItem(IEnumerable<ShopItem> accounts, string filepath)
		{
			string json = JsonConvert.SerializeObject(accounts);
			File.WriteAllText(filepath, json);
		}

		public static IEnumerable<ShopItem> LoadItem(string filepath)
		{
			if (!File.Exists(filepath)) return null;
			string json = File.ReadAllText(filepath);
			return JsonConvert.DeserializeObject<List<ShopItem>>(json);
		}

		public static bool SaveItemExists(string filepath)
		{
			return File.Exists(filepath);
		}
	}
}
