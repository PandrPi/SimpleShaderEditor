using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace SimpleShaderEditor.Managers
{
	public class HighlighterManager
	{
		// Fields
		private static HighlighterManager instance = new HighlighterManager();

		// Methods
		public HighlighterManager()
		{
			this.Highlighters = new Dictionary<string, AurelienRibon.Ui.SyntaxHighlightBox.IHighlighter>();
			XmlSchema schema = XmlSchema.Read(Application.GetResourceStream(new Uri("pack://application:,,,/AurelienRibon.Ui.SyntaxHighlightBox;component/resources/syntax.xsd")).Stream, delegate (object s, ValidationEventArgs e) {
			});
			XmlReaderSettings settings = new XmlReaderSettings();
			settings.Schemas.Add(schema);
			settings.ValidationType = ValidationType.Schema;
			foreach (KeyValuePair<string, UnmanagedMemoryStream> pair in this.GetResources("resources/(.+?)[.]xml"))
			{
				XDocument document = null;
				try
				{
					document = XDocument.Load(XmlReader.Create(pair.Value, settings));
				}
				catch (XmlSchemaValidationException)
				{
					break;
				}
				catch (Exception)
				{
					break;
				}
				XElement root = document.Root;
				string key = root.Attribute("name").Value.Trim();
				this.Highlighters.Add(key, new XmlHighlighter(root));
			}
		}

		private IDictionary<string, UnmanagedMemoryStream> GetResources(string filter)
		{
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			IDictionary<string, UnmanagedMemoryStream> dictionary = new Dictionary<string, UnmanagedMemoryStream>();
			foreach (DictionaryEntry entry in new ResourceReader(callingAssembly.GetManifestResourceStream(callingAssembly.GetName().Name + ".g.resources")))
			{
				string key = (string)entry.Key;
				UnmanagedMemoryStream stream2 = (UnmanagedMemoryStream)entry.Value;
				if (Regex.IsMatch(key, filter))
				{
					dictionary.Add(key, stream2);
				}
			}
			return dictionary;
		}

		// Properties
		public static HighlighterManager Instance =>
			instance;

		public IDictionary<string, AurelienRibon.Ui.SyntaxHighlightBox.IHighlighter> Highlighters { get; private set; }

		// Nested Types
		private class AdvancedHighlightRule
		{
			// Methods
			public AdvancedHighlightRule(XElement rule)
			{
				this.Expression = rule.Element("Expression").Value.Trim();
				this.Options = new HighlighterManager.RuleOptions(rule);
			}

			// Properties
			public string Expression { get; private set; }

			public HighlighterManager.RuleOptions Options { get; private set; }
		}

		private class HighlightLineRule
		{
			// Methods
			public HighlightLineRule(XElement rule)
			{
				this.LineStart = rule.Element("LineStart").Value.Trim();
				this.Options = new HighlighterManager.RuleOptions(rule);
			}

			// Properties
			public string LineStart { get; private set; }

			public HighlighterManager.RuleOptions Options { get; private set; }
		}

		private class HighlightWordsRule
		{
			// Methods
			public HighlightWordsRule(XElement rule)
			{
				this.Words = new List<string>();
				this.Options = new HighlighterManager.RuleOptions(rule);
				foreach (string str2 in Regex.Split(rule.Element("Words").Value, @"\s+"))
				{
					if (!string.IsNullOrWhiteSpace(str2))
					{
						this.Words.Add(str2.Trim());
					}
				}
			}

			// Properties
			public List<string> Words { get; private set; }

			public HighlighterManager.RuleOptions Options { get; private set; }
		}

		private class RuleOptions
		{
			// Methods
			public RuleOptions(XElement rule)
			{
				string str = rule.Element("IgnoreCase").Value.Trim();
				string str2 = rule.Element("Foreground").Value.Trim();
				string str3 = rule.Element("FontWeight").Value.Trim();
				string str4 = rule.Element("FontStyle").Value.Trim();
				this.IgnoreCase = bool.Parse(str);
				this.Foreground = (Brush)new BrushConverter().ConvertFrom(str2);
				this.FontWeight = (FontWeight)new FontWeightConverter().ConvertFrom(str3);
				this.FontStyle = (FontStyle)new FontStyleConverter().ConvertFrom(str4);
			}

			// Properties
			public bool IgnoreCase { get; private set; }

			public Brush Foreground { get; private set; }

			public FontWeight FontWeight { get; private set; }

			public FontStyle FontStyle { get; private set; }
		}

		private class XmlHighlighter : AurelienRibon.Ui.SyntaxHighlightBox.IHighlighter
		{
			// Fields
			private List<HighlighterManager.HighlightWordsRule> wordsRules = new List<HighlighterManager.HighlightWordsRule>();
			private List<HighlighterManager.HighlightLineRule> lineRules = new List<HighlighterManager.HighlightLineRule>();
			private List<HighlighterManager.AdvancedHighlightRule> regexRules = new List<HighlighterManager.AdvancedHighlightRule>();

			// Methods
			public XmlHighlighter(XElement root)
			{
				foreach (XElement element in root.Elements())
				{
					string str = element.Name.ToString();
					if (str == null)
					{
						continue;
					}
					if (str == "HighlightWordsRule")
					{
						this.wordsRules.Add(new HighlighterManager.HighlightWordsRule(element));
						continue;
					}
					if (str == "HighlightLineRule")
					{
						this.lineRules.Add(new HighlighterManager.HighlightLineRule(element));
						continue;
					}
					if (str == "AdvancedHighlightRule")
					{
						this.regexRules.Add(new HighlighterManager.AdvancedHighlightRule(element));
					}
				}
			}

			public int Highlight(FormattedText text, int previousBlockCode)
			{
				foreach (Match match in new Regex("[a-zA-Z_][a-zA-Z0-9_]*").Matches(text.Text))
				{
					foreach (HighlighterManager.HighlightWordsRule rule in this.wordsRules)
					{
						foreach (string str in rule.Words)
						{
							if (rule.Options.IgnoreCase)
							{
								if (!match.Value.Equals(str, StringComparison.InvariantCultureIgnoreCase))
								{
									continue;
								}
								text.SetForegroundBrush(rule.Options.Foreground, match.Index, match.Length);
								text.SetFontWeight(rule.Options.FontWeight, match.Index, match.Length);
								text.SetFontStyle(rule.Options.FontStyle, match.Index, match.Length);
								continue;
							}
							if (match.Value == str)
							{
								text.SetForegroundBrush(rule.Options.Foreground, match.Index, match.Length);
								text.SetFontWeight(rule.Options.FontWeight, match.Index, match.Length);
								text.SetFontStyle(rule.Options.FontStyle, match.Index, match.Length);
							}
						}
					}
				}
				foreach (HighlighterManager.AdvancedHighlightRule rule2 in this.regexRules)
				{
					Regex regex2 = new Regex(rule2.Expression);
					IEnumerator enumerator = regex2.Matches(text.Text).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Match current = (Match)enumerator.Current;
							text.SetForegroundBrush(rule2.Options.Foreground, current.Index, current.Length);
							text.SetFontWeight(rule2.Options.FontWeight, current.Index, current.Length);
							text.SetFontStyle(rule2.Options.FontStyle, current.Index, current.Length);
						}
					}
					finally
					{
						IDisposable disposable2 = enumerator as IDisposable;
						if (disposable2 != null)
						{
							disposable2.Dispose();							
						}
					}
				}
				foreach (HighlighterManager.HighlightLineRule rule3 in this.lineRules)
				{
					Regex regex3 = new Regex(Regex.Escape(rule3.LineStart) + ".*");
					IEnumerator enumerator = regex3.Matches(text.Text).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Match current = (Match)enumerator.Current;
							text.SetForegroundBrush(rule3.Options.Foreground, current.Index, current.Length);
							text.SetFontWeight(rule3.Options.FontWeight, current.Index, current.Length);
							text.SetFontStyle(rule3.Options.FontStyle, current.Index, current.Length);
						}
					}
					finally
					{
						IDisposable disposable3 = enumerator as IDisposable;
						if (disposable3 != null)
						{
							disposable3.Dispose();
						}
					}
				}
				return -1;
			}
		}
	}

}
