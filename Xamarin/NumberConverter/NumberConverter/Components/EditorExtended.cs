using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace NumberConverter.Components
{
    public class RightTappRecognizer : Element, IGestureRecognizer
    {
        public event EventHandler RightTapp;

        public virtual void OnRightTapp()
        {
            RightTapp?.Invoke(this, EventArgs.Empty);
        }
    }
}
