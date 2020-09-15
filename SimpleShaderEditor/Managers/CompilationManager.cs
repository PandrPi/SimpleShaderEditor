using SimpleShaderEditor.Managers.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleShaderEditor.Managers
{
	/// <summary>
	/// Implements a compiler interaction logic
	/// </summary>
	class CompilationManager
	{
		public class PixelShaderVersion
		{
			public string Value { get; set; }
			public PixelShaderVersion(string value)
			{
				this.Value = value;
			}

			public static readonly PixelShaderVersion PS_11 = new PixelShaderVersion("ps_1_1");
			public static readonly PixelShaderVersion PS_20 = new PixelShaderVersion("ps_2_0");
			public static readonly PixelShaderVersion PS_30 = new PixelShaderVersion("ps_3_0");
		}

		public static readonly string BuildSucceededMessage = "Build succeeded";
		public static readonly string BuildFailedMessage = "Build failed";
		private static readonly string CompilerFilePath = Path.Combine(ConfigManager.AppPath, "Resources/fxc.exe");
		/// <summary>
		/// Contains the format string from which the compiler argument string will be formed.
		/// {0}: pixel shader version
		/// {1}: path to compiled file
		/// {2}: path to source file
		/// </summary>
		private static readonly string CompilerArgumentsFormat = "/T {0} /E main /Fo {1} {2}";
		private static PixelShaderVersion PSVersion = PixelShaderVersion.PS_30;

		/// <summary>
		/// Runs the compiler process with specified arguments and returns the result of compilation asynchronously.
		/// </summary>
		/// <param name="tabData">TabInfo object associated with the selected tab</param>
		/// <returns>ExitCode of compiler process</returns>
		public static async Task<int> CompileShaderAsync(TabInfo tabData)
		{
			var compilerProcess = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					FileName = CompilerFilePath,
					Arguments = string.Format(
						CompilerArgumentsFormat, PSVersion.Value, tabData.CompiledFilePath, tabData.LinkedFilePath),
					UseShellExecute = false,
					RedirectStandardError = true,
					CreateNoWindow = true
				}
			};
			StringBuilder outputBuilder = new StringBuilder();
			compilerProcess.ErrorDataReceived += (sender, e) => outputBuilder.AppendLine(e.Data?.ToString());

			await Task.Run(() =>
			{
				compilerProcess.Start();
				compilerProcess.BeginErrorReadLine();
				compilerProcess.WaitForExit();
			});

			if (compilerProcess.ExitCode != 0)
			{
				string errorWindowContent = outputBuilder.ToString();
				MessageBox.Show(errorWindowContent, BuildFailedMessage, MessageBoxButton.OK, MessageBoxImage.Error);
			}

			return compilerProcess.ExitCode;
		}
	}
}
