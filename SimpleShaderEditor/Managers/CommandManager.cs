using System.Windows.Input;

namespace SimpleShaderEditor.Managers
{
	class CommandManager
	{
		public static RoutedCommand OpenFile = new RoutedCommand();
		public static RoutedCommand SaveFile = new RoutedCommand();
	}
}
