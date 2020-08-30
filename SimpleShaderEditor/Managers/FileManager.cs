using Microsoft.Win32;
using System.IO;

namespace SimpleShaderEditor.Managers
{
	/// <summary>
	/// Implements file loading and save logic
	/// </summary>
	class FileManager
	{
		private static readonly string DialogFilter = "HLSL files (*.hlsl)|*.hlsl|Text files (*.txt)|*.txt|All files (*.*)|*.*";

		/// <summary>
		/// Shows a OpenFileDialog for the user to select the file to open
		/// </summary>
		/// <returns>Selected file path or ConfigManager.DialogCanceledMessage if the selection dialog was canceled by user</returns>
		public static string GetSelectedFilePathFromOpenFileDialog()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = DialogFilter;

			return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : ConfigManager.DialogCanceledMessage;
		}

		/// <summary>
		/// Shows a SaveFileDialog for the user to select the file to save
		/// </summary>
		/// <param name="textToSave">Text to write to the selected file</param>
		public static void SaveFile(string textToSave)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = DialogFilter;

			if (saveFileDialog.ShowDialog() == true)
			{
				File.WriteAllText(saveFileDialog.FileName, textToSave);
			}
		}
	}
}
