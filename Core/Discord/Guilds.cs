using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Linq;
using System.IO;

namespace Discord_Bot.Core.Discord
{
     internal class Guilds
{
	internal static void InitializeGuilds()
	{
		foreach (var guild in Global.Client.Guilds)
		{
			System.Console.WriteLine(guild.Name + " with " + guild.Users.Count + " users.");
			GetGuildConfig(guild.Id);
		}
	}

	internal static GuildConfig GetGuildConfig(ulong guildID)
	{
		var result = from g in Config.guildConfigs
					 where g.ID == guildID
					 select g;

		GuildConfig guild = result.FirstOrDefault();

		if (guild == null)
		{
			var newGuild = new GuildConfig
			{
				ID = guildID,
				Prefix = Constants.defaultPrefix,
				TextMessageExpReward = Constants.defaultTextMsgReward,
				FileSentExpReward = Constants.defaultFileSentReward,
				ExpSecondsDelay = Constants.defaultExpSecondsDelay
			};

			Config.guildConfigs.Add(newGuild);
			SaveGuildConfigs();
			return GetGuildConfig(guildID);
		}

		return guild;
	}

	internal static GuildConfig GetGuildConfig(SocketGuild guild)
	{
		return GetGuildConfig(guild.Id);
	}

	internal static void SaveGuildConfigs()
	{
		string json = JsonConvert.SerializeObject(Config.guildConfigs, Formatting.Indented);
		File.WriteAllText(Constants.guildsConfigFile, json);
	}

	internal static SocketGuildUser GetGuildUserFromAnyGuild(ulong id)
	{
		foreach (SocketGuild guild in Global.Client.Guilds)
		{
			SocketGuildUser user = guild.GetUser(id);
			if (user != null) return user;
		}
		return null;
	}

	internal static void SetGuildPrefix(ulong guildID, string prefix)
	{
		prefix = prefix.Trim();
		GuildConfig cfg = GetGuildConfig(guildID);
		cfg.Prefix = prefix;
		SaveGuildConfigs();
	}

	internal static void SetGuildPrefix(SocketGuild guild, string prefix)
	{
		SetGuildPrefix(guild.Id, prefix);
	}
}
}
