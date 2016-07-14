using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace NumberConverter
{
    class KeyboardButtonBuilder
    {
	    public KeyboardButtonBuilder()
	    {
		    
	    }
	    public Object Content { get; set; }
	    public Visibility Visibility { get; set; }
	    public bool IsTabStop { get; set; }
	    public double MinWidth { get; set; }
	    public double MinHeight { get; set; }
	    public double MaxWidth { get; set; }
		public double MaxHeight { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public VerticalAlignment VerticalAlignment { get; set; }
		public HorizontalAlignment HorizontalAlignment { get; set; }
		public VerticalAlignment VerticalContentAlignment { get; set; }
		public Style Style { get; set; }
		public RoutedEventHandler OnClick { get; set; }
		/// <summary>
		/// Need set only width
		/// </summary>
		public bool HeighLikeWidth { get; set; }

	    Button CreateButton()
	    {
		    var button = new Button();
		    button.MaxWidth = MaxWidth;
		    button.Width = Width;
		    if (HeighLikeWidth)
		    {
				button.MaxHeight = MaxWidth;
				button.Height = Width;
		    }
		    else
		    {
				button.MaxHeight = MaxHeight;
				button.Height = Height;
		    }
		    button.Style = Style;
		    button.MinHeight = MinHeight;
		    button.MinWidth = MinWidth;
		    button.Content = Content;
		    button.Visibility = Visibility;
		    return button;
	    }
    }
}
