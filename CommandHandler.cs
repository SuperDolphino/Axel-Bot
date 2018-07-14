using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using Discord_Bot.Core.Leveling;
using System.Diagnostics;
using Discord_Bot.Core.UserAccounts;
using Discord;

namespace Discord_Bot
{
	class CommandHandler
	{
		DiscordSocketClient _client;
		CommandService _service;

		public async Task InitializeAsync(DiscordSocketClient client)
		{
			_client = client;
			_service = new CommandService();
			await _service.AddModulesAsync(Assembly.GetEntryAssembly());
			_client.MessageReceived += HandleCommandAsync;

		}

		private async Task HandleCommandAsync(SocketMessage s)
		{
			var msg = s as SocketUserMessage;
			if (msg == null) return;
			var context = new SocketCommandContext(_client, msg);
			if (context.User.IsBot) return;


			// Leveling up
			Leveling.UserSentMessage((SocketGuildUser)context.User);

			int argPos = 0;
			if (msg.HasStringPrefix(Config.BotConfig.cmdPrefix, ref argPos)
				|| msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
			{
				var result = await _service.ExecuteAsync(context, argPos);
				if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
				{
					Console.WriteLine(result.ErrorReason);
				}
			}
		}
	}
}
