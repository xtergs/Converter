using System;
using System.Linq;
using Windows.UI.Xaml.Input;
using NumberConverter.Components;
using NumberConverter.UWP.Render;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Picker), typeof(PickerExtendedRender))]
namespace NumberConverter.UWP.Render
{
    class PickerExtendedRender : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                if (!e.NewElement.GestureRecognizers.Any())
                    return;

                if (e.NewElement.GestureRecognizers.Any(x => x.GetType() == typeof(RightTappRecognizer)))
                {
                    Control.RightTapped += ControlOnRightTapped;
                }

                if (e.NewElement is PickerExtended)
                {
                    (e.NewElement as PickerExtended).FontSizeChanged += NewElementOnSizeChanged;
                }

            }
        }

        private void NewElementOnSizeChanged(object sender, EventArgs eventArgs)
        {
            var pick = Element as PickerExtended;
            Control.FontSize = pick.FontSize;
        }

        private void ControlOnTapped(object sender, TappedRoutedEventArgs tappedRoutedEventArgs)
        {
            foreach (var gesture in this.Element.GestureRecognizers.Where(x => x.GetType() == typeof(RightTappRecognizer)).OfType<RightTappRecognizer>())
            {
                gesture?.OnRightTapp();
            }
        }

        private void ControlOnRightTapped(object sender, RightTappedRoutedEventArgs rightTappedRoutedEventArgs)
        {
            foreach (var gesture in this.Element.GestureRecognizers.Where(x => x.GetType() == typeof(RightTappRecognizer)).OfType<RightTappRecognizer>())
            {
                gesture?.OnRightTapp();
            }
        }

    }
}
