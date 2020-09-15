using AurelienRibon.Ui.SyntaxHighlightBox;
using SimpleShaderEditor.Managers.Data;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SimpleShaderEditor.Managers
{
	class TabManager
	{
		public static IHighlighter Highlighter { get; set; } = HighlighterManager.Instance.Highlighters["HLSL"];
		private static readonly string TabRemovalConfirmationWindow_Caption = "The changes is not saved!";
		private static readonly string TabRemovalConfirmationWindow_Content = "Do you want to save file '{0}'?";

		public ObservableCollection<TabInfo> TabItemsSource { get; private set; }
		public SyntaxHighlightBox CodeEditorBox { get; set; }

		/// <summary>
		/// The reference to the EditorTabControl object from the main window
		/// </summary>
		private TabControl EditorTabControl { get; set; }

		public TabManager(TabControl tabControl)
		{
			EditorTabControl = tabControl;

			TabItemsSource = new ObservableCollection<TabInfo>();
			EditorTabControl.ItemsSource = TabItemsSource;

			// In order to initialize our code editor instance we need to create a temporal tab	
			// When our code editor will be loaded to UI we need to remove this temporal tab from the source list
			TabItemsSource.Add(new TabInfo(string.Empty, string.Empty, "TemporalPath"));

			// TODO: Get the list of files opened during the previous app session from the ConfigManager
			// If this list is empty just call AddNewDefaultTab method
			AddNewDefaultTab();
		}

		/// <summary>
		/// This method initializes the CodeEditorBox property
		/// </summary>
		/// <param name="codeEditorBox"></param>
		public void InitializeCodeEditorBox(SyntaxHighlightBox codeEditorBox)
		{
			// It is necessary to null check, because the tab removal causes the CodeEditorBox_Loaded event and this method
			//to be raised/called again. So when the CodeEditorBox is already initialized we do not need to do initialization again.
			if (CodeEditorBox == null)
			{
				CodeEditorBox = codeEditorBox;				

				// Just remove a temporal element by index 0, because it is useless now
				TabItemsSource.RemoveAt(0);
			}
		}

		/// <summary>
		/// Adds a new editor tab to EditorTabControl with the code read from the file in the specified path
		/// </summary>
		/// <param name="filePath">A path of the file to read code from</param>
		public void AddNewTab(string filePath)
		{
			string tabHeader = Path.GetFileName(filePath);
			string shaderCode = File.ReadAllText(filePath, Encoding.UTF8);
			TabInfo tabInfo = new TabInfo(tabHeader, shaderCode, filePath);

			AddOrSelectTab(tabInfo);
		}

		/// <summary>
		/// Adds a new editor tab to the window with default shader code pre written.
		/// </summary>
		public void AddNewDefaultTab()
		{
			string header = ConfigManager.EditorTabDefaultHeader.Clone().ToString();
			string shaderCode = ConfigManager.DefaultShaderCode.Clone().ToString();
			TabInfo tabInfo = new TabInfo(header, shaderCode, string.Empty);

			AddOrSelectTab(tabInfo);
		}

		/// <summary>
		/// This method adds a new TabInfo object to the TabItemSource collection when there is no equal object, otherwise
		/// this method selects a tab based on tabInfo object that already exists in the source collection
		/// </summary>
		/// <param name="tabInfo">A TabInfo object on which the new tab should be based</param>
		private void AddOrSelectTab(TabInfo tabInfo)
		{
			// Search for item with the same LinkedFilePath and ShaderCode, foundItem will be null when there is no item with			
			//the same file path and shader code in the source list
			var foundItem = TabItemsSource.Where(i => i.LinkedFilePath
											 == tabInfo.LinkedFilePath && i.LinkedFilePath != string.Empty).FirstOrDefault();
			if (foundItem != null)
			{
				SelectTabItemByInfo(foundItem);
			}
			else
			{
				TabItemsSource.Add(tabInfo);
			}
		}



		/// <summary>
		/// Removes the tab associated with the tabInfo parameter from the source list if the list contains more than 1 item
		/// </summary>
		/// <param name="tabInfo"></param>
		public void RemoveTab(TabInfo tabInfo)
		{
			if (TabItemsSource.Count == 1)
				return;

			var windowResult = ShowTabRemovalConfirmationWindow(tabInfo);
			if (windowResult != MessageBoxResult.Cancel)
				TabItemsSource.Remove(tabInfo);
		}

		/// <summary>
		/// Shows the tab removal confirmation window if the tab has unsaved changes
		/// </summary>
		/// <param name="tabInfo"></param>
		/// <returns></returns>
		public MessageBoxResult ShowTabRemovalConfirmationWindow(TabInfo tabInfo)
		{
			if (tabInfo.IsCodeChanged == true)
			{
				var dialogResult = MessageBox.Show(
					string.Format(TabRemovalConfirmationWindow_Content, Path.GetFileName(tabInfo.LinkedFilePath)),
					TabRemovalConfirmationWindow_Caption,
					MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

				if (dialogResult != MessageBoxResult.Cancel)
				{
					if (dialogResult == MessageBoxResult.Yes)
						FileManager.SaveFile(tabInfo, false);

					return dialogResult;
				}
				return MessageBoxResult.Cancel;
			}

			return MessageBoxResult.None;
		}

		/// <summary>
		/// Removes each tab from the ItemSource collection. Call this method when the application is closing.
		/// </summary>
		/// <returns>Boolean object that determines whether user has canceled the operation</returns>
		public bool RemoveAllTabs()
		{
			foreach (var tabItem in TabItemsSource)
			{
				var dialogResult = ShowTabRemovalConfirmationWindow(tabItem);
				if (dialogResult == MessageBoxResult.Cancel)
					return true;
			}
			TabItemsSource = null;
			return false;
		}

		/// <summary>
		/// Makes the tab associated with the tabinfo parameter an active tab
		/// </summary>
		/// <param name="tabInfo"></param>
		public void SelectTabItemByInfo(TabInfo tabInfo)
		{
			EditorTabControl.SelectedIndex = TabItemsSource.IndexOf(tabInfo);
		}
	}	
}
