using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using Discord;

namespace Discord_Bot
{
	class Program : ModuleBase<SocketCommandContext>
	{
		DiscordSocketClient _client;
		CommandHandler _handler;

		

		static void Main(string[] args)
		=> new Program().StartAsync().GetAwaiter().GetResult();

		public async Task StartAsync()
		{
			if (Config.bot.token == "" || Config.bot.token == null)
			{
				Console.WriteLine("no config found");
				return;
			}
			_client = new DiscordSocketClient(new DiscordSocketConfig
			{
				LogLevel = LogSeverity.Verbose
			});
			_client.Log += Log;
			_client.ReactionAdded += OnReactionAdded;
			await _client.LoginAsync(TokenType.Bot, Config.bot.token);
			await _client.StartAsync();
			_handler = new CommandHandler();
			await _handler.InitializeAsync(_client);
			await Task.Delay(-1);
		}

		private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cashe, ISocketMessageChannel channel, SocketReaction reaction)
		{

			if (reaction.MessageId == Global.MessageIdToTrack)
			{
				if (reaction.Emote.Name == Global.Emotee.Name)
				{
					var guildUser = reaction.User.Value as IGuildUser;
					if (!guildUser.IsBot)
					{
						//await channel.SendMessageAsync(reaction.User.Value.Username + " Reacted");
						var role = guildUser.Guild.Roles.FirstOrDefault(x => x.Name == Global.Role);
						await guildUser.AddRoleAsync(role);
						//await channel.SendMessageAsync(guildUser.Username + "  you are given the " + Global.Role + " Congrats");
					}
				}
			}
		}

		private async Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.Message);

		}
	}
}
