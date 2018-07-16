using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Discord_Bot.Core.Discord
{
     internal class AxelFiles
{
	static AxelFiles()
	{
		CreateDirectoryIfNotFound(Constants.configRoot);
	}

	private static void CreateDirectoryIfNotFound(string directory)
	{
		if (!Directory.Exists(Constants.configRoot)) Directory.CreateDirectory(Constants.configRoot);
	}

	internal static string GetFileContents(string file)
	{
		if (!File.Exists(file)) throw new Exception($"File doesn't exist: {file}.");
		return File.ReadAllText(file);
	}

	internal static string GetFileContentsOrCreate(string file)
	{
		if (!File.Exists(file))
		{
			File.WriteAllText(file, "");
			return "";
		}
		return File.ReadAllText(file);
	}

	internal static T GetParsedJSONFromFile<T>(string file)
	{
		string json = GetFileContents(file);
		return JsonConvert.DeserializeObject<T>(json);
	}

	/// <summary>
	/// Gets a JSON object from file's contents
	/// if the file doesn't exist, it creates it.
	/// </summary>
	/// <typeparam name="T">Type to cast JSON to</typeparam>
	/// <param name="file">File's path</param>
	/// <returns>JSON casted into a typeparam.</returns>
	internal static T GetParsedJSONFromFileOrCreate<T>(string file)
	{
		string json = GetFileContentsOrCreate(file);
		return JsonConvert.DeserializeObject<T>(json);
	}
}
}
