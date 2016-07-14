using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NumberConverter.Components
{
    public class PickerExtended : Picker
    {
        public event EventHandler FontSizeChanged;
        public double FontSize
        {
            get
            {
                return (double) GetValue(FontSizeProperty);
            }
            set { SetValue(FontSizeProperty, value); }
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize),
            typeof (double), typeof (PickerExtended), defaultValue:10.0, propertyChanged: propertyChanged);

        private static void propertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
                PickerExtended pic = bindable as PickerExtended;
                pic.OnFontSizeChanged();
        }

        protected virtual void OnFontSizeChanged()
        {
            FontSizeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
