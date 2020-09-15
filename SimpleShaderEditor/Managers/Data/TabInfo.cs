using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleShaderEditor.Managers.Data
{
	class TabInfo : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public string CompiledFilePath
		{
			get
			{
				return _compiledFilePath;
			}
			set
			{
				_compiledFilePath = value;
				OnPropertyChanged();
			}
		}
		public string LinkedFilePath
		{
			get
			{
				return _linkedFilePath;
			}
			set
			{
				_linkedFilePath = value;
				OnPropertyChanged();
			}
		}
		public string ShaderCode
		{
			get
			{
				return _shaderCode;
			}
			set
			{
				_shaderCode = value;
				OnPropertyChanged();
			}
		}
		public string TabHeader
		{
			get
			{
				return _tabHeader;
			}
			set
			{
				_tabHeader = value;
				OnPropertyChanged();
			}
		}
		public bool IsCodeChanged
		{
			get
			{
				return _isCodeChanged;
			}
			set
			{
				_isCodeChanged = value;
				OnPropertyChanged();
			}
		}

		private string _compiledFilePath = string.Empty;
		private string _linkedFilePath = string.Empty;
		private string _shaderCode = string.Empty;
		private string _tabHeader = string.Empty;
		private bool _isCodeChanged = false;

		public TabInfo(string tabHeader, string shaderCode, string linkedFilePath)
		{
			TabHeader = tabHeader;
			LinkedFilePath = linkedFilePath;
			ShaderCode = shaderCode;
			IsCodeChanged = false;
		}

		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
