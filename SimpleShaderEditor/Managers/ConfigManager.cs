using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace SimpleShaderEditor.Managers
{
	/// <summary>
	/// This class is used to store the settings and give the access to them from the other classes
	/// </summary>
	class ConfigManager
	{
		public static string AppPath { get; set; } = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
		public static string ConfigFileName { get; set; } = "config.json";


		private Dictionary<string, dynamic> configFieldsDictionary = new Dictionary<string, dynamic>();

		public ConfigManager()
		{
			InitializeConfigFile();
		}

		private void InitializeConfigFile()
		{
			string configFileFullPath = Path.Combine(AppPath, ConfigFileName);

			if (File.Exists(configFileFullPath) == false)
			{
				File.WriteAllText(configFileFullPath, JsonConvert.SerializeObject(configFieldsDictionary, Formatting.Indented));
			}
		}
	}
}
