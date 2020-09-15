using System.Windows.Controls;
using System.Windows.Input;

namespace SimpleShaderEditor.Managers
{
	class CommandManager
	{
		public static RoutedCommand CompileShader { get; set; } = new RoutedCommand();
		public static RoutedCommand NewFile { get; set; } = new RoutedCommand();
		public static RoutedCommand OpenFile { get; set; } = new RoutedCommand();
		public static RoutedCommand SaveFile { get; set; } = new RoutedCommand();

		public static RoutedCommand TabItemClose { get; set; } = new RoutedCommand();
	}
}
