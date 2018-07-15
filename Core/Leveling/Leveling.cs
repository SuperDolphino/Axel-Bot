using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;

namespace Discord_Bot.Core.Leveling
{
	internal static class Leveling
	{
		internal static void UserSentMessage(SocketGuildUser user)
		{
			var userAccount = UserAccounts.UserAccounts.GetAccount(user);

			var BronzeRole = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Bronze Pass");
			var SilverRole = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Silver Pass");
			var GoldRole = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Gold Pass");

			uint XpMultiplier = 1; //default value if has no roles

			if (user.Roles.Contains(BronzeRole))
			{
				XpMultiplier = 2;
			}
			else if (user.Roles.Contains(SilverRole))
			{
				XpMultiplier = 3;
			}
			else if (user.Roles.Contains(GoldRole))
			{
				XpMultiplier = 5;
			}
			userAccount.Money += XpMultiplier;
			UserAccounts.UserAccounts.SaveAccount();


			//var userAccount = UserAccounts.UserAccounts.GetAccount(user);
			//uint oldwhole = userAccount.Whole;
			//userAccount.Whole += 1;
			//UserAccounts.UserAccounts.SaveAccount();
		}
	}
}
