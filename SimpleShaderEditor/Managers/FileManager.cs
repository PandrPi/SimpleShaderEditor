using Microsoft.Win32;
using SimpleShaderEditor.Managers.Data;
using System.IO;

namespace SimpleShaderEditor.Managers
{
	/// <summary>
	/// Implements file loading and save logic
	/// </summary>
	class FileManager
	{
		private static readonly string CompilationDialogFilter = "PixelShader files (*.ps)|*.ps|All files (*.*)|*.*";
		private static readonly string DialogFilter = "HLSL files (*.hlsl)|*.hlsl|Text files (*.txt)|*.txt|All files (*.*)|*.*";
		private static readonly string DialogTitle = "Select the file to process";

		/// <summary>
		/// Shows the OpenFileDialog, where the user can select the file from which the content will be read.
		/// </summary>
		/// <returns>Selected file path or ConfigManager.DialogCanceledMessage if the selection dialog was canceled by user</returns>
		public static string GetSelectedPathOfFileToOpen()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = DialogFilter;

			return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : ConfigManager.DialogCanceledMessage;
		}

		public static string GetSelectedPathOfCompiledFile()
		{
			SaveFileDialog fileDialog = new SaveFileDialog();
			fileDialog.Filter = CompilationDialogFilter;

			return fileDialog.ShowDialog() == true ? fileDialog.FileName : ConfigManager.DialogCanceledMessage;
		}

		/// <summary>
		/// Saves data from a TabInfo instance to associated file.
		/// </summary>
		/// <param name="dataContext">TabInfo instance from which we get data we need to save</param>
		/// <param name="showSaveDialog">Determines whether we need to show the SaveFileDialog for selecting a file path</param>
		public static void SaveFile(TabInfo dataContext, bool showSaveDialog)
		{
			if (dataContext.LinkedFilePath == string.Empty || showSaveDialog)
			{
				SaveFileDialog saveFileDialog = new SaveFileDialog
				{
					Title = DialogTitle,
					Filter = DialogFilter					
				};

				if (dataContext.LinkedFilePath != string.Empty)
				{
					saveFileDialog.InitialDirectory = Path.GetDirectoryName(dataContext.LinkedFilePath);
					saveFileDialog.FileName = dataContext.LinkedFilePath;
				}

				if (saveFileDialog.ShowDialog() == true)
				{
					dataContext.LinkedFilePath = saveFileDialog.FileName;
					dataContext.TabHeader = Path.GetFileName(saveFileDialog.FileName);
				}
				else
				{
					return;
				}
			}

			if (dataContext.IsCodeChanged == true || File.Exists(dataContext.LinkedFilePath) == false)
				File.WriteAllText(dataContext.LinkedFilePath, dataContext.ShaderCode);
			dataContext.IsCodeChanged = false;
		}
	}
}
