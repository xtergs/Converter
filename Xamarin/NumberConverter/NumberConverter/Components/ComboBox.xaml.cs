using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NumberConverter.Components
{
    public partial class ComboBox : ContentView
    {
        public ComboBox()
        {
            //InitializeComponent();
        }

        public bool IsOpen
        {
            get { return (bool) base.GetValue(isOpenProperty); }
            set { base.SetValue(isOpenProperty, value); }
        }

        public static BindableProperty isOpenProperty = BindableProperty.Create(nameof(IsOpen), typeof (bool),
            typeof (ComboBox), false);
    }
}
