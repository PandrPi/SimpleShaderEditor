using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace SimpleShaderEditor.Managers
{
	class HelpManager
	{
		public static T DuplicateControl<T>(FrameworkElement controlToDuplicate)
		{
			string savedXaml = XamlWriter.Save(controlToDuplicate);
			StringReader stringReader = new StringReader(savedXaml);
			XmlReader xmlReader = XmlReader.Create(stringReader);
			return (T)XamlReader.Load(xmlReader);
		}
	}
}
