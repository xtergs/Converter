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
		public BlankPage1()
		{
			this.InitializeComponent();
			CreateKeyboard(Buttons);
			SetVisibleButton(int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString()));
		}

		public void CreateKeyboard(Panel panel)
		{
			string letters = "0123456789ABCDEF";
			Button temp;
			for (int i = 0; i < 16; i++)
			{
				temp = new Button() { Content = letters[i], Visibility = Visibility.Visible, IsTabStop = false };
				temp.Click += Button_Click_1;
				//temp.SizeChanged += Button_SizeChanged;
				panel.Children.Add(temp);
			}
			temp = new Button() { Content = ".", Visibility = Visibility.Visible, IsTabStop = false };
			temp.Click += Button_Click_Dot;
			//temp.SizeChanged += Button_SizeChanged;
			panel.Children.Add(temp);
			temp = new Button() { Content = "<-", Visibility = Visibility.Visible, IsTabStop = false };
			//temp.SizeChanged += Button_SizeChanged;
			panel.Children.Add(temp);
		}

		private void Button_Click_Dot(object sender, RoutedEventArgs e)
		{
			InputText.Focus(Windows.UI.Xaml.FocusState.Pointer);
			if (InputText.Text.IndexOf(((Button)sender).Content.ToString()) < 0) //точки нету
				Button_Click_1(sender, e);
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
				case 3: slag = (firstslagD / secondslagD).ToString();
					break;
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

		void SetVisibleButton(int count)
		{
			var temp = Buttons.Children;
			for (int i = 0; i < temp.Count - 2; i++)
				if (i < count)
				{
					((Button)temp[i]).Visibility = Visibility.Visible;

				}
				else
				{
					((Button)temp[i]).Visibility = Visibility.Collapsed;

				}
		}

		private void From_Holding(object sender, HoldingRoutedEventArgs e)
		{

		}

		private void From_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				if (From != null || To != null)
				{
					Calculate();
					SetVisibleButton(int.Parse((((ComboBoxItem)((ComboBox)sender).SelectedItem)).Content.ToString()));
					ResizeButton();
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
			if (ApplicationView.Value == ApplicationViewState.FullScreenPortrait)
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
			x.FontSize = x.ActualHeight * 0.8;
		}

		void ResizeButton()
		{
			double a = (Buttons.ActualHeight - 50) * (Buttons.ActualWidth - 50);
			int bases = int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString());
			a = Math.Sqrt(a / (bases + 2));

			for (int i = 0; i < Buttons.Children.Count; i++)
			{
				((Button)Buttons.Children[i]).Height = a;
				((Button)Buttons.Children[i]).Width = a;
			}

			if (ApplicationView.Value == ApplicationViewState.FullScreenPortrait)
			{

			}
			else
			{

			}
		}

		private void Buttons_SizeChanged_1(object sender, SizeChangedEventArgs e)
		{

			double a = (e.NewSize.Height - 50) * (e.NewSize.Width - 50);
			int bases = int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString());
			a = Math.Sqrt(a / (bases + 2));



			if (ApplicationView.Value == ApplicationViewState.FullScreenPortrait)
			{
				for (int i = 0; i < Buttons.Children.Count; i++)
				{
					((Button)Buttons.Children[i]).Height = InputText.ActualHeight * 0.5;
					((Button)Buttons.Children[i]).Width = InputText.ActualHeight * 0.5;
					((Button)Buttons.Children[i]).FontSize = InputText.ActualHeight * 0.5 * 0.7;
				}
			}
			else
			{
				for (int i = 0; i < Buttons.Children.Count; i++)
				{
					((Button)Buttons.Children[i]).Height = InputText.ActualHeight;
					((Button)Buttons.Children[i]).Width = InputText.ActualHeight;
					((Button)Buttons.Children[i]).FontSize = InputText.ActualHeight * 0.7;
				}
			}
		}

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			switch (ApplicationView.Value)
			{
				case ApplicationViewState.FullScreenLandscape:
					{
						MainGrid.Margin = new Thickness(100, 25, 100, 25);
					}
					break;
				case ApplicationViewState.FullScreenPortrait:
					{
						MainGrid.Margin = new Thickness(10, 50, 10, 50);
					}
					break;
				case ApplicationViewState.Snapped:
					{
						MainGrid.Margin = new Thickness(10, 50, 10, 50);
					}
					break;
			}
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
			SetVisibleButton(int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString()));
		}

		private void InputText2_GotFocus(object sender, RoutedEventArgs e)
		{
			SetVisibleButton(int.Parse(((ComboBoxItem)From2.SelectedItem).Content.ToString()));
		}

		private void Result_GotFocus(object sender, RoutedEventArgs e)
		{
			SetVisibleButton(-2);
		}
	}
}
