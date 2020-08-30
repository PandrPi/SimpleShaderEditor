using SimpleShaderEditor.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using AurelienRibon.Ui.SyntaxHighlightBox;

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

		private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e)
		{
			LoadScriptFromFile();
			Console.WriteLine("Open");
		}

		private void SaveFileMenuItem_Click(object sender, RoutedEventArgs e)
		{
			Console.WriteLine("Save");
		}

		private void SaveFileAsMenuItem_Click(object sender, RoutedEventArgs e)
		{
			Console.WriteLine("Save As");
		}

		private void CloseTabCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			TabManager.RemoveTab((TabInfo)e.Parameter);
		}
		#endregion

		// Methods that initialize something or are used in initialization
		#region InitializationMethods
				
		#endregion

		// Methods that interacts with other classes
		#region InteractionMethods
		private void LoadScriptFromFile()
		{
			string filePath = FileManager.GetSelectedFilePathFromOpenFileDialog();

			// Check if OpenFileDialog was not canceled by the user
			if (filePath != ConfigManager.DialogCanceledMessage)
			{
				// If there is no tab in TabManager that is linked with a file path selected from a DialogBox
				// we should create a new editor tab, otherwise select the existing tab
				TabManager.AddNewTab(filePath);
			}
		}
		#endregion
	}
}
