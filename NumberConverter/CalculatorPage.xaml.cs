using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
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
	public sealed partial class BlankPage1 : Page
	{
		private int op = 0;

		private Keyboard keyboard;
		private int fromBase;
		private int fromBase2;
		private int toBase;
		private int fromBaseIndex, fromBase2Index, toBaseIndex;
		private static SuspendPage suspendPage;
		//string BAse;
		public int FromBase
		{
			get { return fromBaseIndex; }
			set
			{
				fromBaseIndex = value;
				if (From.Items != null) 
					fromBase = int.Parse(((ComboBoxItem) From.Items[value]).Content.ToString());
			}
		}

		public int FromBase2
		{
			get { return fromBase2Index; }
			set
			{
				fromBase2Index = value;
				if (From2.Items != null) fromBase2 = int.Parse(((ComboBoxItem) From2.Items[value]).Content.ToString());
			}
		}

		public int ToBase
		{
			get { return toBaseIndex; }
			set
			{
				toBaseIndex = value;
				if (To.Items != null) toBase = int.Parse(((ComboBoxItem) To.Items[value]).Content.ToString());
			}
		}

		private ComboBox parentFlyout;
		private Dictionary<FrameworkElement, FrameworkElement> TextBoxToComboBox;
		private double scaleFontTextBox = 2.3;
		private TextBox LastFocusTextBox;

		public BlankPage1()
		{
			this.InitializeComponent();
			CreateKeyboard(Buttons);
			FromBase = 0;
			FromBase2 = 0;
			ToBase = 0;
			Buttons.UpdateLayout();
			keyboard.SetVisibleButton(fromBase);
			TextBoxToComboBox = new Dictionary<FrameworkElement, FrameworkElement>();
			TextBoxToComboBox.Add(InputText, From);
			TextBoxToComboBox.Add(InputText2, From2);
			TextBoxToComboBox.Add(Result, To);
			DataContext = this;
			LastFocusTextBox = InputText;

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
			keyboard = new Keyboard(panel, Application.Current.Resources["ButtonStyle1"] as Style);
			for (int i = 0; i < panel.Children.Count - 3; i++)
			{
				((Button) (panel.Children[i])).Click += Button_Click_1;
			}
			((Button) (panel.Children[panel.Children.Count - 3])).Click += Button_Click_Dot; // "."
			((Button) (panel.Children[panel.Children.Count - 2])).Click += Backspace_Click; // backspace
			//((Button)(panel.Children[panel.Children.Count - 2])).SizeChanged += Buttons_SizeChanged_1;
			((Button)(panel.Children[panel.Children.Count - 1])).Click += Button_Click_Clean; //Clean
		}

		private void Button_Click_Clean(object sender, RoutedEventArgs e)
		{
			if (!(FocusManager.GetFocusedElement() is TextBox))
				return;
			var input = (TextBox)(FocusManager.GetFocusedElement());
			if (input == Result)
				return;
			input.Text = "";
		}


		private void Backspace_Click(object sender, RoutedEventArgs e)
		{
			if (!(FocusManager.GetFocusedElement() is TextBox))
				return;
			var input = (TextBox) (FocusManager.GetFocusedElement());
			if (input == Result)
				return;
			SharePages.Backspace(input);
			input.Select(input.Text.Length, 0);
		}

		private void Button_Click_Dot(object sender, RoutedEventArgs e)
		{
			if (!(FocusManager.GetFocusedElement() is TextBox))
				return;
			var input = (TextBox) (FocusManager.GetFocusedElement());
			if (input == Result)
				return;
			if (input.Text.IndexOf(((Button) sender).Content.ToString()) < 0) //точки нету
				Button_Click_1(sender, e);
		}

		private void ResultText(string text)
		{
			Result.Text = text;
		}

		private string Operation(string In, string Out)
		{
			var slag = Converter.Converter.ConvertTo((uint) fromBase, new LongDouble(In), 10);
			//double firstslagD = double.Parse(slag);
			var slag2 = Converter.Converter.ConvertTo((uint) fromBase2, new LongDouble(Out), 10);
			//double secondslagD = double.Parse(slag);
			switch (op)
			{
				case 0:
					slag = (slag + slag2);
					break;
				case 1:
					slag = (slag - slag2);
					break;
				case 2:
					slag = (slag*slag2);
					break;
				case 3:
				{
					if (slag2.Integer == "0" && slag2.Fraction == "0")
					{
						var resourceLoader = new ResourceLoader();
						return resourceLoader.GetString("DivideByZero");
						//return "You can not divide by zero";
					}
					slag = (slag/slag2);
					break;
				}
			}
			return Converter.Converter.ConvertTo(10, slag, (uint) toBase).ToString();
		}

		private void Calculate()
		{
			if (InputText.Text != "" && InputText2.Text != "")
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
			TextBox input;
			if (!(FocusManager.GetFocusedElement() is TextBox))
				input = LastFocusTextBox;
			else
			 input = (TextBox) (FocusManager.GetFocusedElement());
			if (input == Result)
				return;
			//var x = input.SelectionStart; //временное запоминание
			//input.Text = input.Text.Insert(input.SelectionStart, ((Button) sender).Content.ToString());
			//input.SelectionStart = x + ((Button) sender).Content.ToString().Length;
			//InputText.Text += ((Button)sender).Content.ToString();
			//input.Text += ((Button) sender).Content.ToString();
			//input.Focus(FocusState.Programmatic);
			SharePages.AddTextTextBox(((Button) sender).Content.ToString(), input);
		}



		private void From_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				if (From != null || From2 != null)
				{
					Calculate();
					keyboard.SetVisibleButton(int.Parse((((ComboBoxItem) ((ComboBox) sender).SelectedItem)).Content.ToString()), true);
				}
			}
			catch (Exception ee)
			{
				Result.Text = ee.Message;
			}
		}



		private void TextBox_SizeChanged(object sender, SizeChangedEventArgs e)
		{

			var x = ((TextBox) sender);
			//var statView = ApplicationView.GetForCurrentView();
			//if (statView.IsFullScreen && statView.Orientation == ApplicationViewOrientation.Landscape)
			//{
			//}
			//else
			//{
			//	x.TextWrapping = TextWrapping.NoWrap;
			//	x.FontSize = x.ActualHeight*0.8;
			//}
				x.TextWrapping = TextWrapping.Wrap;
				x.FontSize = x.ActualHeight*scaleFontTextBox;
		}

		private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var x = ((ButtonBase) sender);
			x.FontSize = (x.ActualHeight - 15)*0.5;
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
			((ComboBox) sender).FontSize = e.NewSize.Height*0.7;

		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			//MainGrid.Visibility = Visibility.Collapsed;
			//this.Frame.Navigate(typeof(MainPage));
			if (Frame.CanGoBack)
			{
				suspendPage = new SuspendPage();
				suspendPage.indexFrom = FromBase;
				suspendPage.indexFrom2 = FromBase2;
				suspendPage.indexTo = ToBase;
				suspendPage.InputText = InputText.Text;
				suspendPage.InputText2 = InputText2.Text;
				this.Frame.GoBack();
			}
		}


		private void Button_Plus_Checked(object sender, RoutedEventArgs e)
		{
			op = 0;
			if (Button_Plus != null)
				Button_Plus.IsChecked = true;
			else
				return;
			if (Button_Minus != null)
				Button_Minus.IsChecked = false;
			if (Button_multipl != null)
				Button_multipl.IsChecked = false;
			if (Button_divide != null)
				Button_divide.IsChecked = false;
			Calculate();
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
			Calculate();
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
			Calculate();
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
			Calculate();
		}



		private void InputText_GotFocus(object sender, RoutedEventArgs e)
		{
			//Buttons.Visibility = Windows.UI.Xaml.Visibility.Visible;
			//int count = int.Parse(((ComboBoxItem)From2.SelectedItem).Content.ToString());
			LastFocusTextBox = (TextBox) sender;
			var selectedItem = ((ComboBoxItem) ((ComboBox) TextBoxToComboBox.First(a => a.Key == sender).Value).SelectedItem);
			int _base = int.Parse(selectedItem.Content.ToString());
			keyboard.SetVisibleButton(_base, false);
			keyboard.ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, _base + 3);
		}

		private void InputText2_GotFocus(object sender, RoutedEventArgs e)
		{
			//Buttons.Visibility = Windows.UI.Xaml.Visibility.Visible;
			//int count = int.Parse(((ComboBoxItem)From2.SelectedItem).Content.ToString());
			var selectedItem = ((ComboBoxItem) ((ComboBox) TextBoxToComboBox.First(a => a.Key == sender).Value).SelectedItem);
			int _base = int.Parse(selectedItem.Content.ToString());
			keyboard.SetVisibleButton(_base, false);
			keyboard.ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, _base + 3);
		}

		private void Result_GotFocus(object sender, RoutedEventArgs e)
		{
			//Buttons.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
		}

		private void From_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if(e.NewSize.Height > 0)
			((ComboBox) sender).FontSize = (e.NewSize.Height)*0.7;
		}

		private void From_Holding(object sender, RoutedEventArgs e)
		{
			FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);
			parentFlyout = (ComboBox) sender;
		}

		private void sizeKeyboard_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			keyboard.ResizeButton(e.NewSize.Height, e.NewSize.Width, fromBase + 3);
		}

		private void To_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				if (From != null || To != null)
				{
					//	toBase = int.Parse(((ComboBoxItem)To.Items[toBase]).Content.ToString());
					//	Result.Text = Converter.Converter.ConvertTo((uint)fromBase,
					//		InputText.Text, (uint)toBase);
					Calculate();
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

		//void AddComboBoxItem(ComboBoxItem item, ComboBox combobox, bool isSelet)
		//{
		//	if (!combobox.Items.Any((a) =>
		//	{
		//		if (((ComboBoxItem)a).Content.ToString() == item.Content.ToString())
		//			return true;
		//		return false;
		//	}
		//	))
		//	{
		//		combobox.Items.Add(item);
		//		combobox.SelectedItem = combobox.Items.Last();
		//	}
		//	else
		//	{
		//		combobox.SelectedIndex = combobox.Items.IndexOf(combobox.Items.Single((a) => ((ComboBoxItem)a).Content.ToString() == item.Content.ToString()));
		//	}
		//}

		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var listbox = ((ListBox) sender);
			var str = ((ListBoxItem) listbox.SelectedItem).Content;
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

		public Flyout openedFlyout { get; set; }

		private void CalculatorPage_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			//var statView = ApplicationView.GetForCurrentView();
			//if (statView.IsFullScreen)
			//	if (statView.Orientation == ApplicationViewOrientation.Landscape)
			//	{
			//		MainGrid.Margin = new Thickness(0, 0, 0, 0);
			//		Grid.SetColumn(Buttons_operation, 1);
			//		Grid.SetColumnSpan(Buttons_operation, 1);
			//	}
			//	else //full screen portrait
			//	{
			//		MainGrid.Margin = new Thickness(0, 0, 0, 0);
			//		Grid.SetColumn(Buttons_operation, 0);
			//		Grid.SetColumnSpan(Buttons_operation, 3);
			//	}
			//else
			//{
			//	MainGrid.Margin = new Thickness(0, 0, 0, 0);
			//}

			MenuGrid.ColumnDefinitions[0].Width = new GridLength(MainGrid.ColumnDefinitions[0].ActualWidth);

			var statView = ApplicationView.GetForCurrentView();
			if (statView.IsFullScreen)
				if (statView.Orientation == ApplicationViewOrientation.Landscape)  //FullScreen and Landscape
				{
					var marginComboBox = new Thickness(20, 40, 20, 60);
					var marginTextBox = new Thickness(20, 10, 20, 40);
					var marginKeyboard = new Thickness(20, 10, 20, 60);
					var marginOperat = new Thickness(20, 10, 20, 10);

					MainGrid.RowDefinitions[1].Height = new GridLength(2, GridUnitType.Star);
					MainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
					MainGrid.RowDefinitions[3].Height = new GridLength(2, GridUnitType.Star);
					MainGrid.RowDefinitions[4].Height = new GridLength(2, GridUnitType.Star);
					MainGrid.RowDefinitions[5].Height = GridLength.Auto;
					MainGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);

					InputTextGrid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
					//Grid.SetRowSpan(InputText, 1);
					//Grid.SetColumnSpan(InputText, 1);
					InputText.Margin = marginTextBox;

					From.Margin = marginComboBox;

					InputText2Grid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
					//Grid.SetRowSpan(InputText, 1);
					//Grid.SetColumnSpan(InputText, 1);
					InputText2.Margin = marginTextBox;

					From2.Margin = marginComboBox;

					ResultGrid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
					//Grid.SetRow(Result, 3);
					//Grid.SetRowSpan(Result, 2);
					//Grid.SetColumnSpan(Result, 2);
					Result.Margin = marginTextBox;

					//Grid.SetRow(To, 3);
					//Grid.SetRowSpan(To, 1);
					To.Margin = marginComboBox;

					Buttons_operation.Margin = marginOperat;

					//Grid.SetRow(sizeKeyboard, 1);
					//Grid.SetRowSpan(sizeKeyboard, 5);
					//Grid.SetColumn(sizeKeyboard, 3);
					//Grid.SetColumnSpan(sizeKeyboard, 2);
					Grid.SetRow(sizeKeyboard, 1);
					Grid.SetColumn(sizeKeyboard, 3);
					sizeKeyboard.Margin = marginKeyboard;

					Grid.SetColumn(MenuGrid, 1);
					

					//InputText.Margin = new Thickness(10, 10, 10, 60);
					//From.Margin = new Thickness(10, 40, 10, 60);
					//Result.Margin = new Thickness(10, 10, 10, 60);
					//To.Margin = new Thickness(10, 40, 10, 60);
					//sizeKeyboard.Margin = new Thickness(10, 10, 10, 60);
					//MainGrid.ColumnDefinitions[0].Width = new GridLength(0.5, GridUnitType.Star);
					scaleFontTextBox = 0.35;
					//		//MainGrid.Margin = new Thickness(10, 10, 10, 10);
					//		Grid.SetColumnSpan(From, 1);
					//		Grid.SetRow(InputText, 1);
					//		Grid.SetColumn(InputText, 1);
					//		Grid.SetRow(To, 2);
					//		Grid.SetRowSpan(To, 1);
					//		Grid.SetColumn(To, 0);
					//		Grid.SetColumnSpan(To, 1);
					//		//To.Margin = new Thickness(0, 0, 0, 0);
					//		//To.Height = double.NaN;
					//		//To.Width = double.NaN;
					//		Grid.SetRow(Result, 2);
					//		Grid.SetColumn(Result, 1);
					//		Grid.SetRow(sizeKeyboard, 3);
					//		Grid.SetRowSpan(sizeKeyboard, 4);
					//		Grid.SetRow((FrameworkElement)Buttons, 3);
					//		Grid.SetRowSpan((FrameworkElement)Buttons, 4);
				}
				else     //FullScreen and Portrate
				{
					var marginComboBox = new Thickness(5, 40, 5, 60);
					var marginTextBox = new Thickness(5, 10, 5, 10);
					var marginKeyboard = new Thickness(5, 10, 5, 10);
					var marginOperat = new Thickness(5, 10, 5, 10);

					MainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
					MainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
					MainGrid.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);
					MainGrid.RowDefinitions[4].Height = new GridLength(1, GridUnitType.Star);
					MainGrid.RowDefinitions[5].Height = new GridLength(2, GridUnitType.Star);
					MainGrid.ColumnDefinitions[2].Width = GridLength.Auto;

					InputTextGrid.RowDefinitions[1].Height = GridLength.Auto;
					//Grid.SetRowSpan(InputText, 1);
					//Grid.SetColumnSpan(InputText, 1);
					InputText.Margin = marginTextBox;

					From.Margin = marginComboBox;

					InputText2Grid.RowDefinitions[1].Height = GridLength.Auto;
					//Grid.SetRowSpan(InputText, 1);
					//Grid.SetColumnSpan(InputText, 1);
					InputText2.Margin = marginTextBox;

					From2.Margin = marginComboBox;

					ResultGrid.RowDefinitions[1].Height = GridLength.Auto;
					//Grid.SetRow(Result, 3);
					//Grid.SetRowSpan(Result, 2);
					//Grid.SetColumnSpan(Result, 2);
					Result.Margin = marginTextBox;

					//Grid.SetRow(To, 3);
					//Grid.SetRowSpan(To, 1);
					To.Margin = marginComboBox;

					Buttons_operation.Margin = marginOperat;

					//Grid.SetRow(sizeKeyboard, 1);
					//Grid.SetRowSpan(sizeKeyboard, 5);
					//Grid.SetColumn(sizeKeyboard, 3);
					//Grid.SetColumnSpan(sizeKeyboard, 2);
					Grid.SetRow(sizeKeyboard, 5);
					Grid.SetColumn(sizeKeyboard, 0);
					sizeKeyboard.Margin = marginKeyboard;

					Grid.SetColumn(MenuGrid, 0);

					//InputText.Margin = new Thickness(10, 10, 10, 60);
					//From.Margin = new Thickness(10, 40, 10, 60);
					//Result.Margin = new Thickness(10, 10, 10, 60);
					//To.Margin = new Thickness(10, 40, 10, 60);
					//sizeKeyboard.Margin = new Thickness(10, 10, 10, 60);
					//MainGrid.ColumnDefinitions[0].Width = new GridLength(0.5, GridUnitType.Star);
					//scaleFontTextBox = 0.25;
					MainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
					MenuGrid.ColumnDefinitions[0].Width = GridLength.Auto;
					scaleFontTextBox = 0.25;
				}
			else  // not full screen
			{
				if ((this.ActualWidth <= 510 && ActualWidth >= 490) || (statView.Orientation == ApplicationViewOrientation.Portrait && statView.IsFullScreen))
				{
					var marginComboBox = new Thickness(5, 40, 5, 60);
					var marginTextBox = new Thickness(5, 10, 5, 10);
					var marginKeyboard = new Thickness(5, 10, 5, 10);
					var marginOperat = new Thickness(5, 10, 5, 10);

					MainGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
					MainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
					MainGrid.RowDefinitions[3].Height = new GridLength(1, GridUnitType.Star);
					MainGrid.RowDefinitions[4].Height = new GridLength(1, GridUnitType.Star);
					MainGrid.RowDefinitions[5].Height = new GridLength(2, GridUnitType.Star);
					MainGrid.ColumnDefinitions[2].Width = GridLength.Auto;

					InputTextGrid.RowDefinitions[1].Height = GridLength.Auto;
					//Grid.SetRowSpan(InputText, 1);
					//Grid.SetColumnSpan(InputText, 1);
					InputText.Margin = marginTextBox;

					From.Margin = marginComboBox;

					InputText2Grid.RowDefinitions[1].Height = GridLength.Auto;
					//Grid.SetRowSpan(InputText, 1);
					//Grid.SetColumnSpan(InputText, 1);
					InputText2.Margin = marginTextBox;

					From2.Margin = marginComboBox;

					ResultGrid.RowDefinitions[1].Height = GridLength.Auto;
					//Grid.SetRow(Result, 3);
					//Grid.SetRowSpan(Result, 2);
					//Grid.SetColumnSpan(Result, 2);
					Result.Margin = marginTextBox;

					//Grid.SetRow(To, 3);
					//Grid.SetRowSpan(To, 1);
					To.Margin = marginComboBox;

					Buttons_operation.Margin = marginOperat;

					//Grid.SetRow(sizeKeyboard, 1);
					//Grid.SetRowSpan(sizeKeyboard, 5);
					//Grid.SetColumn(sizeKeyboard, 3);
					//Grid.SetColumnSpan(sizeKeyboard, 2);
					Grid.SetRow(sizeKeyboard, 5);
					Grid.SetColumn(sizeKeyboard, 0);
					sizeKeyboard.Margin = marginKeyboard;

					Grid.SetColumn(MenuGrid, 0);

					//InputText.Margin = new Thickness(10, 10, 10, 60);
					//From.Margin = new Thickness(10, 40, 10, 60);
					//Result.Margin = new Thickness(10, 10, 10, 60);
					//To.Margin = new Thickness(10, 40, 10, 60);
					//sizeKeyboard.Margin = new Thickness(10, 10, 10, 60);
					//MainGrid.ColumnDefinitions[0].Width = new GridLength(0.5, GridUnitType.Star);
					//scaleFontTextBox = 0.25;
					MainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
					scaleFontTextBox = 0.25;
				}
				else
				{
					var marginComboBox = new Thickness(5, 10, 5, 60);
					var marginTextBox = new Thickness(5, 5, 5, 40);
					var marginKeyboard = new Thickness(5, 5, 5, 40);
					var marginOperat = new Thickness(5, 5, 5, 5);

					MainGrid.RowDefinitions[1].Height = new GridLength(2, GridUnitType.Star);
					MainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
					MainGrid.RowDefinitions[3].Height = new GridLength(2, GridUnitType.Star);
					MainGrid.RowDefinitions[4].Height = new GridLength(2, GridUnitType.Star);
					MainGrid.RowDefinitions[5].Height = GridLength.Auto;
					MainGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);

					InputTextGrid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
					//Grid.SetRowSpan(InputText, 1);
					//Grid.SetColumnSpan(InputText, 1);
					InputText.Margin = marginTextBox;

					From.Margin = marginComboBox;

					InputText2Grid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
					//Grid.SetRowSpan(InputText, 1);
					//Grid.SetColumnSpan(InputText, 1);
					InputText2.Margin = marginTextBox;

					From2.Margin = marginComboBox;

					ResultGrid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
					//Grid.SetRow(Result, 3);
					//Grid.SetRowSpan(Result, 2);
					//Grid.SetColumnSpan(Result, 2);
					Result.Margin = marginTextBox;

					//Grid.SetRow(To, 3);
					//Grid.SetRowSpan(To, 1);
					To.Margin = marginComboBox;

					Buttons_operation.Margin = marginOperat;

					//Grid.SetRow(sizeKeyboard, 1);
					//Grid.SetRowSpan(sizeKeyboard, 5);
					//Grid.SetColumn(sizeKeyboard, 3);
					//Grid.SetColumnSpan(sizeKeyboard, 2);
					Grid.SetRow(sizeKeyboard, 1);
					Grid.SetColumn(sizeKeyboard, 3);
					sizeKeyboard.Margin = marginKeyboard;

					Grid.SetColumn(MenuGrid, 1);

					//InputText.Margin = new Thickness(10, 10, 10, 60);
					//From.Margin = new Thickness(10, 40, 10, 60);
					//Result.Margin = new Thickness(10, 10, 10, 60);
					//To.Margin = new Thickness(10, 40, 10, 60);
					//sizeKeyboard.Margin = new Thickness(10, 10, 10, 60);
					//MainGrid.ColumnDefinitions[0].Width = new GridLength(0.5, GridUnitType.Star);
					scaleFontTextBox = 0.35;
				}


				////	MainGrid.Margin = new Thickness(10, 50, 10, 50);
				//	Grid.SetColumnSpan(From, 2);
				//	Grid.SetRow(InputText, 2);
				//	Grid.SetColumn(InputText, 0);
				//	Grid.SetRow(To, 1);
				//	Grid.SetColumn(To, 3);
				//	Grid.SetColumnSpan(To, 2);

				//	//To.Margin = new Thickness(0, 0, 0, 0);
				//	//To.Height = double.NaN;
				//	//To.Width = double.NaN;
				//	Grid.SetRow(Result, 3);
				//	Grid.SetColumn(Result, 0);
				//	Grid.SetRow(sizeKeyboard, 4);
				//	Grid.SetRowSpan(sizeKeyboard, 1);
				//	Grid.SetRow((FrameworkElement)Buttons, 4);
				//	Grid.SetRowSpan((FrameworkElement)Buttons, 1);
			}
			////VisualStateManager.GoToState(this, "FullScreenLandscape", true);
			
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
			if (Frame.CanGoBack)
			{
				suspendPage = new SuspendPage();
				suspendPage.indexFrom = FromBase;
				suspendPage.indexFrom2 = FromBase2;
				suspendPage.indexTo = ToBase;
				suspendPage.InputText = InputText.Text;
				suspendPage.InputText2 = InputText2.Text;
				this.Frame.GoBack();
			}
		}

		private void From_Holding(object sender, HoldingRoutedEventArgs e)
		{
			FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);
			parentFlyout = (ComboBox) sender;
		}

		private void Backspace()
		{
			var input = (TextBox) (FocusManager.GetFocusedElement());
			if (input.Text != "")
			{
				input.Text = input.Text.Remove(input.Text.Length - 1);
			}
		}

		private void InputText2_KeyUp(object sender, KeyRoutedEventArgs e)
		{
			var d = (ComboBox) TextBoxToComboBox.First((a) => a.Key == sender).Value;
			var i = int.Parse(((ComboBoxItem) d.SelectedItem).Content.ToString());
			SharePages.InputText_KeyUp(sender, e, i);

		}

		private void Button_Plus_Unchecked(object sender, RoutedEventArgs e)
		{
			if (!Button_Minus.IsChecked.Value &&
			    !Button_multipl.IsChecked.Value &&
			    !Button_divide.IsChecked.Value)
				Button_Plus.IsChecked = true;
		}

		private void Button_Minus_Unchecked(object sender, RoutedEventArgs e)
		{
			if (!Button_Plus.IsChecked.Value &&
			    !Button_multipl.IsChecked.Value &&
			    !Button_divide.IsChecked.Value)
				Button_Minus.IsChecked = true;
		}

		private void Button_multipl_Unchecked(object sender, RoutedEventArgs e)
		{
			if (!Button_Plus.IsChecked.Value &&
			    !Button_Minus.IsChecked.Value &&
			    !Button_divide.IsChecked.Value)
				Button_multipl.IsChecked = true;
		}

		private void Button_divide_Unchecked(object sender, RoutedEventArgs e)
		{
			if (!Button_Plus.IsChecked.Value &&
			    !Button_Minus.IsChecked.Value &&
			    !Button_multipl.IsChecked.Value)
				Button_divide.IsChecked = true;
		}

		private void Button_SizeChanged_1(object sender, SizeChangedEventArgs e)
		{
			var button = ((ButtonBase) sender);
			button.FontSize = e.NewSize.Height*0.5;
		}

		private void CalculatorPage_Loaded(object sender, RoutedEventArgs e)
		{
			if (suspendPage != null)
			{
				FromBase = suspendPage.indexFrom;
				FromBase2 = suspendPage.indexFrom2;
				ToBase = suspendPage.indexTo;
				if (From.Items != null && From2.Items != null && To.Items != null)
				{
					From.SelectedItem = From.Items[FromBase];
					From2.SelectedItem = From2.Items[FromBase2];
					To.SelectedItem = To.Items[ToBase];
				}
				
				InputText.Text = suspendPage.InputText;
				InputText2.Text = suspendPage.InputText2;
				Calculate();
				//keyboard.ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, fromBase + 2);
				suspendPage = null;
			}
			InputText.Focus(FocusState.Programmatic);
		}
	}
}

