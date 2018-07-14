using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Discord_Bot
{
	class Config
    {
		public const string TokenPath = "Resources/token.json";
		public const string ConfigPath = "Resources/config.json";
		public const string TokenEnvVar = "TOKEN";

		public static BotConfig BotConfig;
		public static string Token;

		static Config()
		{
			BotConfig = new BotConfig ();

			if (File.Exists (TokenPath))
				Token = JsonConvert.DeserializeObject<string> (File.ReadAllText (TokenPath));
			else
				Token = Environment.GetEnvironmentVariable (TokenEnvVar);

			BotConfig = JsonConvert.DeserializeObject<BotConfig> (File.ReadAllText (ConfigPath));
		}
	}
}
	
public class BotConfig
{
	public string cmdPrefix;
}
