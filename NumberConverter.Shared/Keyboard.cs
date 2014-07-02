﻿using System;
using System.Collections.Generic;
using System.Text;
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
		Panel panel;
		int visibleCount;

		int marg_bot = 0;
		int marg_top = 0;

		public Keyboard(Panel panel)
		{
			this.panel = panel;
			CreateKeyboard(this.panel);
		}
		public void CreateKeyboard(Panel panel)
		{
			string letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			Button temp;

#if WINDOWS_PHONE_APP
			marg_bot = -6;
			marg_top = -15;
#endif
			for (int i = 0; i < 36; i++)
			{
				temp = new Button()
				{
					Content = letters[i],
					Visibility = Visibility.Visible,
					IsTabStop = false,
					MinWidth = 0,
					MinHeight = 0,
					MaxHeight = 1000,
					VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch,
					HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch,
					Padding = new Thickness(0,0,0,0),
					VerticalContentAlignment = Windows.UI.Xaml.VerticalAlignment.Center,
					Margin = new Thickness(0, marg_top, 0, marg_bot)
				};
				//temp.Click += Button_Click_1;
				panel.Children.Add(temp);
			}
			temp = new Button()
			{
				Content = ".",
				Visibility = Visibility.Visible,
				IsTabStop = false,
				MinWidth = 0,
				MinHeight = 0,
				VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch,
				HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch,
				Padding = new Thickness(0, 0, 0, 0),
				VerticalContentAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch,
				Margin = new Thickness(0, marg_top, 0, marg_bot)
			};
			//temp.Click += Button_Click_Dot;
			//temp.SizeChanged += Buttons_SizeChanged;
			panel.Children.Add(temp);
			panel.Children.Add(new Button()
			{
				Content = "<-",
				Visibility = Visibility.Visible,
				IsTabStop = false,
				MinWidth = 0,
				MinHeight = 0,
				VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch,
				HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch,
				Padding = new Thickness(0, 0, 0, 0),
				VerticalContentAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch,
				Margin = new Thickness(0, marg_top, 0, marg_bot)
			});
			visibleCount = panel.Children.Count-2;
		}
		public void ResizeButton(double height, double width, int countKeys)
		{
			double maxHeight = height - 10;
			double maxWidth = width - 5;
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

			for (int i = 0; i < panel.Children.Count; i++)
			{
#if WINDOWS_PHONE_APP
				h+=15;
#endif
				((Button)panel.Children[i]).Height = h;
				((Button)panel.Children[i]).Width = w;
				//((Button)panel.Children[i]).UpdateLayout();
				((Button)panel.Children[i]).FontSize = h * 0.5;
			}
			//	((WrapPanel)panel).ItemHeight = h;
			//	((WrapPanel)panel).ItemWidth = w;
			
		}

		public void SetVisibleButton(int count)
		{
			var temp = panel.Children;
			for (int i = 0; i < temp.Count - 2 || i < visibleCount; i++)
				if (i < count)
				{
					((Button)temp[i]).Visibility = Visibility.Visible;

				}
				else
				{
					((Button)temp[i]).Visibility = Visibility.Collapsed;

				}
		}
	}

}
