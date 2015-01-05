using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using NumberConverter.Annotations;


namespace NumberConverter
{
	public class SettingsModelView : INotifyPropertyChanged
	{
		private Dictionary<string, Uri> themes = new Dictionary<string, Uri>()
		{
			{"DarkBlue", new Uri("ms-resource:/Files/Resource/DarkBlueTheme.xaml", UriKind.Absolute)},
			{"DarkGreen", new Uri("ms-resource:/Files/Resource/DarkGreenTheme.xaml", UriKind.Absolute)},
			{"DarkOrange", new Uri("ms-resource:/Files/Resource/DarkOrangeTheme.xaml", UriKind.Absolute)},
			{"DarkRed", new Uri("ms-resource:/Files/Resource/DarkRedTheme.xaml", UriKind.Absolute)}
		};

		public Dictionary<string, Uri> Themes
		{
			get { return themes; }
		}

		private SettingsModelView()
		{
			
		}

		private static SettingsModelView settings;
		public static SettingsModelView Settings
		{
			get { return settings ?? (settings = new SettingsModelView()); }
		}
		#region Roaming Settings
		public int Precise
		{
			get
			{
				if (!ApplicationData.Current.RoamingSettings.Values.ContainsKey("Precise"))
					Precise = 8;
				return (int)ApplicationData.Current.RoamingSettings.Values["Precise"];
			}
			set
			{
				if (!ApplicationData.Current.RoamingSettings.Values.ContainsKey("Precise"))
				{
					ApplicationData.Current.RoamingSettings.Values.Add("Precise", value);
					return;
				}
				ApplicationData.Current.RoamingSettings.Values["Precise"] = value;
				OnPropertyChanged();
			}
		}
		#endregion

		#region Local Settings

		public bool AllButtonsSame
		{
			get
			{
				if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("AllButtonsSame"))
					AllButtonsSame = false;
				return (bool)ApplicationData.Current.LocalSettings.Values["AllButtonsSame"];
			}
			set
			{
				if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("AllButtonsSame"))
					ApplicationData.Current.LocalSettings.Values.Add("AllButtonsSame", value);
				else
				{
					ApplicationData.Current.LocalSettings.Values["AllButtonsSame"] = value;
				}
				OnPropertyChanged();
			}
		}

		public int MaxSizeButton
		{
			get
			{
				if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("MaxSizeButton"))
					MaxSizeButton = 300;
				return (int)ApplicationData.Current.LocalSettings.Values["MaxSizeButton"];
			}
			set
			{
				if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("MaxSizeButton"))
					ApplicationData.Current.LocalSettings.Values.Add("MaxSizeButton", value);
				else
				{
					ApplicationData.Current.LocalSettings.Values["MaxSizeButton"] = value;
				}
				OnPropertyChanged();
			}
		}
		public Uri Theme
		{
			get { return Themes.Values.ToList()[SelectedThemeIndex]; }

			//set
			//{
			//	if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("Theme"))
			//		ApplicationData.Current.LocalSettings.Values.Add("Theme", value);
			//	else
			//	{
			//		ApplicationData.Current.LocalSettings.Values["Theme"] = value;
			//	}
			//}
		}
		

		private int selectedThemeIndex;
		public int SelectedThemeIndex
		{
			get
			{
				if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("ThemeIndex"))
					SelectedThemeIndex = 0;
				return (int)ApplicationData.Current.LocalSettings.Values["ThemeIndex"];
			}
			set
			{
				if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("ThemeIndex"))
					ApplicationData.Current.LocalSettings.Values.Add("ThemeIndex", value);
				else
				{
					ApplicationData.Current.LocalSettings.Values["ThemeIndex"] = value;
				}
				OnPropertyChanged();
			}
		}
		#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
