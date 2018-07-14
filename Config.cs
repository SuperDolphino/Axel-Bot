﻿using System;
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
			private static string configFolder = $"{Environment.CurrentDirectory}/Resources";
			private const string configFile = "config.json";

			public static BotConfig bot;

			static Config()
			{
				if (!Directory.Exists(configFolder))
					Directory.CreateDirectory(configFolder);

				if (!File.Exists(configFolder + "/" + configFile))
				{
					bot = new BotConfig();
					string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
					File.WriteAllText(configFolder + "/" + configFile, json);

				}
				else
				{
					string json = File.ReadAllText(configFolder + "/" + configFile);

					bot = JsonConvert.DeserializeObject<BotConfig>(json);
				}

				if (bot.token == "" && bot.cmdPrefix == "")
				{
					bot = new BotConfig
					{
						token = Environment.GetEnvironmentVariable("TOKEN"),
						cmdPrefix = Environment.GetEnvironmentVariable("PREFIX")
					};
				}
			}
		}
	}
	
	public class BotConfig
	{
		public string token;
		public string cmdPrefix;
	}


