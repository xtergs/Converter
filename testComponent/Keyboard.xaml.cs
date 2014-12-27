using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace NumberConverter
{
	public sealed partial class Keyboard : UserControl
	{
		private Style styleButton;

		public Keyboard()
		{
			this.InitializeComponent();
			MaxButtonHeight = 1000;
			MaxButtonWidth = 1000;
			this.panel = Buttons;
			VisibleButtonCount = 0;
			//StyleButton = (Application.Current.Resources["ButtonStyle1"]) as Style;
			CreateKeyboard(this.panel, null);
		}

		public Style StyleButton
		{
			get { return styleButton; }
			set
			{
				styleButton = value;
				for (int i = 0; i < panel.Children.Count; i++)
					((Button) panel.Children[i]).Style = styleButton;
			}
		}

		private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			SetVisibleButton(VisibleButtonCount,true);
		}
	}
}
