using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NumberConverter
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		Keyboard keyboard;
		ComboBox parentFlyout;

		int fromBase;
		int toBase;
		int fromBaseIndex,  toBaseIndex;
		static SuspendPage suspendPage;
		//string BAse;
		public int FromBase
		{
			get { return fromBaseIndex; }
			set
			{
				fromBaseIndex = value;
				fromBase = int.Parse(((ComboBoxItem)From.Items[value]).Content.ToString());
			}
		}

		public int ToBase
		{
			get { return toBaseIndex; }
			set
			{
				toBaseIndex = value;
				toBase = int.Parse(((ComboBoxItem)To.Items[value]).Content.ToString());
			}
		}
		public MainPage()
		{
			this.InitializeComponent();
			DataContext = this;
			CreateKeyboard(Buttons);
			ToBase = 0;
			FromBase = 0;
			//fromBase = int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString());
			//toBase = int.Parse(((ComboBoxItem)To.SelectedItem).Content.ToString());
			if (suspendPage != null)
			{
				//FromBase = suspendPage.indexFrom;
				//FromBase2 = suspendPage.indexFrom2;
				//ToBase = suspendPage.indexTo;
				//InputText.Text = suspendPage.InputText;
				//InputText2.Text = suspendPage.InputText2;
				//keyboard.ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, fromBase + 2);
			}
			keyboard.SetVisibleButton(fromBase);
			
		}

		public void CreateKeyboard(Panel panel)
		{
			keyboard = new Keyboard(panel);
			for (int i = 0; i < panel.Children.Count - 2; i++)
			{
				((Button)(panel.Children[i])).Click += Button_Click_1;
			}
			((Button)(panel.Children[panel.Children.Count - 2])).Click += Button_Click_Dot; // "."
			((Button)(panel.Children[panel.Children.Count - 2])).SizeChanged += Buttons_SizeChanged;
			((Button)(panel.Children[panel.Children.Count - 1])).Click += Backspace_Click; //backspace
		}

		private void Button_Click_Dot(object sender, RoutedEventArgs e)
		{
			InputText.Focus(Windows.UI.Xaml.FocusState.Pointer);
			if (InputText.Text.IndexOf(((Button)sender).Content.ToString()) < 0) //точки нету
				Button_Click_1(sender, e);
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				if (InputText.Text != "")
					Result.Text = Converter.Converter.ConvertTo((uint)fromBase,
					InputText.Text, (uint)toBase);
			}
			catch (Exception ee)
			{
				Result.Text = ee.Message;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			InputText.Text = "";
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			SharePages.AddTextTextBox(((Button)sender).Content.ToString(), InputText);
		}
		
		private void InputText_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			//if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Touch)
			//	((TextBox)sender).IsReadOnly = true;
			//else
			//	((TextBox)sender).IsReadOnly = false;
		}

		private void From_Holding(object sender, HoldingRoutedEventArgs e)
		{
			FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
			parentFlyout = (ComboBox)sender;
		}

		private void From_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				if (sender != null && Result != null)
				{
					fromBase = int.Parse((((ComboBoxItem)((ComboBox)sender).SelectedItem)).Content.ToString());
					Result.Text = Converter.Converter.ConvertTo((uint)fromBase,	InputText.Text, (uint)toBase);
					keyboard.SetVisibleButton(fromBase);
					keyboard.ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, fromBase + 2);
				}
			}
			catch (Exception ee)
			{
				Result.Text = ee.Message;
			}
		}

		private void To_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				if (From != null || To != null)
				{
					toBase = int.Parse(((ComboBoxItem)To.SelectedItem).Content.ToString());
					Result.Text = Converter.Converter.ConvertTo((uint)fromBase,
						InputText.Text, (uint) toBase);
				}
			}
			catch (Exception ee)
			{
				Result.Text = ee.Message;
			}
		}

		private void Result_SizeChanged(object sender, SizeChangedEventArgs e)
		{

			SharePages.ResizeFontTextBox((TextBox)sender);
		}
				
		private void Buttons_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			//var x = ((Button)sender);
			//if (x.Height == double.NaN)

			//x.Height = Clearr.ActualWidth-50;
			//x.Width = Clearr.ActualWidth -50;
			//x.FontSize = x.Height - 50;
		}

		private void Buttons_SizeChanged_1(object sender, SizeChangedEventArgs e)
		{

		}

		

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var statView = ApplicationView.GetForCurrentView();
			if (statView.IsFullScreen)
				if (statView.Orientation == ApplicationViewOrientation.Landscape)
				{
					//MainGrid.Margin = new Thickness(10, 10, 10, 10);
					Grid.SetColumnSpan(From, 1);
					Grid.SetRow(InputText, 1);
					Grid.SetColumn(InputText, 1);
					Grid.SetRow(To, 2);
					Grid.SetRowSpan(To, 1);
					Grid.SetColumn(To, 0);
					Grid.SetColumnSpan(To, 1);
					//To.Margin = new Thickness(0, 0, 0, 0);
					//To.Height = double.NaN;
					//To.Width = double.NaN;
					Grid.SetRow(Result, 2);
					Grid.SetColumn(Result, 1);
					Grid.SetRow(sizeKeyboard, 3);
					Grid.SetRowSpan(sizeKeyboard, 4);
					Grid.SetRow((FrameworkElement)Buttons, 3);
					Grid.SetRowSpan((FrameworkElement)Buttons, 4);
				}
				else
				{
				//	MainGrid.Margin = new Thickness(10, 50, 10, 50);
					Grid.SetColumnSpan(From, 2);
					Grid.SetRow(InputText, 2);
					Grid.SetColumn(InputText, 0);
					Grid.SetRow(To, 1);
					Grid.SetColumn(To, 3);
					Grid.SetColumnSpan(To, 2);

					//To.Margin = new Thickness(0, 0, 0, 0);
					//To.Height = double.NaN;
					//To.Width = double.NaN;
					Grid.SetRow(Result, 3);
					Grid.SetColumn(Result, 0);
					Grid.SetRow(sizeKeyboard, 4);
					Grid.SetRowSpan(sizeKeyboard, 1);
					Grid.SetRow((FrameworkElement)Buttons, 4);
					Grid.SetRowSpan((FrameworkElement)Buttons, 1);
				}
			else
			{
			//	MainGrid.Margin = new Thickness(10, 50, 10, 50);
				Grid.SetColumnSpan(From, 2);
				Grid.SetRow(InputText, 2);
				Grid.SetColumn(InputText, 0);
				Grid.SetRow(To, 1);
				Grid.SetColumn(To, 3);
				Grid.SetColumnSpan(To, 2);

				//To.Margin = new Thickness(0, 0, 0, 0);
				//To.Height = double.NaN;
				//To.Width = double.NaN;
				Grid.SetRow(Result, 3);
				Grid.SetColumn(Result, 0);
				Grid.SetRow(sizeKeyboard, 4);
				Grid.SetRowSpan(sizeKeyboard, 1);
				Grid.SetRow((FrameworkElement)Buttons, 4);
				Grid.SetRowSpan((FrameworkElement)Buttons, 1);
			}
			//VisualStateManager.GoToState(this, "FullScreenLandscape", true);
			
		}

		private void From_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			SharePages.ScaleText((ComboBox)sender, e.NewSize.Height);
			//double scale = e.PreviousSize.Height / e.NewSize.Height;
			//((ComboBox)sender).FontSize = (e.NewSize.Height) * 0.7;
			//combo.Height = e.NewSize.Height;
			//combo.Width = e.NewSize.Width;
			//combo.FontSize = e.NewSize.Height * 0.7;
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			//MainGrid.Visibility = Visibility.Collapsed;
			suspendPage = new SuspendPage();
			suspendPage.indexFrom = FromBase;
			//suspendPage.indexFrom2 = FromBase2;
			suspendPage.indexTo = ToBase;
			suspendPage.InputText = InputText.Text;
			//suspendPage.InputText2 = InputText2.Text;
			this.Frame.Navigate(typeof(BlankPage1));
		}

		private void Button_GotFocus(object sender, RoutedEventArgs e)
		{
			
		}

		private void sizeKeyboard_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			keyboard.ResizeButton(e.NewSize.Height, e.NewSize.Width, int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString()) + 2);
		}
		
		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			this.Frame.Navigate(typeof(BlankPage1));
		}

		private void From_RightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
			parentFlyout = (ComboBox)sender;
		}

		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var listbox = ((ListBox)sender);
			var str = ((ListBoxItem)listbox.SelectedItem).Content;
			//var fly = ((Flyout)((FlyoutPresenter)((Grid)((ListBox)sender).Parent).Parent).Parent);
			var comboItem = new ComboBoxItem();
			comboItem.Content = str;
			SharePages.AddComboBoxItem(comboItem, parentFlyout, true);
			//parentFlyout.SelectedIndex = parentFlyout.Items.Count - 1;
			//fly.Hide();
			openedFlyout.Hide();
			listbox.SelectionChanged -= ListBox_SelectionChanged;
			listbox.SelectedIndex = -1;
			listbox.SelectionChanged += ListBox_SelectionChanged;
		}

		Flyout openedFlyout;
		private void Flyout_Opened(object sender, object e)
		{
			openedFlyout = sender as Flyout;
		}

		private void InputText_Tapped(object sender, TappedRoutedEventArgs e)
		{
			//if (e.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Touch)
			//	((TextBox)sender).IsReadOnly = true;
			//else
			//	((TextBox)sender).IsReadOnly = false;
		}

		private void InputText_GotFocus(object sender, RoutedEventArgs e)
		{
			
		}

		private void InputText_SelectionChanged(object sender, RoutedEventArgs e)
		{
			
		}

		private void InputText_PointerReleased(object sender, PointerRoutedEventArgs e)
		{

		}

		private void Backspace_Click(object sender, RoutedEventArgs e)
		{
			SharePages.Backspace(InputText);
			InputText.Select(InputText.Text.Length, 0);
		}

		
		private void InputText_KeyUp(object sender, KeyRoutedEventArgs e)
		{
			//if ("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(e.Key.ToString())) ;
			
			//if (e.Key)
			SharePages.InputText_KeyUp(sender, e, fromBase);
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			ResourceDictionary newDictionary = new ResourceDictionary();
			System.UriBuilder a = new UriBuilder();
			newDictionary.Source = new Uri("ms-resource:/Files/Resource/DarkOrange.xaml", UriKind.Absolute);
		///	newDictionary.Source = new Uri(@"Resource/DarkOrange.xaml", UriKind.RelativeOrAbsolute);
			Application.Current.Resources = newDictionary;
			this.UpdateLayout();
		}

		private void HyperlinkButton_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			((ButtonBase) sender).FontSize = e.NewSize.Height*0.5;
		}
		
	}
}
