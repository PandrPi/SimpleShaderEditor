using AurelienRibon.Ui.SyntaxHighlightBox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleShaderEditor.Managers
{
	class TabManager
	{
		public ObservableCollection<TabInfo> TabItemsSource { get; private set; }

		/// <summary>
		/// The reference to the EditorTabControl object from the main window
		/// </summary>
		private TabControl EditorTabControl { get; set; }

		public TabManager(TabControl tabControl)
		{
			EditorTabControl = tabControl;

			TabItemsSource = new ObservableCollection<TabInfo>();
			EditorTabControl.ItemsSource = TabItemsSource;

			// TODO: Get the list of files opened during the previous app session from the ConfigManager
			// If this list is empty just call AddNewDefaultTab method
			AddNewDefaultTab();
		}

		/// <summary>
		/// Adds a new editor tab to EditorTabControl with the code read from the file in the specified path
		/// </summary>
		/// <param name="filePath">A path of the file to read code from</param>
		public void AddNewTab(string filePath)
		{
			string tabHeader = Path.GetFileName(filePath);
			string shaderCode = File.ReadAllText(filePath, Encoding.UTF8);

			TabItemsSource.Add(new TabInfo(tabHeader, shaderCode, filePath));
		}

		/// <summary>
		/// Adds a new editor tab to the window with default shader code pre written.
		/// </summary>
		public void AddNewDefaultTab()
		{
			TabItemsSource.Add(new TabInfo(ConfigManager.EditorTabDefaultHeader, ConfigManager.DefaultShaderCode, string.Empty));
		}

		public void RemoveTab(TabInfo tabInfo)
		{
			TabItemsSource.Remove(tabInfo);
			
			if (TabItemsSource.Count == 0)
			{
				AddNewDefaultTab();
			}

			// TODO: Show confirm message box if code changes of tab's editor were not saved
			
		}
	}

	struct TabInfo
	{
		public string TabHeader { get; set; }
		public string ShaderCode { get; set; }
		public string LinkedFilePath { get; set; }
		public bool IsCodeChanged { get; set; }

		public TabInfo(string tabHeader, string shaderCode, string linkedFilePath)
		{
			TabHeader = tabHeader;
			ShaderCode = shaderCode;
			LinkedFilePath = linkedFilePath;
			IsCodeChanged = false;
		}

	}
}
