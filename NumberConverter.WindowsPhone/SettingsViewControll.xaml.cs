using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236
using NumberConverter.Common;

namespace NumberConverter
{
	public sealed partial class SettingsViewControll : UserControl
	{
		public SettingsViewControll()
		{
			this.InitializeComponent();

			//this.navigationHelper = new NavigationHelper(this);
			//this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
			//this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
			//ContentRoot.SelectionChanged += null;
			settigns = SettingsModelView.Settings;
			DataContext = settigns;
			//ContentRoot.SelectionChanged += ContentRoot_SelectionChanged;
		}

		private NavigationHelper navigationHelper;
		private ObservableDictionary defaultViewModel = new ObservableDictionary();
		private readonly SettingsModelView settigns;

		/// <summary>
		/// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
		/// </summary>
		//public NavigationHelper NavigationHelper
		//{
		//	get { return this.navigationHelper; }
		//}

		/// <summary>
		/// Gets the view model for this <see cref="Page"/>.
		/// This can be changed to a strongly typed view model.
		/// </summary>
		public ObservableDictionary DefaultViewModel
		{
			get { return this.defaultViewModel; }
		}

		/// <summary>
		/// Populates the page with content passed during navigation.  Any saved state is also
		/// provided when recreating a page from a prior session.
		/// </summary>
		/// <param name="sender">
		/// The source of the event; typically <see cref="NavigationHelper"/>
		/// </param>
		/// <param name="e">Event data that provides both the navigation parameter passed to
		/// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
		/// a dictionary of state preserved by this page during an earlier
		/// session.  The state will be null the first time a page is visited.</param>
		private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
		{
		}

		/// <summary>
		/// Preserves state associated with this page in case the application is suspended or the
		/// page is discarded from the navigation cache.  Values must conform to the serialization
		/// requirements of <see cref="SuspensionManager.SessionState"/>.
		/// </summary>
		/// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
		/// <param name="e">Event data that provides an empty dictionary to be populated with
		/// serializable state.</param>
		private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
		{
		}

		#region NavigationHelper registration

		/// <summary>
		/// The methods provided in this section are simply used to allow
		/// NavigationHelper to respond to the page's navigation methods.
		/// <para>
		/// Page specific logic should be placed in event handlers for the  
		/// <see cref="NavigationHelper.LoadState"/>
		/// and <see cref="NavigationHelper.SaveState"/>.
		/// The navigation parameter is available in the LoadState method 
		/// in addition to page state preserved during an earlier session.
		/// </para>
		/// </summary>
		/// <param name="e">Provides data for navigation methods and event
		/// handlers that cannot cancel the navigation request.</param>
		//protected override void OnNavigatedTo(NavigationEventArgs e)
		//{
		//	this.navigationHelper.OnNavigatedTo(e);
		//}

		//protected override void OnNavigatedFrom(NavigationEventArgs e)
		//{
		//	this.navigationHelper.OnNavigatedFrom(e);
		//}

		#endregion

		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			var uri = new Uri("ms-resource:/Files/Resource/DarkBlueTheme.xaml", UriKind.Absolute);
			ChangeTheme(uri);
		}

		void ChangeTheme(Uri uri)
		{
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
			//if (Frame != null && Frame.CanGoBack)
			//	Frame.GoBack();
			//(Window.Current.Content as Frame).Navigate(typeof(MainPage));
			(Window.Current.Content as Frame).Navigate(typeof(MainPage));
		}


		private void HyperlinkButton_Click_4(object sender, RoutedEventArgs e)
		{
			GoToConverter();
		}

		private void HyperlinkButton_Click_5(object sender, RoutedEventArgs e)
		{
			GoToCalculator();
		}

		private void HyperlinkButton_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			((ButtonBase)sender).FontSize = e.NewSize.Height * 0.5;
		}

		void GoToCalculator()
		{
			//Frame.Navigate(typeof(BlankPage1));
		}

		void GoToConverter()
		{
			//Frame.Navigate(typeof(MainPage));
		}

		private void ContentRoot_ItemClick(object sender, ItemClickEventArgs e)
		{
			//settigns.Theme = settigns.Themes[((HyperlinkButton)sender).Content.ToString()];
			ChangeTheme(settigns.Theme);
		}

		private void ContentRoot_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ChangeTheme(settigns.Theme);
		}

		private void UserControl_Unloaded(object sender, RoutedEventArgs e)
		{
			this.navigationHelper = null;
			this.defaultViewModel = null;
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			ContentRoot.SelectionChanged += ContentRoot_SelectionChanged;
		}
	}
}
