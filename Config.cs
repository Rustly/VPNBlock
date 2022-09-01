using Newtonsoft.Json;
using System;
using System.IO;

namespace VPNBlock
{
	public class Config
	{
		private static readonly string _path = Path.Combine(TShockAPI.TShock.SavePath, "vpnblock.json");

		public static Config Read()
		{
			try
			{
				var res = new Config();
				if (File.Exists(_path))
					res = JsonConvert.DeserializeObject<Config>(File.ReadAllText(_path));
				File.WriteAllText(_path, JsonConvert.SerializeObject(res, Formatting.Indented));
				return res;
			}
			catch (Exception ex)
			{
				TShockAPI.TShock.Log.Error(ex.ToString());
			}

			return null;
		}

		public bool Enabled { get; set; } = true;

		public int MaxConnectionsPerIp { get; set; } = 2;

		public string ContactEmail { get; set; }
	}
}
