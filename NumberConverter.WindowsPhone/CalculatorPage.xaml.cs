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
		private int op
		{
			get { return CalculatorController.Operation; }
			set { CalculatorController.Operation = value; }
		}

		//public int Operation {
		//	get { return CalculatorController.Operation; }
		//	set { op = value; } }

		private static SuspendPage suspendPage;
		private CalculatorController calculatorController;

		public CalculatorController CalculatorController
		{
			get
			{
				if (calculatorController == null)
					calculatorController = new CalculatorController();
				return calculatorController;
			}
			set { calculatorController = value; }
		}

		private TextBox lastTextBox;


		private ComboBox parentFlyout;
		//private Dictionary<FrameworkElement, FrameworkElement> TextBoxToComboBox;
		private double scaleFontTextBox = 3.5;

		private List<ToggleButton> operationList; 

		public BlankPage1()
		{
			this.InitializeComponent();

			DataContext = CalculatorController;

			sizeKeyboard.MaxButtonHeight = SettingsModelView.Settings.MaxSizeButton;
			sizeKeyboard.MaxButtonWidth = SettingsModelView.Settings.MaxSizeButton;
			sizeKeyboard.ButtonsAreSame = SettingsModelView.Settings.AllButtonsSame;
			
			operationList = new List<ToggleButton>();
			operationList.Add(Button_Plus);
			operationList.Add(Button_Minus);
			operationList.Add(Button_multipl);
			operationList.Add(Button_divide);
			operationList.Add(Button_Pow);
			operationList.Add(Button_Xor);
			operationList.Add(Button_LShift);
			operationList.Add(Button_RShift);
			operationList.Add(Button_Or);
			operationList.Add(Button_AND);

			InputText.Focus(FocusState.Pointer);
			//lastTextBox = InputText;

		}

		private void KeyboardOnOnButtonClick(object sender, ButtonClickArgs args)
		{
			//	InputText.Focus(Windows.UI.Xaml.FocusState.Pointer);
			TextBox input;
			if (!(FocusManager.GetFocusedElement() is TextBox))
				input = lastTextBox;
			else
				input = (TextBox) (FocusManager.GetFocusedElement());
			if (input == Result)
				return;
			var x = input.SelectionStart; //временное запоминание
			SharePages.AddTextTextBox(args.Button.Content.ToString(), input);
			CalculatorController.CalculateCommand.Execute();
		}

		private void Button_Click_Clean(object sender, ButtonClickArgs e)
		{
			if (!(FocusManager.GetFocusedElement() is TextBox))
				return;
			var input = (TextBox)(FocusManager.GetFocusedElement());
			if (input == Result)
				return;
			input.Text = "";
			CalculatorController.CalculateCommand.Execute();
		}


		private void Backspace_Click(object sender, ButtonClickArgs e)
		{
			if (!(FocusManager.GetFocusedElement() is TextBox))
				return;
			var input = (TextBox) (FocusManager.GetFocusedElement());
			if (input == Result)
				return;
			SharePages.Backspace(input);
			input.Select(input.Text.Length, 0);
			CalculatorController.CalculateCommand.Execute();
		}

		private void Button_Click_Dot(object sender, ButtonClickArgs e)
		{
			if (!(FocusManager.GetFocusedElement() is TextBox))
				return;
			var input = (TextBox) (FocusManager.GetFocusedElement());
			if (input == Result)
				return;
			if (input.Text.IndexOf(e.Button.Content.ToString()) < 0) //точки нету
				KeyboardOnOnButtonClick(sender, e);
		}

		private void ResultText(string text)
		{
			Result.Text = text;
		}


		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			InputText.Text = "";
		}

		private void From_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sizeKeyboard != null) 
				sizeKeyboard.VisibleButtonCount = CalculatorController.Input.InputBase;
			CalculatorController.CalculateCommand.Execute();
		}


		private void TextBox_SizeChanged(object sender, SizeChangedEventArgs e)
		{

			var x = ((TextBox) sender);
			
				x.TextWrapping = TextWrapping.Wrap;
				x.FontSize = x.ActualHeight*scaleFontTextBox;
		}

		private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var x = ((ButtonBase) sender);
			x.FontSize = (x.ActualHeight) * 0.7;
		}



		private void Buttons_SizeChanged_1(object sender, SizeChangedEventArgs e)
		{

			
		}

		private void ButtonOperationChecked(object sender, RoutedEventArgs e)
		{
			var button = (ToggleButton) sender;
			op = StrToOp(button.Content.ToString());
			button.IsChecked = true;

			if (operationList == null)
				return;

			for (int i = 0; i < operationList.Count; i++)
			{
				if (StrToOp(operationList[i].Content.ToString()) == op)
					continue;
				if (operationList[i] != null)
					operationList[i].IsChecked = false;
			}
			CalculatorController.CalculateCommand.Execute();
		}

		
		private void InputText_GotFocus(object sender, RoutedEventArgs e)
		{
			//Buttons.Visibility = Windows.UI.Xaml.Visibility.Visible;
			//int count = int.Parse(((ComboBoxItem)From2.SelectedItem).Content.ToString());
			lastTextBox = (TextBox) sender;
			sizeKeyboard.VisibleButtonCount = CalculatorController.Input.InputBase;
			//var selectedItem = ((ComboBoxItem) ((ComboBox) TextBoxToComboBox.First(a => a.Key == sender).Value).SelectedItem);
			//int _base = int.Parse(selectedItem.Content.ToString());
			//keyboard.SetVisibleButton(_base, false);
			//keyboard.ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, _base + 3);
		}

		private void InputText2_GotFocus(object sender, RoutedEventArgs e)
		{
			lastTextBox = (TextBox)sender;
			sizeKeyboard.VisibleButtonCount = CalculatorController.Input2.InputBase;
		}

		private void Result_GotFocus(object sender, RoutedEventArgs e)
		{
			//Buttons.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
		}

		private void From_SizeChanged(object sender, SizeChangedEventArgs e)
		{
		
		}

		private void From_Holding(object sender, RoutedEventArgs e)
		{
			FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);
			parentFlyout = (ComboBox) sender;
		}

		private void sizeKeyboard_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			//keyboard.ResizeButton(e.NewSize.Height, e.NewSize.Width, fromBase + 3);
		}

		private void To_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			CalculatorController.CalculateCommand.Execute();
			
		}

		private void Flyout_Opened(object sender, object e)
		{
			openedFlyout = sender as Flyout;
		}

		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 0)
				return;
			var listbox = ((ListBox)sender);
			var str = ((ListBoxItem)listbox.SelectedItem).Content;

			int newBase = int.Parse(str.ToString());
			CalculatorController.Input.AddNewBase.Execute(newBase);

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
				if (statView.Orientation == ApplicationViewOrientation.Landscape)  //FullScreen and Landscape
				{
					var marginComboBox = new Thickness(5,10,5,0);
					var marginTextBox = new Thickness(5, 5, 5, 5);
					var marginKeyboard = new Thickness(5, 5, 5, 0);
					var marginOperat = new Thickness(5, 5, 5, 5);

					MainGrid.RowDefinitions[1].Height = new GridLength(2, GridUnitType.Star);
					MainGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);
					MainGrid.RowDefinitions[3].Height = new GridLength(2, GridUnitType.Star);
					MainGrid.RowDefinitions[4].Height = new GridLength(2, GridUnitType.Star);
					MainGrid.RowDefinitions[5].Height = GridLength.Auto;
					MainGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);

					InputTextGrid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
					InputText.Margin = marginTextBox;

					From.Margin = marginComboBox;

					InputText2Grid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
					InputText2.Margin = marginTextBox;

					From2.Margin = marginComboBox;

					ResultGrid.RowDefinitions[1].Height = new GridLength(0.5, GridUnitType.Star);
					Result.Margin = marginTextBox;

					//Grid.SetRow(To, 3);
					//Grid.SetRowSpan(To, 1);
					To.Margin = marginComboBox;

					Buttons_operation.Margin = marginOperat;

					Grid.SetRow(sizeKeyboard, 1);
					Grid.SetColumn(sizeKeyboard, 3);
					sizeKeyboard.Margin = marginKeyboard;

					Grid.SetColumn(MenuGrid, 1);

					scaleFontTextBox = 0.35;
				}
				else     //FullScreen and Portrate
				{
					var marginComboBox = new Thickness(5, 10, 5, 0);
					var marginTextBox = new Thickness(5, 5, 5, 5);
					var marginKeyboard = new Thickness(0, 0, 0, 0);
					var marginOperat = new Thickness(5, 5, 5, 5);

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
					MainGrid.ColumnDefinitions[0].Width = new GridLength(0.5, GridUnitType.Star);
				//	MainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
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
			//correction = 15;
#endif
			var grd = (Grid)sender;
			for (int i = 0; i < operationList.Count; i++)
				operationList[i].Height = e.NewSize.Height / grd.RowDefinitions.Count + correction;
		}

		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			GoToConverter();
		}

		void GoToConverter()
		{
			SaveState();
			Frame.Navigate(typeof(MainPage));
		}

		void GoToThemes()
		{
			SaveState();
			Frame.Navigate(typeof(Themes));
		}

		void SaveState()
		{
			suspendPage = new SuspendPage();
			//suspendPage.indexFrom = FromBase;
			//suspendPage.indexFrom2 = FromBase2;
			//suspendPage.indexTo = ToBase;
			suspendPage.InputText = InputText.Text;
			suspendPage.InputText2 = InputText2.Text;
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
			//var d = (ComboBox) TextBoxToComboBox.First((a) => a.Key == sender).Value;
			//var i = int.Parse(((ComboBoxItem) d.SelectedItem).Content.ToString());
			//SharePages.InputText_KeyUp(sender, e, i);

		}

		private void ButtonOperationUnchecked(object sender, RoutedEventArgs e)
		{
			var button = (ToggleButton) sender;
			if (op == StrToOp(button.Content.ToString()))
				button.IsChecked = true;
		}

		private void Button_Minus_Unchecked(object sender, RoutedEventArgs e)
		{
			if (op == 1)
				Button_Minus.IsChecked = true;
		}

		private void Button_multipl_Unchecked(object sender, RoutedEventArgs e)
		{
			if (op == 2)
				Button_multipl.IsChecked = true;
		}

		private void Button_divide_Unchecked(object sender, RoutedEventArgs e)
		{
			if (op == 3)
				Button_divide.IsChecked = true;
		}

		int StrToOp(string str)
		{
			int operation = 0;
			switch (str.ToLower())
			{
				case "+":
					operation = 0;
					break;
				case "-":
					operation = 1;
					break;
				case "*":
				case "x":
					operation = 2;
					break;
				case "/":
					operation = 3;
					break;
				case "^":
					operation = 4;
					break;
				case "~":
					operation = 5;
					break;
				case "<<":
					operation = 6;
					break;
				case ">>":
					operation = 7;
					break;
				case "|":
					operation = 8;
					break;
				case "&":
					operation = 9;
					break;
				case "xor":
					operation = 10;
					break;
			}
			return operation;
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
				//FromBase = suspendPage.indexFrom;
				//FromBase2 = suspendPage.indexFrom2;
				//ToBase = suspendPage.indexTo;
				//if (From.Items != null && From2.Items != null && To.Items != null)
				//{
				//	From.SelectedItem = From.Items[FromBase];
				//	From2.SelectedItem = From2.Items[FromBase2];
				//	To.SelectedItem = To.Items[ToBase];
				//}
				
				//InputText.Text = suspendPage.InputText;
				//InputText2.Text = suspendPage.InputText2;
				//Calculate();
				//keyboard.ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, fromBase + 2);
				suspendPage = null;
			}
			InputText.Focus(FocusState.Programmatic);
		}

		private void Swipe(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			if (e.Velocities.Linear.X > 1)
				GoToConverter();
			else if (e.Velocities.Linear.X < -1)
				GoToThemes();
		}

		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			GoToThemes();
		}

		private void From2_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sizeKeyboard != null) 
				sizeKeyboard.VisibleButtonCount = CalculatorController.Input2.InputBase;
			CalculatorController.CalculateCommand.Execute();
		}

		private void ListBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 0)
				return;
			var listbox = ((ListBox)sender);
			var str = ((ListBoxItem)listbox.SelectedItem).Content;

			int newBase = int.Parse(str.ToString());
			CalculatorController.Input2.AddNewBase.Execute(newBase);

			openedFlyout.Hide();

			listbox.SelectionChanged -= ListBox_SelectionChanged;
			listbox.SelectedIndex = -1;
			listbox.SelectionChanged += ListBox_SelectionChanged;
		}

		private void ListBox3_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 0)
				return;
			var listbox = ((ListBox)sender);
			var str = ((ListBoxItem)listbox.SelectedItem).Content;

			int newBase = int.Parse(str.ToString());
			CalculatorController.Outputs.AddNewBase.Execute(newBase);

			openedFlyout.Hide();

			listbox.SelectionChanged -= ListBox_SelectionChanged;
			listbox.SelectedIndex = -1;
			listbox.SelectionChanged += ListBox_SelectionChanged;
		}
	}
}

