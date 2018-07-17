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
		public async Task Dir()
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
			string files = string.Join(", ", Directory.EnumerateFiles(dir));
			Console.WriteLine(files);
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
					embed.AddField("AddMoney:", "Give a user an amount of money **(MOD ONLY)**", false);
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

		[Command("Vote")]
		[Summary("Creates a vote.")]
		public async Task NewVote([Remainder] string secondPart)
		{
			var configCfg = Guilds.GetGuildConfig(Context.Guild);
			if (secondPart.Length >= 200)
			{
				await Context.Channel.SendMessageAsync("Vote is too long");
				return;
			}
			var embed = new EmbedBuilder();
			embed.WithColor(Color.Green);
			embed.WithTitle("VOTE");
			embed.WithDescription(secondPart);
			RestUserMessage msg = await Context.Channel.SendMessageAsync("", embed: embed);
			await msg.AddReactionAsync(new Emoji("✅"));
			await msg.AddReactionAsync(new Emoji("❌"));
		}

		[Command("Axolotl")]
		public async Task Axolotl()
		{
			#region link
			var links = "https://i.imgur.com/9IlApTP.jpg , https://i.imgur.com/09r84At.jpg , https://i.imgur.com/dmiAkWx.jpg , https://i.imgur.com/IKt8A2J.jpg , https://i.imgur.com/EZflXPu.jpg , https://i.imgur.com/r3zSlJr.jpg , https://i.imgur.com/hyKgWQ2.jpg , https://i.imgur.com/x0jBNJ5.jpg , https://i.imgur.com/kymTLW5.jpg , https://i.imgur.com/Gii8onS.jpg , https://i.imgur.com/4PWxyQY.jpg , https://i.imgur.com/B3D8pPX.jpg , https://i.imgur.com/HWjbK4U.jpg , https://i.imgur.com/VVGBC72.jpg , https://i.imgur.com/RAJkfNU.jpg , https://i.imgur.com/qRFC18J.jpg , https://i.imgur.com/I1tHadO.jpg , https://i.imgur.com/oBjvSdt.jpg , https://i.imgur.com/mbCv0Ts.jpg , https://i.imgur.com/6Z2SjCq.jpg , https://i.imgur.com/MFedbn8.jpg , https://i.imgur.com/Aylfw7q.jpg , https://i.imgur.com/nNkFNli.jpg , https://i.imgur.com/Ke20FIo.jpg , https://i.imgur.com/TE6J0WS.jpg , https://i.imgur.com/TVqNZJb.jpg , https://i.imgur.com/wPClP6g.jpg , https://i.imgur.com/F1U9JVS.jpg , https://i.imgur.com/pWk663L.jpg , https://i.imgur.com/BIHlnGz.jpg , https://i.imgur.com/RePVsoe.jpg , https://i.imgur.com/pdEAtLX.jpg , https://i.imgur.com/bJmZPBx.jpg , https://i.imgur.com/rUoOAlx.jpg , https://i.imgur.com/5NVK05l.jpg , https://i.imgur.com/C3ZpIwo.jpg , https://i.imgur.com/VkgI8Lx.jpg , https://i.imgur.com/5BVKSmi.jpg , https://i.imgur.com/3csNbn2.jpg , https://i.imgur.com/IbHQaXj.jpg , https://i.imgur.com/B8eeFAj.jpg , https://i.imgur.com/jLVU6TD.jpg , https://i.imgur.com/qbJ5W29.jpg , https://i.imgur.com/nukWklR.jpg , https://i.imgur.com/NN0zzoZ.jpg , https://i.imgur.com/qXFf3OO.jpg , https://i.imgur.com/V93ZL87.jpg , https://i.imgur.com/INRks1s.jpg , https://i.imgur.com/aLVyUcQ.jpg , https://i.imgur.com/sFK6Mjx.jpg , https://i.imgur.com/wRflVSq.jpg , https://i.imgur.com/Pmpq3UC.jpg , https://i.imgur.com/VCbfJ7L.jpg , https://i.imgur.com/kwYujry.jpg , https://i.imgur.com/y44cB76.jpg , https://i.imgur.com/s7R731Z.jpg , https://i.imgur.com/m7o9l4K.jpg , https://i.imgur.com/J8fJfqu.jpg , https://i.imgur.com/zHKOlZl.jpg , https://i.imgur.com/abxNFIy.jpg , https://i.imgur.com/ljDR1BJ.jpg , https://i.imgur.com/VTVTYF1.jpg , https://i.imgur.com/j1Vyt9p.jpg , https://i.imgur.com/gwffjlm.jpg";
			#endregion

			List<string> list = links.Split(',').ToList<string>();

			var rng = new Random();
			var randomIndex = rng.Next(0, list.Count());

			var embed = new EmbedBuilder();
			embed.WithImageUrl(list[randomIndex].Trim());
									 //const int numberOfImages = 60;
									 //var rand = new Random();
									 //int imageNumber = rand.Next(1, numberOfImages);
									 //string imageName = "Resources/Axolotls" + "/" + "Axolotl (" + imageNumber.ToString() + ").jpg";
									 //await Context.Channel.SendFileAsync(imageName);
			await Context.Channel.SendMessageAsync("", false, embed);
		}

		[Command("AddAxolotl")]
		public async Task AddAxolotl()
		{
			
		}
		[Command("flip")]
		public async Task Flip()
		{
			Random r = new Random();
			int result = r.Next(1, 3);

			switch (result)
			{
				case 1:
					var embed = new EmbedBuilder();
					embed.WithTitle("Coin Flip Result:");
					embed.WithDescription($"**{Context.User.Username}** You tossed the coin and got **HEADS**!");
					embed.WithColor(new Color(0, 255, 0));
					await Context.Channel.SendMessageAsync("", false, embed);

					break;
				case 2:
					var embed2 = new EmbedBuilder();
					embed2.WithTitle("Coin Flip Result:");
					embed2.WithDescription($"**{Context.User.Username}** You tossed the coin and got **TAILS**!");
					embed2.WithColor(new Color(0, 255, 0));
					await Context.Channel.SendMessageAsync("", false, embed2);
					break;
			}
		}

		[Command("Fight")]
		public async Task Fight(IGuildUser fight)
		{

			await Context.Channel.SendMessageAsync($"Oh Boi!!, it looks like {Context.User.Mention} and {fight.Mention} are fighting!!");
			var seconds = 3000;
			Random r = new Random();
			int result = r.Next(1, 3);

			await Task.Delay(seconds);
			switch (result)
			{
				case 1:
					await Context.Channel.SendMessageAsync($"***{fight.Mention} WON!***");
					break;
				case 2:
					await Context.Channel.SendMessageAsync($"***{Context.User.Mention} WON!***");
					break;
			}
		}

	}
}