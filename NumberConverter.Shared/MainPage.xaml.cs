using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
		int fromBase;
		int toBase;
		ComboBox parentFlyout;
		public MainPage()
		{
			this.InitializeComponent();
			CreateKeyboard(Buttons);
			fromBase = int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString());
			toBase = int.Parse(((ComboBoxItem)To.SelectedItem).Content.ToString());
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
				if (From != null || To != null)
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
			//FocusManager.GetFocusedElement()			
			InputText.Focus(Windows.UI.Xaml.FocusState.Pointer);
			var x = InputText.SelectionStart; //временное запоминание
			InputText.Text = InputText.Text.Insert(InputText.SelectionStart, ((Button)sender).Content.ToString());
			InputText.SelectionStart = x + ((Button)sender).Content.ToString().Length;
			//InputText.Text += ((Button)sender).Content.ToString();
		}
				

		private void InputText_PointerPressed(object sender, PointerRoutedEventArgs e)
		{

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
				if (From != null || To != null)
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
			
			var x = ((TextBox)sender);
			//if (x.Height == double.NaN)
			var statView = ApplicationView.GetForCurrentView();
			if (statView.IsFullScreen)
			{
				x.TextWrapping = TextWrapping.Wrap;
				x.FontSize = x.ActualHeight * 0.4;
			}
			else
			{
				x.TextWrapping = TextWrapping.NoWrap;
				x.FontSize = x.ActualHeight * 0.8;
			}
			//Button_SizeChanged(Clearr, e);
		}

		private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var x = ((Button)sender);
			//if (x.Height == double.NaN)
			x.FontSize = x.ActualHeight*0.8;
			//ResizeButton();
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

		void AddComboBoxItem(ComboBoxItem item, ComboBox combobox, bool isSelet)
		{
			if (!combobox.Items.Any((a) =>
			{
				if (((ComboBoxItem)a).Content.ToString() == item.Content.ToString())
					return true;
				return false;
			}
			))
			{
				combobox.Items.Add(item);
				combobox.SelectedItem = combobox.Items.Last();
			}
			else
			{
				combobox.SelectedIndex = combobox.Items.IndexOf(combobox.Items.Single((a) =>   ((ComboBoxItem)a).Content.ToString() == item.Content.ToString()));
			}
		}

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var statView = ApplicationView.GetForCurrentView();
			if (statView.IsFullScreen)
				if (statView.Orientation == ApplicationViewOrientation.Landscape)
				{
					MainGrid.Margin = new Thickness(10, 10, 10, 10);
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
					MainGrid.Margin = new Thickness(10, 50, 10, 50);
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
				MainGrid.Margin = new Thickness(10, 50, 10, 50);
				MainGrid.Margin = new Thickness(10, 50, 10, 50);
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
			((ComboBox)sender).FontSize = (e.NewSize.Height) * 0.7;
			//combo.Height = e.NewSize.Height;
			//combo.Width = e.NewSize.Width;
			//combo.FontSize = e.NewSize.Height * 0.7;
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			//MainGrid.Visibility = Visibility.Collapsed;
			this.Frame.Navigate(typeof(BlankPage1));
		}

		private void Button_GotFocus(object sender, RoutedEventArgs e)
		{
			
		}

		private void sizeKeyboard_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			keyboard.ResizeButton(e.NewSize.Height, e.NewSize.Width, int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString()) + 2);
		}

		private void combo_Holding(object sender, HoldingRoutedEventArgs e)
		{
			FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender); 
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			var a = (Button)Buttons.Children[Buttons.Children.Count - 1];
			var temp = new Button()
			{
				Content = "4",
				Visibility = Visibility.Visible,
				IsTabStop = false,
				MinWidth = 0,
				MinHeight = 0,
				MaxHeight = 1000,
				VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch,
				HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch,
				Padding = new Thickness(0, 0, 0, 0),
				VerticalContentAlignment = Windows.UI.Xaml.VerticalAlignment.Center,
				Margin = new Thickness(0, -12, 0, -6)
			};
			temp.Height = a.ActualHeight;
			temp.Width = a.ActualWidth;
			Buttons.Children.Add(temp);
			Buttons.UpdateLayout();
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
			AddComboBoxItem(comboItem, parentFlyout, true);
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
		
	}
}
