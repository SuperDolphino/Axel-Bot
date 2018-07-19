//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Discord.WebSocket;
//using Discord;
//using Npgsql;
//using Discord.Commands;

//namespace Discord_Bot.Core.Leveling
//{
//	internal static class Leveling
//	{
//		internal static void UserSentMessage(SocketGuildUser user)
//		{

//			UriBuilder uriBuilder = new UriBuilder(Environment.GetEnvironmentVariable("DATABASE_URL"));
//			string connString = string.Format("Host={0};Username={1};Password={2};Database={3}", uriBuilder.Host, uriBuilder.UserName, uriBuilder.Password, await SanitizeDB(uriBuilder.Path));
//			string tableName = "shopitems" + user.Guild;

//			using (NpgsqlConnection conn = new NpgsqlConnection(connString))
//			{
//				conn.Open();
//				try
//				{
//					using (NpgsqlCommand cmd = new NpgsqlCommand())
//					{
//						cmd.Connection = conn;
//						cmd.CommandText = $"CREATE TABLE {tableName} (ID BIGINT, Money int, role varchar(255))";
//						cmd.ExecuteNonQueryAsync();
//					}
//				}
//				catch
//				{
//				}
//				bool exists = false;
//				using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM {tableName} WHERE itemname='{options[0].Trim()}' AND price={price} AND role='{options[1].Trim()}'", conn))
//				using (NpgsqlDataReader reader = cmd.ExecuteReader())
//					while (reader.Read())
//					{
//						exists = true;
//						break;
//					}
//				if (!exists)
//				{
//					using (NpgsqlCommand cmd = new NpgsqlCommand())
//					{
//						cmd.Connection = conn;
//						if (options[1] != "" || options[1] != null)
//						{
//							cmd.CommandText = $"INSERT INTO {tableName} VALUES('{options[0].Trim()}',{price},'{options[1].Trim()}')";
//							await cmd.ExecuteNonQueryAsync();
//						}
//						else
//						{
//							cmd.CommandText = $"INSERT INTO {tableName} VALUES('{options[0].Trim()}',{price},'')";
//							await cmd.ExecuteNonQueryAsync();
//						}
//					}
//				}
//				else
//				{
//					await ReplyAsync("Item already exists.");
//				}

//				var userAccount = UserAccounts.UserAccounts.GetAccount(user);
//				var BronzeRole = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Bronze Pass");
//				var SilverRole = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Silver Pass");
//				var GoldRole = (user as IGuildUser).Guild.Roles.FirstOrDefault(x => x.Name == "Gold Pass");

//				uint XpMultiplier = 1; //default value if has no roles

//				if (user.Roles.Contains(BronzeRole))
//				{
//					XpMultiplier = 2;
//				}
//				else if (user.Roles.Contains(SilverRole))
//				{
//					XpMultiplier = 3;
//				}
//				else if (user.Roles.Contains(GoldRole))
//				{
//					XpMultiplier = 5;
//				}
//				userAccount.Money += XpMultiplier;
//				UserAccounts.UserAccounts.SaveAccount();


//				//var userAccount = UserAccounts.UserAccounts.GetAccount(user);
//				//uint oldwhole = userAccount.Whole;
//				//userAccount.Whole += 1;
//				//UserAccounts.UserAccounts.SaveAccount();
//			}
//		}
//	}
//}
