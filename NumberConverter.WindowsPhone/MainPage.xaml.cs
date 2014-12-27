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
		//Keyboard keyboard;
		ComboBox parentFlyout;

		private ConverterController converterControllerManyOutput;

		private double scaleFontTextBox = 0.35;

		int fromBase;
		int toBase;
		int fromBaseIndex,  toBaseIndex;
		static SuspendPage suspendPage;
		//string BAse;
		public int FromBase
		{
			get { return converterControllerManyOutput.Input.InputBase; }
			set
			{
				//fromBaseIndex = value;
				//fromBase = int.Parse(((ComboBoxItem)From.Items[value]).Content.ToString());
			}
		}

		public int ToBase
		{
			get { return converterControllerManyOutput.Outputs.InputBase; }
			set
			{
				//toBaseIndex = value;
				//toBase = int.Parse(((ComboBoxItem)To.Items[value]).Content.ToString());
			}
		}
		public MainPage()
		{
			this.InitializeComponent();
			converterControllerManyOutput = new ConverterController();
			DataContext = converterControllerManyOutput;
			//CreateKeyboard(Buttons);
			ToBase = 0;
			FromBase = 0;
			//fromBase = int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString());
			//toBase = int.Parse(((ComboBoxItem)To.SelectedItem).Content.ToString());
			
			//keyboard.SetVisibleButton(FromBase);
			
		}

		public void CreateKeyboard(Panel panel)
		{
			//keyboard = new Keyboard(panel, (Application.Current.Resources["ButtonStyle1"]) as Style);
			//keyboard.OnButtonClick += Button_Click_1;
			//keyboard.OnDotClick += Button_Click_Dot; // "."
			//keyboard.OnBackspaceClick += Backspace_Click; // backspace
			//keyboard.OnCleanClick += Button_Click_Clean; //Clean
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

		private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			
			//try
			//{
			//	if (InputText.Text != "")
			//		Result.Text = Converter.Converter.ConvertTo((uint)fromBase,
			//		InputText.Text, (uint)toBase);
			//}
			//catch (Exception ee)
			//{
			//	Result.Text = ee.Message;
			//}
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
		}

		private void From_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				if (sender != null && Result != null)
				{
					fromBase = int.Parse((((ComboBoxItem)((ComboBox)sender).SelectedItem)).Content.ToString());
					Result.Text = Converter.Converter.ConvertTo((uint)fromBase,	InputText.Text, (uint)toBase);
					//keyboard.SetVisibleButton(fromBase);
					//keyboard.ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, fromBase + 3);
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
					toBase = int.Parse(((ComboBoxItem)To.SelectedItem).Content.ToString());
					Result.Text = Converter.Converter.ConvertTo((uint)fromBase,
						InputText.Text, (uint) toBase);
				}
			}
			catch (Exception ee)
			{
				Result.Text = ee.Message;
			}
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

					Grid.SetRow(Result, 3);
					Grid.SetRowSpan(Result, 2);
					Grid.SetColumnSpan(Result, 2);

					Grid.SetRow(To, 3);
					Grid.SetRowSpan(To, 1);

					Grid.SetRow(sizeKeyboard, 1);
					Grid.SetRowSpan(sizeKeyboard, 10);
					Grid.SetColumn(sizeKeyboard, 3);
					Grid.SetColumnSpan(sizeKeyboard, 3);

					Grid.SetColumn(MenuGrid, 1);

					InputText.Margin = new Thickness(5, 5, 5, 0);
					From.Margin = new Thickness(5, 10, 5, 0);
					Result.Margin = new Thickness(5, 5, 5, 5);
					To.Margin = new Thickness(5, 10, 5, 0);
					sizeKeyboard.Margin = new Thickness(0, 0,0,0);
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
				else     //FullScreen and Portrate
				{
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
					From.Margin = new Thickness(5, 10, 5, 0);
					Result.Margin = new Thickness(5, 5, 5, 5);
					To.Margin = new Thickness(5, 10, 5, 0);
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
					From.Margin = new Thickness(5, 40, 5, 60);
					Result.Margin = new Thickness(5, 5, 5, 5);
					To.Margin = new Thickness(5, 40, 5, 60);
					sizeKeyboard.Margin = new Thickness(5, 5, 5, 5);
					MainGrid.ColumnDefinitions[0].Width = GridLength.Auto;
					scaleFontTextBox = 0.35;
				}
				else
				{
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
					Result.Margin = new Thickness(5, 5, 5, 5);
					To.Margin = new Thickness(5, 40, 5, 60);
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
			SharePages.ScaleText((ComboBox)sender, e.NewSize.Height);
			//double scale = e.PreviousSize.Height / e.NewSize.Height;
			//((ComboBox)sender).FontSize = (e.NewSize.Height) * 0.7;
			//combo.Height = e.NewSize.Height;
			//combo.Width = e.NewSize.Width;
			//combo.FontSize = e.NewSize.Height * 0.7;
		}

		private void CalculatorHyperlink_Click(object sender, RoutedEventArgs e)
		{
			GoToCalculator();
			
		}

		private void sizeKeyboard_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			//keyboard.ResizeButton(e.NewSize.Height, e.NewSize.Width, int.Parse(((ComboBoxItem)From.SelectedItem).Content.ToString()) + 3);
		}
		
		private void Button_Click_5(object sender, RoutedEventArgs e)
		{
			GoToCalculator();
		}

		private void From_RightTapped(object sender, RightTappedRoutedEventArgs e)
		{
			FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
			parentFlyout = (ComboBox)sender;
		}

		private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var listbox = ((ListBox)sender);
			var str = ((ListBoxItem)listbox.SelectedItem).Content;
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
			SharePages.InputText_KeyUp(sender, e, fromBase);
		}

		private void HyperlinkButton_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			((ButtonBase) sender).FontSize = e.NewSize.Height*0.5;
		}

		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			ResourceDictionary newDictionary = new ResourceDictionary();
			newDictionary.Source = new Uri("ms-resource:/Files/Resource/DarkOrange.xaml", UriKind.Absolute);
			Application.Current.Resources = newDictionary;
			Frame.Navigate(typeof(MainPage));
			Frame.BackStack.RemoveAt(0);
			//VisualStateManager.GoToState(this, "SnappedLandscape", false);
			//ApplicationViewStates.SetValue(); = SnappedLandscape;
		}

		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			if (suspendPage != null)
			{
				FromBase = suspendPage.indexFrom;
				
				ToBase = suspendPage.indexTo;
				InputText.Text = suspendPage.InputText;
				//keyboard.ResizeButton(sizeKeyboard.ActualHeight, sizeKeyboard.ActualWidth, fromBase + 3);
				if (From.Items != null  && To.Items != null)
				{
					From.SelectedItem = From.Items[FromBase];
				
					To.SelectedItem = To.Items[ToBase];
				}
				InputText.Focus(FocusState.Programmatic);
				suspendPage = null;
			}
		}

		private void Page_Unloaded(object sender, RoutedEventArgs e)
		{
			//keyboard.OnButtonClick -= Button_Click_1;
			//keyboard.OnDotClick -= Button_Click_Dot; // "."
			//keyboard.OnBackspaceClick -= Backspace_Click; // backspace
			//keyboard.OnCleanClick -= Button_Click_Clean; //Clean
		}


		void SaveState()
		{
			suspendPage = new SuspendPage();
			suspendPage.indexFrom = FromBase;
			//suspendPage.indexFrom2 = FromBase2;
			suspendPage.indexTo = ToBase;
			suspendPage.InputText = InputText.Text;
		}

		private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
		{
			GoToThemes();
		}

		void GoToThemes()
		{
			SaveState();
			this.Frame.Navigate(typeof(Themes));
		}

		void GoToCalculator()
		{
			SaveState();
			this.Frame.Navigate(typeof(BlankPage1));
		}

		private void Swipe(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			double x = e.Velocities.Linear.X;
			if (x < -1)
				GoToCalculator();
		}

		private void From_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
		{
			sizeKeyboard.VisibleButtonCount = converterControllerManyOutput.Input.InputBase;
			converterControllerManyOutput.ConvertCommand.Execute();
		}

		
		
	}
}
