using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace testConvert
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			//Output.Text = Converter.Converter.ConvertTo(int.Parse(From.Text), str.Text, int.Parse(To.Text);
		}

		private void str_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (From != null || To != null)
				Output.Text = Converter.Converter.ConvertTo(uint.Parse(From.Text), str.Text, uint.Parse(To.Text));
		}
	}
}
