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


namespace Discord_Bot.Modules
{
	public class Misc : ModuleBase<SocketCommandContext>
	{
		[Command("echo")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task Echo(ITextChannel channel, [Remainder] string message)
		{

			var embed = new EmbedBuilder();
			embed.WithTitle("");
			embed.WithDescription(message);
			embed.WithColor(new Color(0, 255, 0));
			await channel.SendMessageAsync("", false, embed);
		}

		[Command("Currency")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task SetCurrency(string Currency)
		{
			Global.Currency = Currency;
			await Context.Channel.SendMessageAsync($"Currency was set to {Currency}!");
		}

		[Command("Dir")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task dir()
		{
			string dir = "/app/heroku_output/Resources";
			try
			{
				//Set the current directory.
				Directory.SetCurrentDirectory(dir);
			}
			catch (DirectoryNotFoundException e)
			{
				Console.WriteLine("The specified directory does not exist. {0}", e);
			}
			// Print to console the results.
			Console.WriteLine("Root directory: {0}", Directory.GetDirectoryRoot(dir));
			Console.WriteLine("Current directory: {0}", Directory.GetCurrentDirectory());
		}

		[Command("React")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task HandleReactionMessage(ITextChannel channel, [Remainder] string message)
		{
			string[] options = message.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
			//await channel.SendMessageAsync("The message is: " + options[0] + "The emote is: " + options[1] + "The role name is: " + options[2]);
			IEmote emote1 = ParseEmotji(options[1].Trim());
			IUserMessage msg = await channel.SendMessageAsync(options[0].Trim());
			var emote = ParseEmotji(options[1].Trim());
			Global.Emotee = emote;
			Global.MessageIdToTrack = msg.Id;
			Global.Emote = options[1].Trim();
			Global.Role = options[2].Trim();
			await msg.AddReactionAsync(emote);
		}

		private IEmote ParseEmotji(string text)
		{
			var success = Emote.TryParse(text, out var emote);
			var emoji = new Emoji(text);
			if (success) return emote;
			Global.Emotee = emoji;
			return emoji;
		}

		[Command("info")]
		public async Task GiveInfo([Remainder]string command = "")
		{
			if (command == null || command == "")
			{
				if ((Context.User as SocketGuildUser).GuildPermissions.Has(GuildPermission.Administrator))
				{
					var embed = new EmbedBuilder();
					embed.WithTitle("Commands list:");
					embed.WithDescription("");
					embed.AddField("React:", "Create a new message that members can react to to get roles! **(MOD ONLY)**", false);
					embed.AddField("Echo:", "Send a message from the bot to a specific channel (announcement)!  **(MOD ONLY)**", false);
					embed.AddField("Additem:", "Add an item to the shop **(MOD ONLY)**", false);
					embed.AddField("Axolotl:", "Get a random axolotl picture!", false);
					embed.AddField("Fight:", "Get into a fight and kick some ass!", false);
					embed.AddField("Stats:", "Display how much Money you have", false);
					embed.AddField("Flip:", "flip a coin!", false);
					embed.AddField("Shop:", "Open the Shop", false);
					embed.AddField("Buy:", "Buy something from the Shop", false);
					embed.WithColor(new Color(0, 255, 0));
					await Context.Channel.SendMessageAsync("", false, embed);
				}
				else
				{
					var embed = new EmbedBuilder();
					embed.WithTitle("Commands list:");
					embed.WithDescription("");
					embed.AddField("Axolotl:", "Get a random axolotl picture!", false);
					embed.AddField("Stats:", "See how much Whole you have!", false);
					embed.AddField("Fight:", "Get into a fight and kick some ass!", false);
					embed.AddField("Flip:", "flip a coin!", false);
					embed.AddField("Shop:", "Open the Shop", false);
					embed.AddField("Buy:", "Buy something from the Shop", false);
					embed.WithColor(new Color(0, 255, 0));
					await Context.Channel.SendMessageAsync("", false, embed);
				}

			}
			else
			{
				if (command == "react" || command == "React")
				{
					var embed = new EmbedBuilder();
					embed.WithTitle("How to use React:");
					embed.WithDescription("$react {#channel} {message}|{emote}|{role given on reaction}");
					embed.WithColor(new Color(0, 255, 0));
					await Context.Channel.SendMessageAsync("", false, embed);
				}
				if (command == "echo" || command == "Echo")
				{
					var embed = new EmbedBuilder();
					embed.WithTitle("How to use Echo:");
					embed.WithDescription("echo {#channel} {message}");
					embed.WithColor(new Color(0, 255, 0));
					await Context.Channel.SendMessageAsync("", false, embed);
				}
				if (command == "buy" || command == "Buy")
				{
					var embed = new EmbedBuilder();
					embed.WithTitle("How to use Buy:");
					embed.WithDescription("buy {Item name (Caps sensitive)}");
					embed.WithColor(new Color(0, 255, 0));
					await Context.Channel.SendMessageAsync("", false, embed);
				}
				if (command == "Additem" || command == "additem")
				{
					var embed = new EmbedBuilder();
					embed.WithTitle("How to use additem:");
					embed.WithDescription("additem {Price} {Item name (Caps sensitive)} | {Role given when bought (if any) -- use [emote] instead if the item is an emote}");
					embed.WithColor(new Color(0, 255, 0));
					await Context.Channel.SendMessageAsync("", false, embed);
				}

			}
		}

		[Command("Axolotl")]
		public async Task Axolotl()
		{
			const int numberOfImages = 60;
			var rand = new Random();
			int imageNumber = rand.Next(1, numberOfImages);
			string imageName = "Resources/Axolotls" + "/" + "Axolotl (" + imageNumber.ToString() + ").jpg";
			await Context.Channel.SendFileAsync(imageName);
		}

		[Command("flip")]
		public async Task Flip()
		{
			Random r = new Random();
			int result = r.Next(1, 3);

			switch (result)
			{
				case 1:
					await Context.Channel.SendMessageAsync($"You got heads!");
					break;
				case 2:
					await Context.Channel.SendMessageAsync($"You got tails!");
					break;
			}
		}

		[Command("Fight")]
		public async Task Fight(IGuildUser fight)
		{

			await Context.Channel.SendMessageAsync($"Oh Boi!!, it looks like {Context.User.Mention} and {fight.Mention} are fighting!!");
			Console.WriteLine("FIGHITING AND SHIT");
			var seconds = 3000;
			Random r = new Random();
			int result = r.Next(1, 3);

			await Task.Delay(seconds);
			switch (result)
			{
				case 1:
					await Context.Channel.SendMessageAsync($"{fight.Mention} WON!");
					break;
				case 2:
					await Context.Channel.SendMessageAsync($"{Context.User.Mention} WON!");
					break;
			}
		}

		[Command("stats")]
		public async Task MyXP()
		{
			var account = UserAccounts.GetAccount(Context.User as IGuildUser);
			await Context.Channel.SendMessageAsync($"{Context.User.Username} You have {account.Whole} {Global.Currency}.");
		}

		[Command("AddMoney")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task AddXP(IGuildUser user, uint amount)
		{
			var account = UserAccounts.GetAccount(Context.User as IGuildUser);
			account.Whole += amount;
			await Context.Channel.SendMessageAsync($"{user.Username} now has {account.Whole.ToString()} whole!");
		}

		[Command("staffChannel")]
		[RequireUserPermission(GuildPermission.Administrator)]
		public async Task setChannel(ITextChannel channel)
		{
			Global.StaffChannel = channel;
			await Context.Channel.SendMessageAsync($"The Staff channel was set to {channel}");
		}
		[Command("Shop")]
		public async Task Shop()
		{
			var embed = new EmbedBuilder();
			embed.WithTitle("Shop:");
			var shopList = DataStorage.ListItems("Resources/ShopItems.json");
			foreach (var item in shopList)
			{
				embed.AddField(item.itemName, item.price.ToString() + Global.Currency, true);
			}
			embed.WithColor(new Color(0, 255, 0));
			await Context.Channel.SendMessageAsync("", false, embed);
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
					if (account.Whole >= itemm.price)
					{
						if (!account.boughtItems.Any(i => i.itemName == Item))
						{
							await BuyItem(itemm, Context.User as IGuildUser);
							account.Whole -= itemm.price;
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
					await Context.Channel.SendMessageAsync($"{Item} was not found, rip");
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
	}
}