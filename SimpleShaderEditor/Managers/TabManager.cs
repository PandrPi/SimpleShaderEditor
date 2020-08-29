using AurelienRibon.Ui.SyntaxHighlightBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SimpleShaderEditor.Managers
{
	class TabManager
	{
		/// <summary>
		/// The reference to the EditorTabControl object from the main window
		/// </summary>
		private TabControl EditorTabControl { get; set; }
		/// <summary>
		/// TabItem instance for duplicating new tabs from.
		/// </summary>
		/// 
		private TabItem TemplateTabItem { get; set; }
		/// <summary>
		/// Stores info about each opened editor tab.
		/// </summary>
		private Dictionary<string, TabInfo> TabInfoDictionary { get; set; }

		public TabManager(TabControl tabControl, TabItem templateTabItem)
		{
			EditorTabControl = tabControl;
			TemplateTabItem = templateTabItem;
		}

		/// <summary>
		/// Adds new editor tab to the window.
		/// </summary>
		/// <param name="tabCode">Code of the local tab's editor</param>
		/// <param name="header">Tab header</param>
		public void AddNewTab(string tabCode, string header)
		{
			TabItem newTab = HelpManager.DuplicateControl<TabItem>(TemplateTabItem);
			newTab.Header = header;
			Grid tabGrid = newTab.Content as Grid;
			SyntaxHighlightBox editor = tabGrid.Children[0] as SyntaxHighlightBox; 
			editor.Text = tabCode;

			EditorTabControl.Items.Add(newTab);
			EditorTabControl.SelectedItem = newTab;
		}

		/// <summary>
		/// Adds new default editor tab to the window.
		/// </summary>
		public void AddNewDefaultTab()
		{
			AddNewTab(ConfigManager.DefaultShaderCode, ConfigManager.EditorTabDefaultHeader);
		}
	}

	struct TabInfo
	{
		public string TabTitle { get; set; }
		public string LinkedFilePath { get; set; }
		public bool IsContentChanged { get; set; }

	}
}
