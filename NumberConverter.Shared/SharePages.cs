using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace NumberConverter
{
	class SharePages
	{
		public static void AddComboBoxItem(ComboBoxItem item, ComboBox combobox, bool isSelet)
		{
			if (!combobox.Items.Any((a) =>
			{
				if (((ComboBoxItem)a).Content.ToString() == item.Content.ToString())
					return true;
				return false;
			}
			))
			{
				combobox.Items.Add(item);
				combobox.SelectedItem = combobox.Items.Last();
			}
			else
			{
				combobox.SelectedIndex = combobox.Items.IndexOf(combobox.Items.Single((a) => ((ComboBoxItem)a).Content.ToString() == item.Content.ToString()));
			}
		}

		public static void Backspace(TextBox InputText)
		{
			if (InputText.Text != "")
				InputText.Text = InputText.Text.Remove(InputText.Text.Length - 1);
		}

		public static void InputText_KeyUp(object sender, KeyRoutedEventArgs e, int fromBase)
		{
			var textbox = ((TextBox)sender);
			switch (e.Key)
			{
				case VirtualKey.NumberPad0:
				case VirtualKey.Number0:
					if (0 < fromBase)
						textbox.Text += "0";
					else
						e.Handled = true;
					break;
				case VirtualKey.NumberPad1:
				case VirtualKey.Number1:
					if (1 < fromBase)
						textbox.Text += "1";
					else
						e.Handled = true;
					break;
				case VirtualKey.NumberPad2:
				case VirtualKey.Number2:
					if (2 < fromBase)
						textbox.Text += "2";
					else
						e.Handled = true;
					break;
				case VirtualKey.NumberPad3:
				case VirtualKey.Number3:
					if (3 < fromBase)
						textbox.Text += "3";
					else
						e.Handled = true;
					break;
				case VirtualKey.NumberPad4:
				case VirtualKey.Number4:
					if (4 < fromBase)
						textbox.Text += "4";
					else
						e.Handled = true;
					break;
				case VirtualKey.NumberPad5:
				case VirtualKey.Number5:
					if (5 < fromBase)
						textbox.Text += "5";
					else
						e.Handled = true;
					break;
				case VirtualKey.NumberPad6:
				case VirtualKey.Number6:
					if (6 < fromBase)
						textbox.Text += "6";
					else
						e.Handled = true;
					break;
				case VirtualKey.NumberPad7:
				case VirtualKey.Number7:
					if (7 < fromBase)
						textbox.Text += "7";
					else
						e.Handled = true;
					break;
				case VirtualKey.NumberPad8:
				case VirtualKey.Number8:
					if (8 < fromBase)
						textbox.Text += "8";
					else
						e.Handled = true;
					break;
				case VirtualKey.NumberPad9:
				case VirtualKey.Number9:
					if (9 < fromBase)
						textbox.Text += "9";
					else
						e.Handled = true;
					break;
				case VirtualKey.Back: SharePages.Backspace(textbox); break;
			}
			if (e.Key == VirtualKey.Decimal && textbox.Text.IndexOf(LongDouble.Splitter) < 0)
				textbox.Text += LongDouble.Splitter;
			textbox.Select(textbox.Text.Length, 0);
		}

		public static void AddTextTextBox(string text, TextBox inputText)
		{
			inputText.Focus(Windows.UI.Xaml.FocusState.Programmatic); //got focus for textbox
			var x = inputText.SelectionStart; //временное запоминание
			inputText.Text = inputText.Text.Insert(inputText.SelectionStart, text);
			inputText.SelectionStart = x + text.Length; // set caret in the end of pasted
		}

		public static void ResizeFontTextBox(TextBox textBox)
		{
			//if (x.Height == double.NaN)
			var statView = ApplicationView.GetForCurrentView();
			if (statView.IsFullScreen)
			{
				textBox.TextWrapping = TextWrapping.Wrap;
				textBox.FontSize = textBox.ActualHeight * 0.35;
			}
			else
			{
				textBox.TextWrapping = TextWrapping.Wrap;
				textBox.FontSize = textBox.ActualHeight * 0.35;
			}
		}

		public static void ScaleText(ComboBox element, double newHeight)
		{
			element.FontSize = (newHeight) * 0.5;
		}
	}
}
