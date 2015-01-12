﻿using System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NumberConverter
{
	/// <summary>
	///     An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		//Keyboard keyboard;

		private static SuspendPage suspendPage;
		private readonly ConverterController converterController;
		private Flyout openedFlyout;
		private ComboBox parentFlyout;

		private double scaleFontTextBox = 0.35;

		public MainPage()
		{
			InitializeComponent();
			converterController = new ConverterController();
			DataContext = converterController;
		}

		//int fromBase;
		//int toBase;
		//string BAse;
		public int FromBase { get; set; }

		public int ToBase { get; set; }

		private void Button_Click_Dot(object sender, ButtonClickArgs e)
		{
			InputText.Focus(FocusState.Pointer);
			if (converterController.DotCommand.CanExecute(null)) //точки нету
				converterController.DotCommand.Execute();
		}

		private void Button_Click_Clean(object sender, ButtonClickArgs e)
		{
			InputText.Focus(FocusState.Pointer);
			if (converterController.ClearCommand.CanExecute(null))
				converterController.ClearCommand.Execute();
		}

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				if (converterController.ConvertCommand.CanExecute(null))
					converterController.ConvertCommand.Execute();
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

		private void Button_Click_1(object sender, ButtonClickArgs e)
		{
			SharePages.AddTextTextBox(e.Button.Content.ToString(), InputText);
			//InputText.Select(InputText.Text.Length, 0);
			converterController.ConvertCommand.Execute();
		}

		private void InputText_PointerPressed(object sender, PointerRoutedEventArgs e)
		{
			//if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Touch)
			//	((TextBox)sender).IsReadOnly = true;
			//else
			//	((TextBox)sender).IsReadOnly = false;
		}

		private void From_Holding(object sender, HoldingRoutedEventArgs e)
		{
			FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);
			parentFlyout = (ComboBox) sender;
		}


		private void To_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				buttons.VisibleButtonCount = converterController.Input.InputBase;
				if (converterController.ConvertCommand.CanExecute(null))
					converterController.ConvertCommand.Execute();
				//if (From != null || To != null)
				//{
				//	toBase = int.Parse(((ComboBoxItem)To.SelectedItem).Content.ToString());
				//	Result.Text = Converter.Converter.ConvertTo((byte)fromBase,
				//		InputText.Text, (byte)toBase);
				//}
			}
			catch (Exception ee)
			{
				Result.Text = ee.Message;
			}
		}

		private void Result_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			SharePages.ResizeFontTextBox((TextBox) sender, scaleFontTextBox);
		}

		private void Buttons_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			//var x = ((Button)sender);
			//if (x.Height == double.NaN)

			//x.Height = Clearr.ActualWidth-50;
			//x.Width = Clearr.ActualWidth -50;
			//x.FontSize = x.Height - 50;
		}


		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			ApplicationView statView = ApplicationView.GetForCurrentView();

			MenuGrid.ColumnDefinitions[0].Width = new GridLength(MainGrid.ColumnDefinitions[0].ActualWidth);
			if (statView.IsFullScreen)
				if (statView.Orientation == ApplicationViewOrientation.Landscape) //FullScreen and Landscape
				{
					var marginComboBox = new Thickness(20, 40, 20, 60);

					Grid.SetRowSpan(InputText, 2);
					Grid.SetColumnSpan(InputText, 2);

					Grid.SetRow(Result, 3);
					Grid.SetRowSpan(Result, 2);
					Grid.SetColumnSpan(Result, 2);

					Grid.SetRow(To, 3);
					Grid.SetRowSpan(To, 1);

					Grid.SetRow(sizeKeyboard, 1);
					Grid.SetRowSpan(sizeKeyboard, 5);
					Grid.SetColumn(sizeKeyboard, 3);
					Grid.SetColumnSpan(sizeKeyboard, 2);

					Grid.SetColumn(MenuGrid, 1);

					InputText.Margin = new Thickness(10, 20, 10, 60);
					Result.Margin = new Thickness(10, 10, 10, 60);
					From.Margin = marginComboBox;
					To.Margin = marginComboBox;
					sizeKeyboard.Margin = new Thickness(10, 10, 10, 60);
					MainGrid.ColumnDefinitions[0].Width = new GridLength(0.5, GridUnitType.Star);
					scaleFontTextBox = 0.25;

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
				else //FullScreen and Portrate
				{
					var marginComboBox = new Thickness(5, 40, 5, 120);

					Grid.SetRowSpan(InputText, 1);
					Grid.SetColumnSpan(InputText, 4);

					Grid.SetRow(To, 2);
					Grid.SetRowSpan(To, 1);

					Grid.SetRow(Result, 2);
					Grid.SetRowSpan(Result, 1);
					Grid.SetColumnSpan(Result, 4);

					Grid.SetRow(sizeKeyboard, 3);
					Grid.SetColumn(sizeKeyboard, 0);
					Grid.SetColumnSpan(sizeKeyboard, 5);

					Grid.SetColumn(MenuGrid, 0);

					InputText.Margin = new Thickness(5, 5, 5, 5);
					Result.Margin = new Thickness(5, 5, 5, 5);
					From.Margin = marginComboBox;
					To.Margin = marginComboBox;
					sizeKeyboard.Margin = new Thickness(5, 5, 5, 5);
					MainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
					MenuGrid.ColumnDefinitions[0].Width = GridLength.Auto;
					//VisualStateManager.GoToState(this, "SnappedLandscape", true);
					scaleFontTextBox = 0.35;
				}
			else // not full screen
			{
				if (ActualWidth <= 510)
				{
					var marginComboBox = new Thickness(5, 40, 5, 60);

					Grid.SetRowSpan(InputText, 1);
					Grid.SetColumnSpan(InputText, 4);

					Grid.SetRow(To, 2);
					Grid.SetRowSpan(To, 1);

					Grid.SetRow(Result, 2);
					Grid.SetRowSpan(Result, 1);
					Grid.SetColumnSpan(Result, 4);

					Grid.SetRow(sizeKeyboard, 3);
					Grid.SetColumn(sizeKeyboard, 0);
					Grid.SetColumnSpan(sizeKeyboard, 5);

					Grid.SetColumn(MenuGrid, 0);

					InputText.Margin = new Thickness(5, 5, 5, 5);
					From.Margin = marginComboBox;
					Result.Margin = new Thickness(5, 5, 5, 5);
					To.Margin = marginComboBox;
					sizeKeyboard.Margin = new Thickness(5, 5, 5, 5);
					MainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
					MenuGrid.ColumnDefinitions[0].Width = GridLength.Auto;
					//VisualStateManager.GoToState(this, "SnappedLandscape", true);
					scaleFontTextBox = 0.35;
				}
				else
				{
					var marginComboBox = new Thickness(5, 10, 5, 60);

					Grid.SetRowSpan(InputText, 2);
					Grid.SetColumnSpan(InputText, 2);

					Grid.SetRow(Result, 3);
					Grid.SetRowSpan(Result, 2);
					Grid.SetColumnSpan(Result, 2);

					Grid.SetRow(To, 3);
					Grid.SetRowSpan(To, 1);

					Grid.SetRow(sizeKeyboard, 1);
					Grid.SetRowSpan(sizeKeyboard, 5);
					Grid.SetColumn(sizeKeyboard, 3);
					Grid.SetColumnSpan(sizeKeyboard, 2);

					Grid.SetColumn(MenuGrid, 1);

					InputText.Margin = new Thickness(5, 5, 5, 5);
					From.Margin = new Thickness(5, 40, 5, 60);
					Result.Margin = marginComboBox;
					To.Margin = marginComboBox;
					sizeKeyboard.Margin = new Thickness(5, 5, 5, 5);
					MainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
					scaleFontTextBox = 0.25;
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

		private void From_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			SharePages.ScaleText((ComboBox) sender, e.NewSize.Height);
			//double scale = e.PreviousSize.Height / e.NewSize.Height;
			//((ComboBox)sender).FontSize = (e.NewSize.Height) * 0.7;
			//combo.Height = e.NewSize.Height;
			//combo.Width = e.NewSize.Width;
			//combo.FontSize = e.NewSize.Height * 0.7;
		}

		private void SaveState()
		{
			suspendPage = new SuspendPage();
			//suspendPage.indexFrom = FromBase;
			//suspendPage.indexFrom2 = FromBase2;
			//suspendPage.indexTo = ToBase;
			suspendPage.InputText = InputText.Text;
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			SaveState();
			Frame.Navigate(typeof (BlankPage1));
		}

		private void Button_GotFocus(object sender, RoutedEventArgs e)
		{
		}

		private void sizeKeyboard_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			//keyboard.ResizeButton(e.NewSize.Height, e.NewSize.Width, int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString()) + 3);
		}

		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			SaveState();
			Frame.Navigate(typeof (BlankPage1));
		}

		private void From_RightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			FlyoutBase.ShowAttachedFlyout((FrameworkElement) sender);
			parentFlyout = (ComboBox) sender;
		}

		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var listbox = ((ListBox) sender);
			var listBoxItem = (ListBoxItem) listbox.SelectedItem;
			if (listBoxItem != null)
			{
				object str = listBoxItem.Content;

				byte newBase = byte.Parse((string) str);

				((InputField) parentFlyout.DataContext).AddNewBase.Execute(newBase);
			}


			openedFlyout.Hide();
			listbox.SelectionChanged -= ListBox_SelectionChanged;
			listbox.SelectedIndex = -1;
			listbox.SelectionChanged += ListBox_SelectionChanged;
		}

		private void Flyout_Opened(object sender, object e)
		{
			openedFlyout = sender as Flyout;
		}

		private void InputText_Tapped(object sender, TappedRoutedEventArgs e)
		{
			//if (e.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Touch)
			//	((TextBox)sender).IsReadOnly = true;
			//else
			//	((TextBox)sender).IsReadOnly = false;
		}

		private void InputText_GotFocus(object sender, RoutedEventArgs e)
		{
			buttons.VisibleButtonCount = converterController.Input.InputBase;
		}

		private void InputText_SelectionChanged(object sender, RoutedEventArgs e)
		{
		}

		private void InputText_PointerReleased(object sender, PointerRoutedEventArgs e)
		{
		}

		private void Backspace_Click(object sender, ButtonClickArgs e)
		{
			SharePages.Backspace(InputText);
			InputText.Select(InputText.Text.Length, 0);
		}


		private void InputText_KeyUp(object sender, KeyRoutedEventArgs e)
		{
			//if ("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(e.Key.ToString())) ;

			//if (e.Key)
			SharePages.InputText_KeyUp(sender, e, converterController.Input.InputBase);
		}

		

		private void HyperlinkButton_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			((ButtonBase) sender).FontSize = e.NewSize.Height*0.5;
		}

		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			var newDictionary = new ResourceDictionary();
			newDictionary.Source = new Uri("ms-resource:/Files/Resource/DarkOrange.xaml", UriKind.Absolute);
			Application.Current.Resources = newDictionary;
			SaveState();
			Frame.Navigate(typeof (MainPage));
			Frame.BackStack.RemoveAt(0);
			//VisualStateManager.GoToState(this, "SnappedLandscape", false);
			//ApplicationViewStates.SetValue(); = SnappedLandscape;
		}

		private void Button_Click_4(object sender, RoutedEventArgs e)
		{
			//App.Current.Resources
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			if (suspendPage != null)
			{
				//FromBase = suspendPage.indexFrom;

				//ToBase = suspendPage.indexTo;
				//InputText.Text = suspendPage.InputText;
				////keyboard.ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, fromBase + 3);
				//if (From.Items != null  && To.Items != null)
				//{
				//	From.SelectedItem = From.Items[FromBase];

				//	To.SelectedItem = To.Items[ToBase];
				//}
				suspendPage = null;
			}
			InputText.Focus(FocusState.Programmatic);
		}

		private void Page_GotFocus(object sender, RoutedEventArgs e)
		{
			//if (e.OriginalSource is TextBox)
			//	((TextBox) e.OriginalSource).Focus(FocusState.Programmatic);
		}

		private void InputText_LostFocus(object sender, RoutedEventArgs e)
		{
		}
	}
}