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
			int value = -1;
			if (e.Key >= VirtualKey.Number0 && e.Key <= VirtualKey.Number9)
			{
				value = (int)e.Key - 48;
				
			}
			if (e.Key >= VirtualKey.NumberPad0 && e.Key <= VirtualKey.NumberPad9)
			{
				value = (int) e.Key - 96;
			}
			if (e.Key >= VirtualKey.A && e.Key <= VirtualKey.Z)
			{
				value = (int) e.Key - 55;
			}
			if (value >= 0 && value < fromBase)
			{
				textbox.Text += Keyboard.letters[value];
				e.Handled = true;
			}

			if (e.Key == VirtualKey.Back)
				Backspace(textbox);
			
			if (e.Key == VirtualKey.Decimal && textbox.Text.IndexOf(LongDouble.Splitter) < 0)
				textbox.Text += LongDouble.Splitter;
			textbox.Select(textbox.Text.Length, 0);
		}

		public static void AddTextTextBox(string text, TextBox inputText)
		{
			//inputText.Focus(Windows.UI.Xaml.FocusState.Programmatic); //got focus for textbox
			//var x = inputText.SelectionStart; //временное запоминание
			//inputText.Text = inputText.Text.Insert(inputText.SelectionStart, text);
			//inputText.SelectionStart = x + text.Length; // set caret in the end of pasted
			//inputText.SelectedText = String.Empty;
			inputText.Text += text;
		}

		public static void ResizeFontTextBox(TextBox textBox, double fontScale)
		{
			//if (x.Height == double.NaN)
			//var statView = ApplicationView.GetForCurrentView();
			//if (statView.IsFullScreen)
			//{
				textBox.TextWrapping = TextWrapping.Wrap;
				textBox.FontSize = textBox.ActualHeight * fontScale;
			//}
			//else
			//{
			//	textBox.TextWrapping = TextWrapping.Wrap;
			//	textBox.FontSize = textBox.ActualHeight * 0.35;
			//}
		}

		public static void ScaleText(ComboBox element, double newHeight)
		{
			element.FontSize = (newHeight) * 0.4;
		}
	}
}
