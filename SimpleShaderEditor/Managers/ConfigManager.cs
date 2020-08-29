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
		public static readonly string AppPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
		public static readonly string ConfigFileName  = "config.json";
		public static readonly string DialogCanceledMessage = "Dialog box was canceled by user";
		public static readonly string DefaultShaderCode = "float4 main(float2 uv:TEXCOORD) : COLOR\n{\n\treturn float4(1.0f, 1.0f, 1.0f, 1.0f);\n}";
		public static readonly string EditorTabDefaultHeader = "New tab";

		/// <summary>
		/// A KeyValue pair to store the config fields
		/// </summary>
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
			else
			{
				// TODO: Load config from file 
			}
		}
	}
}
