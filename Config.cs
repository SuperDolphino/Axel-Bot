using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Discord_Bot.Core.Discord;

namespace Discord_Bot
{
	class Config
    {
		public const string TokenPath = "Resources/token.json";
		public const string ConfigPath = "Resources/config.json";
		public const string TokenEnvVar = "TOKEN";
		internal static BotConfig bot;
		public static BotConfig BotConfig;
		internal static List<GuildConfig> guildConfigs;
		public static string Token;

		static Config()
		{

			bot = AxelFiles.GetParsedJSONFromFileOrCreate<BotConfig>(Constants.botConfigFile);
			guildConfigs = AxelFiles.GetParsedJSONFromFileOrCreate<List<GuildConfig>>(Constants.guildsConfigFile);
			ValidateConfigFileStructure();
			BotConfig = new BotConfig ();

			if (File.Exists (TokenPath))
				Token = JsonConvert.DeserializeObject<string> (File.ReadAllText (TokenPath));
			else
				Token = Environment.GetEnvironmentVariable (TokenEnvVar);

			BotConfig = JsonConvert.DeserializeObject<BotConfig> (File.ReadAllText (ConfigPath));
		}

		private static void ValidateConfigFileStructure()
		{
			if (bot == null)
			{
				bot = new BotConfig { token = "NDY3Nzk2MDYzNzUxNzY2MDI3.Di3N5w.URwnpv7oPbBIQLuK39CwtfZv6Xg" };
				SaveBotConfig();
			}

			if (guildConfigs == null)
			{
				guildConfigs = new List<GuildConfig>();
				Guilds.SaveGuildConfigs();
			}
		}

		internal static void SaveBotConfig()
		{
			string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
			File.WriteAllText(Constants.botConfigFile, json);
		}
	}



	public class BotConfig
	{
		public string token;
		public string cmdPrefix;
	}



}
	
public class BotConfig
{
	public string cmdPrefix;
}
