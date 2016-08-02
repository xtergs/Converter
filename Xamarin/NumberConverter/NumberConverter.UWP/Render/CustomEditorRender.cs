using NumberConverter.UWP.Render;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly:ExportRenderer(typeof(Editor), typeof(CustomEditorRender))]
namespace NumberConverter.UWP.Render
{
    class CustomEditorRender : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            Control.ApplyTemplate();
            
        }
    }
}
