using SimpleShaderEditor.Managers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


using AurelienRibon.Ui.SyntaxHighlightBox;
using SimpleShaderEditor.Managers.Data;

namespace SimpleShaderEditor
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private ConfigManager ConfigManager { get; set; }
		private TabManager TabManager { get; set; }

		public MainWindow()
		{
			InitializeComponent();
		}

		#region EventHandlers
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			ConfigManager = new ConfigManager();
			TabManager = new TabManager(EditorTabControl);
		}

		private void NewTabMenuItem_Click(object sender, RoutedEventArgs e)
		{
			TabManager.AddNewDefaultTab();
		}

		private void EditorHeader_Loaded(object sender, RoutedEventArgs e)
		{
			TabInfo tabInfo = (TabInfo)(sender as FrameworkElement).DataContext;
			TabManager.SelectTabItemByInfo(tabInfo);
		}

		private void OpenFileCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			LoadScriptFromFile();
		}

		/// <summary>
		/// This method saves the shader code of the current tab to the file selected in the dialog box. e.Parameter property
		/// must contain a boolean object that is responsible for whether the code save operation is a 'Save As' operation or not.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveFileCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			bool isSaveAsOperation = Convert.ToBoolean(e.Parameter);
			TabInfo selectedTabDataContext = TabManager.TabItemsSource[EditorTabControl.SelectedIndex];

			FileManager.SaveFile(selectedTabDataContext, isSaveAsOperation);
		}

		/// <summary>
		/// This method closes the tab which data is passed in the e.Parameter property. e.Parameter property must contain a TabTnfo object.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CloseTabCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			TabManager.RemoveTab((TabInfo)e.Parameter);
		}

		private void CompileShaderCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// TODO: Call compilation method here
		}

		/// <summary>
		/// This event is handled when user select another editor tab.
		/// </summary>
		/// <param name="sender">The instance of code editor</param>
		/// <param name="e"></param>
		private void CodeEditorBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			SyntaxHighlightBox codeEditorBox = (SyntaxHighlightBox)sender;
			// We need to know when editor TextChanged event is raised because of the changed context
			codeEditorBox.Tag = ConfigManager.TabChangedTag;
		}

		/// <summary>
		/// This event is handled when editor text is changed
		/// </summary>
		/// <param name="sender">The instance of code editor</param>
		/// <param name="e"></param>
		private void CodeEditorBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (sender != null)
			{
				var codeEditor = sender as SyntaxHighlightBox;
				bool isCausedByTabChanging = (string)codeEditor.Tag == ConfigManager.TabChangedTag;
				if (isCausedByTabChanging == false)
				{
					TabInfo tabDataContext = codeEditor.DataContext as TabInfo;
					tabDataContext.IsCodeChanged = true;
				}
				codeEditor.Tag = null;
			}
		}

		/// <summary>
		/// This event is handled when the CodeEditorBox control is loaded to UI
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CodeEditorBox_Loaded(object sender, RoutedEventArgs e)
		{
			// We need to initialize the CodeEditorBox property(our code editor) with the sender object
			TabManager.InitializeCodeEditorBox(sender as SyntaxHighlightBox);
		}
		#endregion

		// Methods that initialize something or are used in initialization
		#region InitializationMethods

		#endregion

		// Methods that interacts with other classes
		#region InteractionMethods
		private void LoadScriptFromFile()
		{
			string filePath = FileManager.GetSelectedFilePathFromDialogWindow();

			// Check if OpenFileDialog was not canceled by the user
			if (filePath != ConfigManager.DialogCanceledMessage)
			{
				// If there is no tab in TabManager that is linked with a file path selected from a DialogBox by user
				// we should create a new editor tab, otherwise select the existing tab
				TabManager.AddNewTab(filePath);
			}
		}

		#endregion
	}
}
