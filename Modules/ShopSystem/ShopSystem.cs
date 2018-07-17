using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Discord_Bot.Core.UserAccounts;
using Discord_Bot.Core;
using System.IO;
using Discord_Bot.Core.Discord;
using System.Web;
using Npgsql;
using System.Runtime.InteropServices;



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
			Global.StaffChannel = channel;
			await Context.Channel.SendMessageAsync($"The Staff channel was set to {channel}");
		}

		[Command("Additem")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task ShopAdd(uint price, [Remainder]string nameAndRole)
		{
			string[] options = nameAndRole.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

			ShopItem itemToAdd = new ShopItem();
			itemToAdd.itemName = options[0].Trim();
			itemToAdd.price = price;
			if (options.Length == 2)
			{
				itemToAdd.rolename = options[1].Trim();
			}
			var elitem = ShopItems.GetItem(itemToAdd);
			if (itemToAdd.rolename == "")
			{
				await Context.Channel.SendMessageAsync($"You added {elitem.itemName} to the shop, it costs {elitem.price}");
			}
			else
			{
				if (Global.StaffChannel == null)
				{
					await Context.Channel.SendMessageAsync("Please set the Staff Channel using a/staffChannel");
					return;
				}
				else
				{
					if (itemToAdd.rolename == "Emote" || itemToAdd.rolename == "emote")
					{
						await Context.Channel.SendMessageAsync($"You added {elitem.itemName} to the shop, it costs {elitem.price} , Admins will get notified in {Global.StaffChannel}");
					}
					else if (itemToAdd.rolename == "Custom Emote" || itemToAdd.rolename == "custom emote")
					{
						await Context.Channel.SendMessageAsync($"You added {elitem.itemName} to the shop, it costs {elitem.price} ,Admins will get notified in {Global.StaffChannel}");
					}
					else
					{
						await Context.Channel.SendMessageAsync($"You added {elitem.itemName} to the shop, it costs {elitem.price} , you get the {itemToAdd.rolename} role for buying it");
					}
				}
			}
		}


		[Command("Shop")]
		public async Task Shop()
		{

			var embed = new EmbedBuilder();
			embed.WithTitle("Shop:");
			var shopList = DataStorage.ListItems("Resources/ShopItems.json");
			if (shopList.Count() == 0 || shopList == null)
			{
				await Context.Channel.SendMessageAsync("The Shop is empty");
				return;
			}
			foreach (var item in shopList)
			{
				embed.AddField(item.itemName, item.price.ToString() + Global.Currency, true);
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
