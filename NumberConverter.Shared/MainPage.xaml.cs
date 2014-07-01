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
			for (int i = 0; i < 36; i++)
			{
				temp = new Button()
				{
					Content = letters[i],
					Visibility = Visibility.Visible,
					IsTabStop = false,
					MinWidth = 0,
					MinHeight = 0,
					Margin = new Thickness(0, -12, 0, -6)
				};
				temp.Click += Button_Click_1;
				panel.Children.Add(temp);
			}
			temp = new Button()
			{
				Content = ".",
				Visibility = Visibility.Visible,
				IsTabStop = false,
				MinWidth = 0,
				MinHeight = 0,
				Margin = new Thickness(0, -12, 0, -6)
			};
			temp.Click += Button_Click_Dot;
			temp.SizeChanged += Buttons_SizeChanged;
			panel.Children.Add(temp);
			panel.Children.Add(new Button()
			{
				Content = "<-",
				Visibility = Visibility.Visible,
				IsTabStop = false,
				MinWidth = 0,
				MinHeight = 0,
				Margin = new Thickness(0, -12, 0, -6)
			});
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
					int fromBase = int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString());
					int toBase = int.Parse(((ComboBoxItem)To.SelectedItem).Content.ToString());
					Result.Text = Converter.Converter.ConvertTo((uint)fromBase,	InputText.Text, (uint)toBase);
					SetVisibleButton(int.Parse((((ComboBoxItem)((ComboBox)sender).SelectedItem)).Content.ToString()));
					ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, fromBase + 2);
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
				

		void ResizeButton(double height, double width, int countKeys)
		{
			double maxHeight = height - 10;
			double maxWidth = width - 10;
			double areaForKeyboard = maxHeight * maxWidth;

			double a = Math.Sqrt(areaForKeyboard / countKeys);
			int row = 0;
			int column = 0;
			double h = a;
			double w = a;
			while (row * column < countKeys)
			{
				a -= 5;
				row = (int)(maxHeight / a);
				column = (int)(maxWidth / a);
			}

			h = a;
			w = maxWidth / column;
			if (w * row <= maxHeight)
				h = w;

			for (int i = 0; i < Buttons.Children.Count; i++)
			{
				((Button)Buttons.Children[i]).Height = h;
				((Button)Buttons.Children[i]).Width = w;
				((Button)Buttons.Children[i]).FontSize = a * 0.5;
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

		}

		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var statView = ApplicationView.GetForCurrentView();
			if (statView.IsFullScreen)
				if (statView.Orientation == ApplicationViewOrientation.Landscape)
					MainGrid.Margin = new Thickness(100, 25, 100, 25);
				else
					MainGrid.Margin = new Thickness(10, 50, 10, 50);
			else
				MainGrid.Margin = new Thickness(10, 50, 10, 50);		
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

		private void sizeKeyboard_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			ResizeButton(e.NewSize.Height, e.NewSize.Width, int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString()) + 2);
		}
	}
}
