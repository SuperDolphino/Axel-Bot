using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discord_Bot
{
	internal static class Global
	{

		internal static DiscordSocketClient Client { get; set; }

		internal static ulong MessageIdToTrack { get; set; }
		internal static string Emote { get; set; }
		internal static string Role { get; set; }
		internal static IEmote Emotee { get; set; }
		internal static string Currency { get; set; } = "Whole";
		internal static ITextChannel StaffChannel { get; set;}
	}
}
