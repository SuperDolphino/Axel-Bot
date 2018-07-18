using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord_Bot.Core.UserAccounts;
using Discord_Bot.Core;
using Npgsql;
using Discord_Bot.Core.Discord;
using System.Collections.Generic;

namespace Discord_Bot.Modules.ShopSystem
{
	public class Misc : ModuleBase<SocketCommandContext>
	{
		[Command("stats")]
		public async Task MyXP()
		{
			#region MYCODE
			var account = UserAccounts.GetAccount(Context.User as IGuildUser);
			var embed = new EmbedBuilder();
			embed.WithTitle($"{Context.User.Username} Stats:");
			embed.WithColor(new Color(0, 255, 0));
			embed.WithDescription($"You have {account.Money} {Global.Currency}.");
			await Context.Channel.SendMessageAsync("", false, embed);
			#endregion		
		}

		[Command("AddMoney")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task AddXP(IGuildUser user, uint amount)
		{
			var account = UserAccounts.GetAccount(Context.User as IGuildUser);
			account.Money += amount;
			await Context.Channel.SendMessageAsync($"{user.Username} now has {account.Money.ToString()} whole!");
		}

		[Command("staffChannel")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task setChannel(ITextChannel channel)
		{
			Guilds.SetGuildStaffChannel(Context.Guild, channel);
			Global.StaffChannel = channel;
			await Context.Channel.SendMessageAsync($"The Staff channel was set to {channel}");
		}

		[Command("Additem")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task ShopAdd(uint price, [Remainder]string nameAndRole)
		{
			UriBuilder uriBuilder = new UriBuilder(Environment.GetEnvironmentVariable("DATABASE_URL"));
			string connString = string.Format("Host={0};Username={1};Password={2};Database={3}", uriBuilder.Host, uriBuilder.UserName, uriBuilder.Password, await SanitizeDB(uriBuilder.Path));

			string[] options = nameAndRole.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
			string tableName = "ShopItems" + Context.Guild.Id;

			using (NpgsqlConnection conn = new NpgsqlConnection(connString))
			{
				conn.Open(); 
				try 
				{
					using (NpgsqlCommand cmd = new NpgsqlCommand())
					{
						cmd.Connection = conn; 
						cmd.CommandText = $"CREATE TABLE {tableName} (ItemName varchar(255), Price int, Role varchar(255))"; 
						await cmd.ExecuteNonQueryAsync();
					}
				}
				catch
				{
				}
				bool exists = false;
				using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM {tableName} WHERE ItemName='{options[0].Trim()}' AND Price={price} AND Role='{options[1].Trim()}'", conn)) //Try to find the time property of a cooldown where the person who sent the message thanked the current person who's being thanked. If it doesn't exist, the next statement won't be executed
				using (NpgsqlDataReader reader = cmd.ExecuteReader())
					while (reader.Read())
					{
						exists = true;
						break;
					}
						if (!exists) { 
				using (NpgsqlCommand cmd = new NpgsqlCommand())
				{
					cmd.Connection = conn;
					cmd.CommandText = $"INSERT INTO {tableName} VALUES('{options[0].Trim()}',{price},'{options[1].Trim()}')";
					await cmd.ExecuteNonQueryAsync();
					
				}
			}
                else
                {
					await ReplyAsync("Item already exists.");
				}
		}
			string ItemName = options[0].Trim();
			string RoleName = options[1].Trim();
			if (options[1].Trim() == "")
			{
				await Context.Channel.SendMessageAsync($"You added {ItemName} to the shop, it costs {price}");
			}
			else
			{
				if (Guilds.GetGuildConfig(Context.Guild).StaffChannel == null)
				{
					await Context.Channel.SendMessageAsync("Please set the Staff Channel using a/staffChannel");
					return;
				}
				else
				{
					if (RoleName == "Emote" || RoleName== "emote")
					{
						await ReplyAsync($"You added {ItemName} to the shop, it costs {price} , Admins will get notified in {Global.StaffChannel}");
					}
					else if (RoleName == "Custom Emote" || RoleName == "custom emote")
					{
						await ReplyAsync($"You added {ItemName} to the shop, it costs {price} ,Admins will get notified in {Guilds.GetGuildConfig(Context.Guild).StaffChannel}");
					}
					else
					{
						await ReplyAsync($"You added {ItemName} to the shop, it costs {price} , you get the {RoleName} role for buying it");
					}
				}
			}
		}

		[Command("Shop")]
		public async Task Shop()
		{
			UriBuilder uriBuilder = new UriBuilder(Environment.GetEnvironmentVariable("DATABASE_URL"));
			string connString = string.Format("Host={0};Username={1};Password={2};Database={3}", uriBuilder.Host, uriBuilder.UserName, uriBuilder.Password, await SanitizeDB(uriBuilder.Path));
			string tableName = "ShopItems" + Context.Guild.Id;
			List<ShopItem> ShopItems = new List<ShopItem>();
			using (NpgsqlConnection conn = new NpgsqlConnection(connString))
			{
				conn.Open();
				using (NpgsqlCommand cmd = new NpgsqlCommand($"SELECT * FROM {tableName}", conn))
				using (NpgsqlDataReader reader = cmd.ExecuteReader())
					while (reader.Read())
					{
						var newitem = new ShopItem()
						{
							itemName = reader.GetString(0),
							price =  (uint)reader.GetInt32(1),
							rolename = reader.GetString(2)
						};
						ShopItems.Add(newitem);
					}
			}

			var embed = new EmbedBuilder();
			embed.WithTitle("Shop:");
			if (ShopItems.Count() == 0 || ShopItems == null)
			{
				await Context.Channel.SendMessageAsync("The Shop is empty");
				return;
			}
			foreach (var item in ShopItems)
			{
				embed.AddField(item.itemName, item.price.ToString() + " "+ Global.Currency, true);
			}
			embed.WithColor(new Color(0, 255, 0));
			await Context.Channel.SendMessageAsync("", false, embed);
		}

		[Command("remove")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task RemoveItem(string nameAndRole)
		{
			var shopList = DataStorage.ListItems("Resources/ShopItems.json");
			foreach (var item in shopList)
			{
				if (nameAndRole.Trim() == item.itemName)
				{
					ShopItems.RemoveItem(item);
					await Context.Channel.SendMessageAsync($"{item.itemName} was removed");
				}
			}
		}

		[Command("Buy")]
		public async Task Buy([Remainder]string Item)
		{
			var shopList = DataStorage.ListItems("Resources/ShopItems.json");
			int counter = shopList.Count();
			foreach (var itemm in shopList)
			{
				if (Item.Trim() != itemm.itemName)
				{
					counter--;
				}
				if (Item.Trim() == itemm.itemName)
				{
					//await Context.Channel.SendMessageAsync($"We found it, you're trying to buy {Item}");
					var account = UserAccounts.GetAccount(Context.User as IGuildUser);
					if (account.Money >= itemm.price)
					{
						if (!account.boughtItems.Any(i => i.itemName == Item))
						{
							await BuyItem(itemm, Context.User as IGuildUser);
							account.Money -= itemm.price;
							account.boughtItems.Add(itemm);
							UserAccounts.SaveAccount();
						}
						else
						{
							await Context.Channel.SendMessageAsync($"Sorry, you already bought {Item}");
						}

					}
					else
					{
						await Context.Channel.SendMessageAsync($"you don't have enough {Global.Currency} to get it ,GO BE ACTIVE!");
					}
					break;
				}
				if (counter == 0)
				{
					await Context.Channel.SendMessageAsync($"{Item} was not found, Please copy the name exactly");
				}
			}
		}

		public async Task BuyItem(ShopItem item, IGuildUser user)
		{
			if (item.rolename == "")
			{
				await Context.Channel.SendMessageAsync($"Congrats! you bought {item.itemName}");
				UserAccounts.SaveAccount();
			}
			else
			{
				if (Global.StaffChannel != null)
				{
					if (item.rolename == "Emote" || item.rolename == "emote")
					{
						await Global.StaffChannel.SendMessageAsync($"{user.Username} Just bought a custom emote, go assign it for them");
						await Context.Channel.SendMessageAsync($"Congrats! you bought a custom emote,admins will get notified ");
						return;
					}
					else
					if (item.rolename == "Custom Emote" || item.rolename == "custom emote")
					{
						await Global.StaffChannel.SendMessageAsync($"{user.Username} Just bought a custom role,go assign it for them");
						await Context.Channel.SendMessageAsync($"Congrats! you bought a custom role,admins will get notified");
					}
					var role = user.Guild.Roles.FirstOrDefault(x => x.Name == item.rolename.Trim());
					await user.AddRoleAsync(role);
					await Context.Channel.SendMessageAsync($"Congrats! you bought {item.itemName} and you now have the role");
					UserAccounts.SaveAccount();
				}
			}
		}
		public async Task<string> SanitizeDB(string str)
		{
			if (str[0] == '/')
			{
				str = str.Substring(1);
			}
			return str;
		}
	}
}
