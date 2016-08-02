﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace NumberConverter
{
	public sealed partial class SettingsThems : SettingsFlyout
	{
		public SettingsThems()
		{
			this.InitializeComponent();
		}


		public Type a;

		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			var uri = new Uri("ms-resource:/Files/Resource/DarkOrangeTheme.xaml", UriKind.Absolute);
			ChangeTheme(uri);
		}

		void ChangeTheme(Uri uri)
		{
			if (!ApplicationData.Current.LocalSettings.Values.ContainsKey("Theme"))
				ApplicationData.Current.LocalSettings.Values.Add("Theme", uri.ToString());
			else
			{
				ApplicationData.Current.LocalSettings.Values["Theme"] = uri.ToString();
			}
			ResourceDictionary newDictionary = new ResourceDictionary();
			newDictionary.Source = uri;
			Application.Current.Resources = newDictionary;
			newDictionary = new ResourceDictionary();
			newDictionary.Source = uri;
			Application.Current.Resources = newDictionary;
			//Application.Current.Resources = newDictionary;
			//var s = Window.Current.GetType();
			//bool bb = Window.Current.Content is MainPage;
			//bool dd = Window.Current.Content is 
			(Window.Current.Content as Frame).Navigate(typeof(MainPage));
			//(Window.Current.Content as Frame).Navigate(typeof(MainPage));
		}

		private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
		{
			var uri = new Uri("ms-resource:/Files/Resource/DarkBlueTheme.xaml", UriKind.Absolute);
			ChangeTheme(uri);
			//ChangeTheme(uri);
		}

		private void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
		{
			var uri = new Uri("ms-resource:/Files/Resource/DarkGreenTheme.xaml", UriKind.Absolute);
			ChangeTheme(uri);
		}

		private void HyperlinkButton_Click_3(object sender, RoutedEventArgs e)
		{
			var uri = new Uri("ms-resource:/Files/Resource/DarkRedTheme.xaml", UriKind.Absolute);
			ChangeTheme(uri);
		}
	}
}