using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NumberConverter.WinRTXamlToolkit.Controls;

namespace NumberConverter
{
	class Keyboard
	{
		static Color col = new Color();
		Panel panel;
		int visibleCount;

		int marg_bot = 0;
		int marg_top = 0;

		public Keyboard(Panel panel, Style style)
		{
			this.panel = panel;
			CreateKeyboard(this.panel, style);
		}

		Button newButton(string text)
		{
			return new Button()
			{
				Content = text,
				Visibility = Visibility.Visible,
				IsTabStop = false,
				MinWidth = 0,
				MinHeight = 0,
				MaxHeight = 1000,
				VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch,
				HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch,
				VerticalContentAlignment = Windows.UI.Xaml.VerticalAlignment.Center
				//Margin = new Thickness(0, marg_top, 0, marg_bot)
			};
		}

		public const  string letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		public void CreateKeyboard(Panel panel, Style style)
		{
			
			Button temp;

#if WINDOWS_PHONE_APP
			marg_bot = -6;
			marg_top = -15;
#endif
			//col = Color.FromArgb(0xff, 0x3f, 0x3f, 0x3f);

			for (int i = 0; i < 36; i++)
			{
				temp = newButton(letters[i].ToString());
				temp.Style = style;
				//temp.Click += Button_Click_1;
				panel.Children.Add(temp);
			}
			temp = newButton(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
			temp.Style = style;
			//temp.Click += Button_Click_Dot;
			//temp.SizeChanged += Buttons_SizeChanged;
			panel.Children.Add(temp);
			temp = newButton("<-");
			temp.Style = style;
			panel.Children.Add(temp);
			temp = newButton("CE");
			temp.Style = style;
			panel.Children.Add(temp);
			visibleCount = panel.Children.Count-3;
		}

		// Resize for current height and width, using count current visible elements
		public void ResizeButton()
		{
			ResizeButton(visibleCount);
		}

		// Resize for current height and width
		public void ResizeButton(int countKeys)
		{
			ResizeButton(panel.ActualHeight, panel.ActualWidth, countKeys);
		}

		
		public void ResizeButton(double height, double width, int countKeys)
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
			while (row * column - countKeys - 1 >= row - 1)
				column--;
			w = maxWidth / column;
#if DEBUG
			double x = Math.Round(3.8,MidpointRounding.ToEven);
			double x1 = Math.Round(3.8, MidpointRounding.AwayFromZero);
			double x2 = Math.Floor(3.8);
#endif
			//row = (int)((double)(countKeys / column));
			if ((countKeys / column) - row > 0 )
				row++;
			//if ((w) * row <= maxHeight) // marg_bot + marg_top - correction for buttons in WP8.1
			//	h = w;
			//else
				h = (maxHeight) / row; 
			//while ((h) * row <= maxHeight + marg_top*row)
			//	h += 5;
				int correction = 0;
			int correctionX = 0;
#if WINDOWS_PHONE_APP
				correction +=15;
			correctionX = -2;
#endif
			for (int i = 0; i < panel.Children.Count; i++)
			{
				((Button)panel.Children[i]).Height = h + correction;
				((Button)panel.Children[i]).Width = w + correctionX;
				//((Button)panel.Children[i]).UpdateLayout();
				((Button)panel.Children[i]).FontSize = h * 0.5;
			}
			//	((WrapPanel)panel).ItemHeight = h;
			//	((WrapPanel)panel).ItemWidth = w;
			
		}

		public void HideAll()
		{
			for (int i = 0; i < panel.Children.Count; i++)
				panel.Children[i].Visibility = Visibility.Collapsed;
		}

		public void SetVisibleButton(int count, bool needResize = false)
		{
			if (count < 0)
				return;
			if (count != visibleCount)
			{
				var temp = panel.Children;
				for (int i = 0; i < temp.Count - 3 || i < visibleCount; i++)
					if (i < count)
					{
						temp[i].Visibility = Visibility.Visible;

					}
					else
					{
						temp[i].Visibility = Visibility.Collapsed;

					}
				visibleCount = count;
			}
			if (needResize)
				ResizeButton(visibleCount + 3);
		}
	}

}
