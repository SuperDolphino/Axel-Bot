using System;
using System.Collections.Generic;
using System.Text;

namespace Discord_Bot.Core.Discord
{
    public class GuildConfig
{
	public ulong ID { get; set; }

	public string Prefix { get; set; }

	public int LanguageID { get; set; }

	// Leveling

	public ulong TextMessageExpReward { get; set; }

	public ulong FileSentExpReward { get; set; }

	public uint ExpSecondsDelay { get; set; }
}
}
