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
		public MainPage()
		{
			this.InitializeComponent();
			CreateKeyboard(Buttons);
			SetVisibleButton(int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString()));
		}

		public void CreateKeyboard(Panel panel)
		{
			string letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			Button temp;
			for (int i = 0; i < 16; i++)
			{
				temp = new Button() { Content = letters[i], Visibility = Visibility.Visible, IsTabStop = false };
				temp.Click += Button_Click_1;
				panel.Children.Add(temp);
			}
			temp = new Button() { Content = ".", Visibility = Visibility.Visible, IsTabStop = false };
			temp.Click += Button_Click_Dot;
			temp.SizeChanged += Buttons_SizeChanged;
			panel.Children.Add(temp);
			panel.Children.Add(new Button() { Content = "<-", Visibility = Visibility.Visible, IsTabStop = false });
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
					Result.Text = Converter.Converter.ConvertTo(uint.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString()),
					InputText.Text, uint.Parse(((ComboBoxItem)To.SelectedItem).Content.ToString()));
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

		private void InputText_PointerPressed(object sender, PointerRoutedEventArgs e)
		{

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
					Result.Text = Converter.Converter.ConvertTo(uint.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString()),
						InputText.Text, uint.Parse(((ComboBoxItem)To.SelectedItem).Content.ToString()));
					SetVisibleButton(int.Parse((((ComboBoxItem)((ComboBox)sender).SelectedItem)).Content.ToString()));
					ResizeButton();
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
					Result.Text = Converter.Converter.ConvertTo(uint.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString()),
						InputText.Text, uint.Parse(((ComboBoxItem)To.SelectedItem).Content.ToString()));
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
			//Button_SizeChanged(Clearr, e);
		}

		private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var x = ((Button)sender);
			//if (x.Height == double.NaN)
			x.FontSize = x.ActualHeight*0.8;
			//ResizeButton();
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
				((Button)Buttons.Children[i]).FontSize = a - 50;
			}

			if (ApplicationView.Value == ApplicationViewState.FullScreenPortrait)
			{
				
			}
			else
			{
				
			}
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

		private void From_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			((ComboBox)sender).FontSize = e.NewSize.Height * 0.7;
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			//MainGrid.Visibility = Visibility.Collapsed;
			this.Frame.Navigate(typeof(BlankPage1));
		}

		private void Button_GotFocus(object sender, RoutedEventArgs e)
		{
			
		}
	}
}
