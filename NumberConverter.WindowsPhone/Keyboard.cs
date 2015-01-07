using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace NumberConverter
{
	public partial class Keyboard
	{
		#region variable and const

		private static Color col = new Color();
		private List<Button> buttonsList = new List<Button>(); 
		private Panel panel;
		private int visibleCount;

		public int MaxButtonHeight
		{
			get { return maxButtonHeight; }
			set
			{
				maxButtonHeight = value;
				for (int i = 0; i < buttonsList.Count; i++)
					buttonsList[i].MaxHeight = value;

			}
		}

		public int MaxButtonWidth
		{
			get { return maxButtonWidth; }
			set
			{
				maxButtonWidth = value;
				for (int i = 0; i < buttonsList.Count; i++)
					buttonsList[i].MaxWidth = value;
			}
		}

		public bool ButtonsAreSame
		{
			get { return buttonsAreSame; }
			set
			{
				buttonsAreSame = value;
				ResizeButton();
			}
		}

		//public int VisibleButtonCount {
		//	get { return visibleCount; }
		//	set
		//	{
		//		if (value >= 0)
		//		{
		//			visibleCount = value + 3;
		//			SetVisibleButton(visibleCount, true);
		//		}
		//	}}


		public int VisibleButtonCount
		{
			get { return (int)GetValue(VisibleButtonCountProperty); }
			set
			{
				if (value < 0 || value > buttonsList.Count - 3)
					return;
				SetValue(VisibleButtonCountProperty, value);
				SetVisibleButton(value , true);
			}
		}

		public static readonly DependencyProperty VisibleButtonCountProperty =
		  DependencyProperty.Register("VisibleButtonCount", typeof(int), typeof(Keyboard),
		  new PropertyMetadata(null));

		private int marg_bot = 0;
		private int marg_top = 0;
		private int maxButtonHeight;
		private int maxButtonWidth;
		private bool buttonsAreSame;

		public const string letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		#endregion

		#region Constructors

		public Keyboard(Panel panel, Style style)
		{
			MaxButtonHeight = 1000;
			MaxButtonWidth = 1000;
			this.panel = panel;
			CreateKeyboard(this.panel, style);
		}

		Button newButton(string text, Style style)
		{
			return new Button()
			{
				Content = text,
				Visibility = Visibility.Visible,
				IsTabStop = false,
				MinWidth = 0,
				MinHeight = 0,
				MaxHeight = MaxButtonHeight,
				MaxWidth = MaxButtonWidth,
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalContentAlignment = VerticalAlignment.Center,
				Style = style
				//Margin = new Thickness(0, marg_top, 0, marg_bot)
			};
		}


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
				temp = newButton(letters[i].ToString(), style);
				temp.Style = style;
				temp.Click += TempOnClick;
				panel.Children.Add(temp);
				buttonsList.Add(temp);
			}
			temp = newButton(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator,style);
			temp.Style = style;
			temp.Click += Button_Click_Dot;
			panel.Children.Add(temp);
			buttonsList.Add(temp);

			temp = newButton("<-",style);
			temp.Style = style;
			temp.Click += BackspaceOnClick;
			panel.Children.Add(temp);
			buttonsList.Add(temp);

			temp = newButton("CE",style);
			temp.Style = style;
			temp.Click += CleanOnClick;
			panel.Children.Add(temp);
			buttonsList.Add(temp);
			visibleCount = panel.Children.Count - 3;
		}

		private void BackspaceOnClick(object sender, RoutedEventArgs e)
		{
			OnOnBackspaceClick(new ButtonClickArgs() {Button = (Button) sender});
		}

		private void CleanOnClick(object sender, RoutedEventArgs e)
		{
			OnOnCleanClick(new ButtonClickArgs() {Button = (Button) sender});
		}

		private void Button_Click_Dot(object sender, RoutedEventArgs e)
		{
			OnOnDotClick(new ButtonClickArgs() {Button = (Button) sender});
		}

		private void TempOnClick(object sender, RoutedEventArgs routedEventArgs)
		{
			OnOnButtonClick(new ButtonClickArgs() {Button = (Button) sender});
		}

		#endregion

		#region Functions

		// Resize for current height and width, using count current visible elements
		public void ResizeButton()
		{
			ResizeButton(visibleCount);
		}

		// Resize for current height and width
		public void ResizeButton(int countKeys)
		{
			if (ActualHeight == 0 || ActualWidth == 0)
				return;
			if (ButtonsAreSame)
				countKeys = panel.Children.Count;
			ResizeButton(ActualHeight, ActualWidth, countKeys);
		}

		
		public void ResizeButton(double height, double width, int countKeys)
		{
			if (countKeys == 0)
				return;
			double maxHeight = height - 5;
			double maxWidth = width - 5;
			double areaForKeyboard = maxHeight*maxWidth;

			double a = Math.Sqrt(areaForKeyboard / countKeys); //average lenght of size
			int row = 0;
			int column = 0;
			double h = a;
			double w = a;

			//approximately count rows and coumns
			while (row * column < countKeys)
			{
				a -= 5;
				row = (int) (maxHeight/a);
				column = (int) (maxWidth/a);
			}

			h = a;
			while (row*column - countKeys - 1 >= row - 1)
				column--;
			w = maxWidth/column;
#if DEBUG
			double x = Math.Round(3.8, MidpointRounding.ToEven);
			double x1 = Math.Round(3.8, MidpointRounding.AwayFromZero);
			double x2 = Math.Floor(3.8);
#endif
			//row = (int)((double)(countKeys / column));
			if ((countKeys/column) - row > 0)
				row++;
			//if ((w) * row <= maxHeight) // marg_bot + marg_top - correction for buttons in WP8.1
			//	h = w;
			//else
#if WINDOWS_PHONE_APP
			maxHeight += row*2;
#endif
			h = (maxHeight)/row;

			if (MaxButtonHeight < h)
				h = MaxButtonHeight;
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
				var button = ((Button)panel.Children[i]);
				button.Height = h + correction;
				button.Width = w + correctionX;
				//((Button)panel.Children[i]).UpdateLayout();
				button.FontSize = h*0.5;
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
				ResizeButton(visibleCount+3);
		}

		#endregion

		#region events


		protected void OnOnButtonClick(ButtonClickArgs args)
		{
			ButtonClick handler = OnButtonClick;
			if (handler != null) handler(this, args);
		}

		protected void OnOnBackspaceClick(ButtonClickArgs args)
		{
			BackspaceClick handler = OnBackspaceClick;
			if (handler != null) handler(this, args);
		}

		protected void OnOnCleanClick(ButtonClickArgs args)
		{
			CleanClick handler = OnCleanClick;
			if (handler != null) handler(this, args);
		}


		public void OnOnDotClick(ButtonClickArgs args)
		{
			DotClick handler = OnDotClick;
			if (handler != null) handler(this, args);
		}

		public delegate void ButtonClick(object sender, ButtonClickArgs args);
		public delegate void BackspaceClick(object sender, ButtonClickArgs args);
		public delegate void CleanClick(object sender, ButtonClickArgs args);
		public delegate void DotClick(object sender, ButtonClickArgs args);

		public event ButtonClick OnButtonClick;
		public event BackspaceClick OnBackspaceClick;
		public event CleanClick OnCleanClick;
		public event DotClick OnDotClick;
		#endregion
	}

	public class ButtonClickArgs : EventArgs
	{
		public Button Button { get; set; }
	}
}
