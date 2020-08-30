using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleShaderEditor.Managers
{
	class CommandManager
	{
		public static RoutedCommand OpenFile { get; set; }
		public static RoutedCommand SaveFile { get; set; }

		public static RoutedCommand TabItemClose { get; set; }

		static CommandManager()
		{
			OpenFile = new RoutedCommand("OpenFile", typeof(MenuItem));
			SaveFile = new RoutedCommand("SaveFile", typeof(MenuItem));
			TabItemClose = new RoutedCommand("TabItemClose", typeof(Button));
		}
	}
}
