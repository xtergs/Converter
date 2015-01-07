using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
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
		Keyboard keyboard;
		ComboBox parentFlyout;

		private ConverterControllerManyOutput converterControllerManyOutput
		{
			get { return converterControllerManyOutputSet; }
		}

		private ConverterControllerManyOutput converterControllerManyOutputSet;

		private double scaleFontTextBox = 0.35;

		static SuspendPage suspendPage;

		public MainPage()
		{
			this.InitializeComponent();
			ListView.Items.Clear();
			converterControllerManyOutputSet = new ConverterControllerManyOutput();
			converterControllerManyOutput.Outputs.Add(new InputField());
			converterControllerManyOutput.Outputs.Add(new InputField());
			converterControllerManyOutput.Outputs.Add(new InputField());
			converterControllerManyOutput.Outputs.Add(new InputField());
			converterControllerManyOutput.Outputs.Add(new InputField());
			converterControllerManyOutput.Outputs.Add(new InputField());
			converterControllerManyOutput.Outputs.Add(new InputField());
			converterControllerManyOutput.Outputs.Add(new InputField());
			DataContext = converterControllerManyOutput;

			sizeKeyboard.MaxButtonHeight = SettingsModelView.Settings.MaxSizeButton;
			sizeKeyboard.MaxButtonWidth = SettingsModelView.Settings.MaxSizeButton;
			sizeKeyboard.ButtonsAreSame = SettingsModelView.Settings.AllButtonsSame;
			sizeKeyboard.ResizeButton();
		}

		private void Button_Click_Dot(object sender, ButtonClickArgs e)
		{
			InputText.Focus(Windows.UI.Xaml.FocusState.Pointer);
			if (InputText.Text.IndexOf(e.Button.Content.ToString()) < 0) //точки нету
				Button_Click_1(sender, e);
		}

		private void Button_Click_Clean(object sender, ButtonClickArgs e)
		{
			InputText.Text = String.Empty;
			converterControllerManyOutput.ConvertCommand.Execute();
			
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			InputText.Text = String.Empty;
			converterControllerManyOutput.ConvertCommand.Execute();
		}

		private void Button_Click_1(object sender, ButtonClickArgs e)
		{
			SharePages.AddTextTextBox(e.Button.Content.ToString(), InputText);
			InputText.Select(InputText.Text.Length, 0);
			converterControllerManyOutput.ConvertCommand.Execute();
		}
		
		private void From_Holding(object sender, HoldingRoutedEventArgs e)
		{
			FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
			parentFlyout = (ComboBox)sender;
			e.Handled = true;
		}

		private void Result_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			SharePages.ResizeFontTextBox((TextBox)sender, scaleFontTextBox);
		}
				
		private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var statView = ApplicationView.GetForCurrentView();
			if (statView.IsFullScreen)
				if (statView.Orientation == ApplicationViewOrientation.Landscape)  //FullScreen and Landscape
				{
					Grid.SetRowSpan(InputText, 2);
					Grid.SetColumnSpan(InputText, 2);

					//Grid.SetRow(ListView.Items = Result, 3);
					//Grid.SetRowSpan(Result, 2);
					//Grid.SetColumnSpan(Result, 2);

					//Grid.SetRow(To, 3);
					//Grid.SetRowSpan(To, 1);

					Grid.SetRow(ListView,3);
					Grid.SetRowSpan(ListView, 2);
					Grid.SetColumnSpan(ListView, 3);

					Grid.SetRow(sizeKeyboard, 1);
					Grid.SetRowSpan(sizeKeyboard, 10);
					Grid.SetColumn(sizeKeyboard, 3);
					Grid.SetColumnSpan(sizeKeyboard, 3);

					Grid.SetColumn(MenuGrid, 1);

					InputText.Margin = new Thickness(5, 5, 5, 0);
					From.Margin = new Thickness(5, 10, 5, 0);
					//Result.Margin = new Thickness(5, 5, 5, 5);
					//To.Margin = new Thickness(5, 10, 5, 0);
					sizeKeyboard.Margin = new Thickness(0, 0,0,0);
					MainGrid.ColumnDefinitions[0].Width = new GridLength(0.5, GridUnitType.Star);
					scaleFontTextBox = 0.25;
				}
				else     //FullScreen and Portrate
				{
					Grid.SetRowSpan(InputText, 1);
					Grid.SetColumnSpan(InputText, 4);

					//Grid.SetRow(To, 2);
					//Grid.SetRowSpan(To, 1);


					//Grid.SetRow(Result, 2);
					//Grid.SetRowSpan(Result, 1);
					
					//Grid.SetColumnSpan(Result, 4);
					Grid.SetRow(ListView, 2);
					Grid.SetRowSpan(ListView, 1);
					Grid.SetColumnSpan(ListView, 5);

					Grid.SetRow(sizeKeyboard, 3);
					Grid.SetColumn(sizeKeyboard, 0);
					Grid.SetColumnSpan(sizeKeyboard, 5);

					Grid.SetColumn(MenuGrid, 0);

					InputText.Margin = new Thickness(5, 5, 5, 5);
					From.Margin = new Thickness(5, 10, 5, 0);
					//Result.Margin = new Thickness(5, 5, 5, 5);
					//To.Margin = new Thickness(5, 10, 5, 0);
					sizeKeyboard.Margin = new Thickness(0, 0, 0, 0);
					MainGrid.ColumnDefinitions[0].Width = new GridLength(0.5, GridUnitType.Star);
					scaleFontTextBox = 0.35;
				}
			else  // not full screen
			{
				if (this.ActualWidth <= 510 && ActualWidth >= 490)
				{
					Grid.SetRowSpan(InputText, 1);
					Grid.SetColumnSpan(InputText, 4);

					//Grid.SetRow(To, 2);
					//Grid.SetRowSpan(To, 1);

					//Grid.SetRow(Result, 2);
					//Grid.SetRowSpan(Result, 1);
					//Grid.SetColumnSpan(Result, 4);

					Grid.SetRow(ListView, 2);
					Grid.SetRowSpan(ListView, 1);
					Grid.SetColumnSpan(ListView, 4);

					Grid.SetRow(sizeKeyboard, 3);
					Grid.SetColumn(sizeKeyboard, 0);
					Grid.SetColumnSpan(sizeKeyboard, 5);

					Grid.SetColumn(MenuGrid, 0);

					InputText.Margin = new Thickness(5, 5, 5, 5);
					From.Margin = new Thickness(5, 40, 5, 60);
					//Result.Margin = new Thickness(5, 5, 5, 5);
					//To.Margin = new Thickness(5, 40, 5, 60);
					sizeKeyboard.Margin = new Thickness(5, 5, 5, 5);
					MainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
					scaleFontTextBox = 0.35;
				}
				else
				{
					Grid.SetRowSpan(InputText, 2);
					Grid.SetColumnSpan(InputText, 2);

					//Grid.SetRow(Result, 3);
					//Grid.SetRowSpan(Result, 2);
					//Grid.SetColumnSpan(Result, 2);

					//Grid.SetRow(To, 3);
					//Grid.SetRowSpan(To, 1);

					Grid.SetRow(ListView, 3);
					Grid.SetRowSpan(ListView, 2);
					Grid.SetColumnSpan(ListView, 3);

					Grid.SetRow(sizeKeyboard, 1);
					Grid.SetRowSpan(sizeKeyboard, 5);
					Grid.SetColumn(sizeKeyboard, 3);
					Grid.SetColumnSpan(sizeKeyboard, 2);

					Grid.SetColumn(MenuGrid, 1);

					InputText.Margin = new Thickness(5, 5, 5, 5);
					From.Margin = new Thickness(5, 40, 5, 60);
					//Result.Margin = new Thickness(5, 5, 5, 5);
					//To.Margin = new Thickness(5, 40, 5, 60);
					sizeKeyboard.Margin = new Thickness(5, 5, 5, 5);
					MainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
					scaleFontTextBox = 0.25;
				}
			}
		}

		private void From_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			SharePages.ScaleText((ComboBox)sender, e.NewSize.Height);
		}

		private void CalculatorHyperlink_Click(object sender, RoutedEventArgs e)
		{
			GoToCalculator();
			
		}

		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			GoToCalculator();
		}

		private void From_RightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
			parentFlyout = (ComboBox)sender;
			e.Handled = true;
		}

		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 0)
				return;
			var listbox = ((ListBox)sender);
			var str = ((ListBoxItem)listbox.SelectedItem).Content;
			//var fly = ((Flyout)((FlyoutPresenter)((Grid)((ListBox)sender).Parent).Parent).Parent);
			byte newBase = byte.Parse(str.ToString());
			converterControllerManyOutput.Input.AddNewBase.Execute(newBase);
			//parentFlyout.SelectedIndex = parentFlyout.Items.Count - 1;
			//fly.Hide();
			openedFlyout.Hide();
			listbox.SelectionChanged -= ListBox_SelectionChanged;
			listbox.SelectedIndex = -1;
			listbox.SelectionChanged += ListBox_SelectionChanged;
		}

		Flyout openedFlyout;
		private void Flyout_Opened(object sender, object e)
		{
			openedFlyout = sender as Flyout;
		}

		object focusedTextBox;

		private void InputText_GotFocus(object sender, RoutedEventArgs e)
		{
			focusedTextBox = sender;
		}

		private void Backspace_Click(object sender, ButtonClickArgs e)
		{
			SharePages.Backspace(InputText);
			InputText.Select(InputText.Text.Length, 0);
			converterControllerManyOutput.ConvertCommand.Execute();
		}

		
		private void InputText_KeyUp(object sender, KeyRoutedEventArgs e)
		{
			//if ("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(e.Key.ToString())) ;
			
			//if (e.Key)
			SharePages.InputText_KeyUp(sender, e, converterControllerManyOutput.Input.InputBase);
			e.Handled = true;
		}

		private void HyperlinkButton_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			((ButtonBase) sender).FontSize = e.NewSize.Height*0.5;
		}

		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			GoToConverter();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			//if (suspendPage != null)
			//{
			//	FromBase = suspendPage.indexFrom;
				
			//	ToBase = suspendPage.indexTo;
			//	InputText.Text = suspendPage.InputText;
			//	//keyboard.ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, fromBase + 3);
			//	if (From.Items != null  && To.Items != null)
			//	{
			//		From.SelectedItem = From.Items[FromBase];
				
			//		To.SelectedItem = To.Items[ToBase];
			//	}
			//	InputText.Focus(FocusState.Programmatic);
			//	suspendPage = null;
			//}
		}

		void SaveState()
		{
			suspendPage = new SuspendPage();
			//suspendPage.indexFrom = FromBase;
			//suspendPage.indexFrom2 = FromBase2;
			//suspendPage.indexTo = ToBase;
			suspendPage.InputText = InputText.Text;
		}

		private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
		{
			GoToThemes();
		}

		void GoToThemes()
		{
			Pivot.SelectedItem = SettignsItem;
			ConverterHyperLink.IsEnabled = true;
			CalculatorHyperLink.IsEnabled = true;
			SettingsHyperLink.IsEnabled = false;
		}

		void GoToConverter()
		{
			Pivot.SelectedItem = ConverterItem;
			ConverterHyperLink.IsEnabled = false;
			CalculatorHyperLink.IsEnabled = true;
			SettingsHyperLink.IsEnabled = true;
		}

		void GoToCalculator()
		{
			Pivot.SelectedItem = CalculatorItem;
			ConverterHyperLink.IsEnabled = true;
			CalculatorHyperLink.IsEnabled = false;
			SettingsHyperLink.IsEnabled = true;
			//SaveState();
			//this.Frame.Navigate(typeof(BlankPage1));
		}

		private void Swipe(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			double x = e.Velocities.Linear.X;
			e.Handled = true;
			if (x < -1)
				GoToCalculator();
		}

		private void From_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
		{
			sizeKeyboard.VisibleButtonCount = converterControllerManyOutput.Input.InputBase;

			if (converterControllerManyOutput.ConvertCommand.CanExecute(null))
				converterControllerManyOutput.ConvertCommand.Execute();

		}

		private void ListBox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 0)
				return;
			var listbox = ((ListBox)sender);
			var str = ((ListBoxItem)listbox.SelectedItem).Content;

			int newBase = int.Parse(str.ToString());
			converterControllerManyOutput.Outputs[0].AddNewBase.Execute(newBase);

			openedFlyout.Hide();

			listbox.SelectionChanged -= ListBox_SelectionChanged;
			listbox.SelectedIndex = -1;
			listbox.SelectionChanged += ListBox_SelectionChanged;
		}

		private void ConverterItem_GotFocus(object sender, RoutedEventArgs e)
		{
			ConverterHyperLink.IsEnabled = false;
			CalculatorHyperLink.IsEnabled = true;
			SettingsHyperLink.IsEnabled = true;
		}

		private void CalculatorItem_GotFocus(object sender, RoutedEventArgs e)
		{
			ConverterHyperLink.IsEnabled = true;
			CalculatorHyperLink.IsEnabled = false;
			SettingsHyperLink.IsEnabled = true;
		}

		private void SettignsItem_GotFocus(object sender, RoutedEventArgs e)
		{
			ConverterHyperLink.IsEnabled = true;
			CalculatorHyperLink.IsEnabled = true;
			SettingsHyperLink.IsEnabled = false;
		}

		private void Page_Unloaded(object sender, RoutedEventArgs e)
		{
			//keyboard = null;
			//this.converterControllerManyOutputSet = null;
			//this.parentFlyout = null;
			//this.openedFlyout = null;
			//this.focusedTextBox = null;
			//DataContext = null;
		}

		private void From_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (converterControllerManyOutput.ConvertCommand.CanExecute(null))
				converterControllerManyOutput.ConvertCommand.Execute();
		}

		private void ListBox2_SelectionChanged1(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count == 0)
				return;
			var listbox = ((ListBox)sender);
			var str = ((ListBoxItem)listbox.SelectedItem).Content;
			//var fly = ((Flyout)((FlyoutPresenter)((Grid)((ListBox)sender).Parent).Parent).Parent);
			byte newBase = byte.Parse(str.ToString());
			//converterControllerManyOutput.Input.AddNewBase.Execute(newBase);
			//parentFlyout.Items.Add(newBase);
			//parentFlyout.SelectedIndex = parentFlyout.Items.Count - 1;
			//fly.Hide();
			openedFlyout.Hide();
			listbox.SelectionChanged -= ListBox_SelectionChanged;
			listbox.SelectedIndex = -1;
			listbox.SelectionChanged += ListBox_SelectionChanged;
		}

		
		
	}
}
