using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Bot
{
    class Constants
    {
	internal static readonly string configRoot = "Config";
	internal static readonly string botConfigFile = configRoot + "/botConfig.json";
	internal static readonly string guildsConfigFile = configRoot + "/guildsConfig.json";
	internal static readonly string playersConfigFile = configRoot + "/playersConfig.json";
	internal static readonly string defaultPrefix = "-";
	internal static readonly ulong defaultTextMsgReward = 10;
	internal static readonly ulong defaultFileSentReward = 15;
	internal static readonly uint defaultExpSecondsDelay = 120;
	}
}
