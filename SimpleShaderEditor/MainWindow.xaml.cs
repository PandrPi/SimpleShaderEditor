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
			TabManager = new TabManager(EditorTabControl, FindResource("TemplateTabItem") as TabItem);

			Initialize_EditorTabControl();
		}

		private void NewTabMenuItem_Click(object sender, RoutedEventArgs e)
		{
			TabManager.AddNewDefaultTab();
		}

		private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e)
		{
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
		#endregion

		// Methods that initialize something or are used in initialization
		#region InitializationMethods
		/// <summary>
		/// Creates new editor tab for each file the user was working on during the previous app session 
		/// </summary>
		private void Initialize_EditorTabControl()
		{
			TabManager.AddNewDefaultTab();
			// TODO: Get the list of files opened during the previous app session from the ConfigManager
		}
		
		#endregion

		// Methods that interacts with other classes
		#region InteractionMethods
		private void LoadScriptFromFile()
		{
			string filePath = FileManager.GetSelectedFilePathFromDialog();

			// Check if OpenFileDialog was not canceled by the user
			if (filePath != ConfigManager.DialogCanceledMessage)
			{
				// If there is no tab in TabManager that is linked with
			}
		}
		#endregion
	}
}
