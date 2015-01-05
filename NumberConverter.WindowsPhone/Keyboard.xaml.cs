using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
			CreateKeyboard(this.panel, StyleButton);
		}

		public Style StyleButton
		{
			get { return styleButton; }
			set
			{
				styleButton = value;
				for (int i = 0; i < buttonsList.Count; i++)
					buttonsList[i].Style = styleButton;
			}
		}

		private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			SetVisibleButton(VisibleButtonCount,true);
		}
	}
}
