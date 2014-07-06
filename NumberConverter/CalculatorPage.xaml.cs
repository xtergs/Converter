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
	public sealed partial class BlankPage1 : Page
	{
		int op = 0;

		Keyboard keyboard;
		int fromBase;
		int toBase;
		ComboBox parentFlyout;

		public BlankPage1()
		{
			this.InitializeComponent();
			CreateKeyboard(Buttons);
			fromBase = int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString());
			toBase = int.Parse(((ComboBoxItem)To.SelectedItem).Content.ToString());
			keyboard.SetVisibleButton(fromBase);
#if WINDOWS_PHONE_APP
			int marg_up = -6;
			Button_Plus.Margin = new Thickness(0, marg_up, 0, 0);
			Button_Minus.Margin = new Thickness(0, marg_up, 0, 0);
			Button_divide.Margin = new Thickness(0, marg_up, 0, 0);
			Button_multipl.Margin = new Thickness(0, marg_up, 0, 0);
#endif
		}

		public void CreateKeyboard(Panel panel)
		{
			keyboard = new Keyboard(panel);
			for (int i = 0; i < panel.Children.Count - 2; i++)
			{
				((Button)(panel.Children[i])).Click += Button_Click_1;
			}
			((Button)(panel.Children[panel.Children.Count - 2])).Click += Button_Click_Dot; // "."
			//((Button)(panel.Children[panel.Children.Count - 2])).SizeChanged += Buttons_SizeChanged_1;
		}

		private void Button_Click_Dot(object sender, RoutedEventArgs e)
		{
			InputText.Focus(Windows.UI.Xaml.FocusState.Pointer);
			if (InputText.Text.IndexOf(((Button)sender).Content.ToString()) < 0) //точки нету
				Button_Click_1(sender, e);
		}

		void ResultText(string text)
		{
			Result.Text = text;
		}

		string Operation(string In, string Out)
		{
			string slag = Converter.Converter.ConvertTo(uint.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString()),
					In, 10);
			double firstslagD = double.Parse(slag);
			slag = Converter.Converter.ConvertTo(uint.Parse(((ComboBoxItem)From2.SelectedItem).Content.ToString()),
					Out, 10);
			double secondslagD = double.Parse(slag);
			switch (op)
			{
				case 0: slag = (firstslagD + secondslagD).ToString(); break;
				case 1: slag = (firstslagD - secondslagD).ToString(); break;
				case 2: slag = (firstslagD * secondslagD).ToString(); break;
				case 3:
					{
						if (secondslagD == 0)
						{
							return "Нou can not divide by zero";
						}
						slag = (firstslagD / secondslagD).ToString();
						break;
					}
			}
			return Converter.Converter.ConvertTo(10, slag, uint.Parse(((ComboBoxItem)To.SelectedItem).Content.ToString()));
		}

		void Calculate()
		{
			Result.Text = Operation(InputText.Text, InputText2.Text);
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				if (From != null || To != null)
					Calculate();
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
		//	InputText.Focus(Windows.UI.Xaml.FocusState.Pointer);
			if (!(FocusManager.GetFocusedElement() is TextBox))
				return;
			var input = (TextBox)(FocusManager.GetFocusedElement());
			var x = input.SelectionStart; //временное запоминание
			input.Text = input.Text.Insert(input.SelectionStart, ((Button)sender).Content.ToString());
			input.SelectionStart = x + ((Button)sender).Content.ToString().Length;
			//InputText.Text += ((Button)sender).Content.ToString();
		}

		

		private void From_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				if (From != null || From2 != null)
				{
					Calculate();
					keyboard.SetVisibleButton(int.Parse((((ComboBoxItem)((ComboBox)sender).SelectedItem)).Content.ToString()), true);
				}
			}
			catch (Exception ee)
			{
				Result.Text = ee.Message;
			}
		}

		

		private void TextBox_SizeChanged(object sender, SizeChangedEventArgs e)
		{

			var x = ((TextBox)sender);
			var statView = ApplicationView.GetForCurrentView();
			if (statView.IsFullScreen && statView.Orientation == ApplicationViewOrientation.Landscape)
			{
				x.TextWrapping = TextWrapping.Wrap;
				x.FontSize = x.ActualHeight * 0.4;
			}
			else
			{
				x.TextWrapping = TextWrapping.NoWrap;
				x.FontSize = x.ActualHeight * 0.8;
			}
		}

		private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var x = ((ButtonBase)sender);
			x.FontSize = (x.ActualHeight-15) * 0.5;
		}

		

		private void Buttons_SizeChanged_1(object sender, SizeChangedEventArgs e)
		{

		//	double a = (e.NewSize.Height - 50) * (e.NewSize.Width - 50);
		//	int bases = int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString());
		//	a = Math.Sqrt(a / (bases + 2));


		//	var statView = ApplicationView.GetForCurrentView();
		//	if (statView.IsFullScreen && statView.Orientation == ApplicationViewOrientation.Portrait)
		//	{
		//		for (int i = 0; i < Buttons.Children.Count; i++)
		//		{
		//			((Button)Buttons.Children[i]).Height = InputText.ActualHeight * 0.5;
		//			((Button)Buttons.Children[i]).Width = InputText.ActualHeight * 0.5;
		//			((Button)Buttons.Children[i]).FontSize = InputText.ActualHeight * 0.5 * 0.7;
		//		}
		//	}
		//	else
		//	{
		//		for (int i = 0; i < Buttons.Children.Count; i++)
		//		{
		//			((Button)Buttons.Children[i]).Height = InputText.ActualHeight;
		//			((Button)Buttons.Children[i]).Width = InputText.ActualHeight;
		//			((Button)Buttons.Children[i]).FontSize = InputText.ActualHeight * 0.7;
		//		}
		//	}
		}



		private void ComboBox_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			((ComboBox)sender).FontSize = e.NewSize.Height * 0.7;
			
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			//MainGrid.Visibility = Visibility.Collapsed;
			this.Frame.Navigate(typeof(MainPage));
		}


		private void Button_Plus_Checked(object sender, RoutedEventArgs e)
		{
			op = 0;
			if (Button_Plus != null && Button_Plus.IsChecked == false)
				Button_Plus.IsChecked = true;
			if (Button_Minus != null)
				Button_Minus.IsChecked = false;
			if (Button_multipl != null)
				Button_multipl.IsChecked = false;
			if (Button_divide != null)
				Button_divide.IsChecked = false;
		}

		private void Button_Minus_Checked(object sender, RoutedEventArgs e)
		{
			op = 1;
			if (Button_Plus != null)
				Button_Plus.IsChecked = false;
			if (Button_multipl != null)
				Button_multipl.IsChecked = false;
			if (Button_divide != null)
				Button_divide.IsChecked = false;
		}

		private void Button_multipl_Checked(object sender, RoutedEventArgs e)
		{
			op = 2;
			if (Button_Plus != null)
				Button_Plus.IsChecked = false;
			if (Button_Minus != null)
				Button_Minus.IsChecked = false;
			if (Button_divide != null)
				Button_divide.IsChecked = false;
		}

		private void Button_divide_Checked(object sender, RoutedEventArgs e)
		{
			op = 3;
			if (Button_Plus != null)
				Button_Plus.IsChecked = false;
			if (Button_Minus != null)
				Button_Minus.IsChecked = false;
			if (Button_multipl != null)
				Button_multipl.IsChecked = false;
		}

		private void InputText_GotFocus(object sender, RoutedEventArgs e)
		{
			//Buttons.Visibility = Windows.UI.Xaml.Visibility.Visible;
			int count = int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString());
			keyboard.SetVisibleButton( count, false);
			keyboard.ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, count+2);
		}

		private void InputText2_GotFocus(object sender, RoutedEventArgs e)
		{
			//Buttons.Visibility = Windows.UI.Xaml.Visibility.Visible;
			int count = int.Parse(((ComboBoxItem)From2.SelectedItem).Content.ToString());
			keyboard.SetVisibleButton(count, false);
			keyboard.ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, count + 2);
		}

		private void Result_GotFocus(object sender, RoutedEventArgs e)
		{
			//Buttons.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
		}

		private void From_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			((ComboBox)sender).FontSize = (e.NewSize.Height) * 0.7;
		}

		private void From_Holding(object sender, RoutedEventArgs e)
		{
			FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
			parentFlyout = (ComboBox)sender;
		}

		private void sizeKeyboard_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			keyboard.ResizeButton(e.NewSize.Height, e.NewSize.Width, int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString()) + 2);
		}

		private void To_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				if (From != null || To != null)
				{
					toBase = int.Parse(((ComboBoxItem)To.SelectedItem).Content.ToString());
					Result.Text = Converter.Converter.ConvertTo((uint)fromBase,
						InputText.Text, (uint)toBase);
				}
			}
			catch (Exception ee)
			{
				Result.Text = ee.Message;
			}
		}

		private void Flyout_Opened(object sender, object e)
		{
			openedFlyout = sender as Flyout;
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
				combobox.SelectedIndex = combobox.Items.IndexOf(combobox.Items.Single((a) => ((ComboBoxItem)a).Content.ToString() == item.Content.ToString()));
			}
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

		public Flyout openedFlyout { get; set; }

		private void CalculatorPage_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var statView = ApplicationView.GetForCurrentView();
			if (statView.IsFullScreen)
				if (statView.Orientation == ApplicationViewOrientation.Landscape)
				{
					MainGrid.Margin = new Thickness(10, 10, 10, 10);
					Grid.SetColumn(Buttons_operation, 1);
					Grid.SetColumnSpan(Buttons_operation, 1);
				}
				else //full screen portrait
				{
					MainGrid.Margin = new Thickness(10, 10, 10, 10);
					Grid.SetColumn(Buttons_operation, 0);
					Grid.SetColumnSpan(Buttons_operation, 3);
				}
			else
			{
				MainGrid.Margin = new Thickness(10, 10, 10, 10);
			}
		}

		private void Buttons_operation_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			int correction = 0;
#if WINDOWS_PHONE_APP
			correction = 15;
#endif
			Button_divide.Height = e.NewSize.Height + correction;
			Button_Minus.Height = e.NewSize.Height + correction;
			Button_Plus.Height = e.NewSize.Height + correction;
			Button_multipl.Height = e.NewSize.Height + correction;
		}

		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			if (this.Frame.CanGoBack)
				this.Frame.GoBack();
		}
	}
}
