using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
				fromBase = int.Parse(((ComboBoxItem) From.Items[value]).Content.ToString());
			}
		}

		public int FromBase2
		{
			get { return fromBase2Index; }
			set
			{
				fromBase2Index = value;
				fromBase2 = int.Parse(((ComboBoxItem) From2.Items[value]).Content.ToString());
			}
		}

		public int ToBase
		{
			get { return toBaseIndex; }
			set
			{
				toBaseIndex = value;
				toBase = int.Parse(((ComboBoxItem) To.Items[value]).Content.ToString());
			}
		}

		private ComboBox parentFlyout;
		private Dictionary<FrameworkElement, FrameworkElement> TextBoxToComboBox;

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
				((Button) (panel.Children[i])).Click += Button_Click_1;
			}
			((Button) (panel.Children[panel.Children.Count - 2])).Click += Button_Click_Dot; // "."
			((Button) (panel.Children[panel.Children.Count - 1])).Click += Backspace_Click; // backspace
			//((Button)(panel.Children[panel.Children.Count - 2])).SizeChanged += Buttons_SizeChanged_1;
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
						return "Нou can not divide by zero";
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
			if (!(FocusManager.GetFocusedElement() is TextBox))
				return;
			var input = (TextBox) (FocusManager.GetFocusedElement());
			if (input == Result)
				return;
			var x = input.SelectionStart; //временное запоминание
			input.Text = input.Text.Insert(input.SelectionStart, ((Button) sender).Content.ToString());
			input.SelectionStart = x + ((Button) sender).Content.ToString().Length;
			//InputText.Text += ((Button)sender).Content.ToString();
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
			var statView = ApplicationView.GetForCurrentView();
			if (statView.IsFullScreen && statView.Orientation == ApplicationViewOrientation.Landscape)
			{
				x.TextWrapping = TextWrapping.Wrap;
				x.FontSize = x.ActualHeight*0.4;
			}
			else
			{
				x.TextWrapping = TextWrapping.NoWrap;
				x.FontSize = x.ActualHeight*0.8;
			}
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
			var selectedItem = ((ComboBoxItem) ((ComboBox) TextBoxToComboBox.First(a => a.Key == sender).Value).SelectedItem);
			int _base = int.Parse(selectedItem.Content.ToString());
			keyboard.SetVisibleButton(_base, false);
			keyboard.ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, _base + 2);
		}

		private void InputText2_GotFocus(object sender, RoutedEventArgs e)
		{
			//Buttons.Visibility = Windows.UI.Xaml.Visibility.Visible;
			//int count = int.Parse(((ComboBoxItem)From2.SelectedItem).Content.ToString());
			var selectedItem = ((ComboBoxItem) ((ComboBox) TextBoxToComboBox.First(a => a.Key == sender).Value).SelectedItem);
			int _base = int.Parse(selectedItem.Content.ToString());
			keyboard.SetVisibleButton(_base, false);
			keyboard.ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, _base + 2);
		}

		private void Result_GotFocus(object sender, RoutedEventArgs e)
		{
			//Buttons.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
		}

		private void From_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			((ComboBox) sender).FontSize = (e.NewSize.Height)*0.7;
		}

		private void From_Holding(object sender, RoutedEventArgs e)
		{
			FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);
			parentFlyout = (ComboBox) sender;
		}

		private void sizeKeyboard_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			keyboard.ResizeButton(e.NewSize.Height, e.NewSize.Width, fromBase + 2);
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
			var statView = ApplicationView.GetForCurrentView();
			if (statView.IsFullScreen)
				if (statView.Orientation == ApplicationViewOrientation.Landscape)
				{
					MainGrid.Margin = new Thickness(0, 0, 0, 0);
					Grid.SetColumn(Buttons_operation, 1);
					Grid.SetColumnSpan(Buttons_operation, 1);
				}
				else //full screen portrait
				{
					MainGrid.Margin = new Thickness(0, 0, 0, 0);
					Grid.SetColumn(Buttons_operation, 0);
					Grid.SetColumnSpan(Buttons_operation, 3);
				}
			else
			{
				MainGrid.Margin = new Thickness(0, 0, 0, 0);
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
			var button = ((Button) sender);
			button.FontSize = e.NewSize.Height*0.7;
		}

		private void CalculatorPage_Loaded(object sender, RoutedEventArgs e)
		{
#if DEBUG
			FromBase = 2;
			FromBase2 = 2;
			ToBase = 2;
			InputText.Text = "2.2";
			InputText2.Text = "2";
#endif
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
			}
			InputText.Focus(FocusState.Programmatic);
		}
	}
}

