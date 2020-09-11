using System.Diagnostics;
using System.IO;

namespace SimpleShaderEditor.Managers
{
	/// <summary>
	/// Implements a compiler interaction logic
	/// </summary>
	class CompilationManager
	{
		private static readonly string CompilerFilePath = Path.Combine(ConfigManager.AppPath, "fxc.exe");
		public static int CompileShader(string shaderFilePath)
		{
			Process compilerProcess = Process.Start(CompilerFilePath);

			return 0;
		}
	}
}
