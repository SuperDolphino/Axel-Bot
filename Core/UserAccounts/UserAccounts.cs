using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace Discord_Bot.Core.UserAccounts
{
	public static class UserAccounts
	{
		private static List<UserAccount> accounts;

		private static string accountsFile = "Resources/accounts.json";
		static UserAccounts()
		{
			if (DataStorage.SaveExists(accountsFile))
			{
				accounts = DataStorage.LoadUserAccount(accountsFile).ToList();
			}
			else
			{
				accounts = new List<UserAccount>();
				SaveAccount();
			}
		}

		public static void SaveAccount()
		{
			DataStorage.SaveUserAccounts(accounts, accountsFile);
		}

		public static UserAccount GetAccount(IGuildUser user)
		{

			return GetOrCreateAccount(user.GuildId);
		}

		private static UserAccount GetOrCreateAccount(ulong id)
		{
			var result = from a in accounts
						 where a.ID == id
						 select a;

			var account = result.FirstOrDefault();
			if (account == null)
			{
				account = CreatUserAccount(id);
			}
			return account;
		}

		private static UserAccount CreatUserAccount(ulong id)
		{
			var newAccount = new UserAccount()
			{
				boughtItems = new List<ShopItem>(),
				ID = id,
				Money = 1,

			};
			accounts.Add(newAccount);
			SaveAccount();
			return newAccount;
		}
	}
}
