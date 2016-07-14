using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace NumberConverter.Components
{
	public partial class Keyboard : ContentView
	{


		private int totalRows = 0;
		private int totalColumns  = 0;
		private int visibleRows = 0;

		public const string letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private List<Button> buttons = new List<Button>(letters.Length); 

		public Keyboard()
		{
			InitializeComponent();
			CreateKeyboard();
		}

		//static Color col = new Color();
		private int currentVisibleCount = 0;
		public int VisibleCount { get { return (int) GetValue(VisibleCountProperty); } set
		{
			SetValue(VisibleCountProperty, value);
		} }

	    public bool IsCanDelete
	    {
	        get { return (bool) GetValue(IsCanDeleteProperty); }
            set { SetValue(IsCanDeleteProperty, value); }
	    }

        public bool IsDotEnabled
        {
            get { return (bool)GetValue(IsDotEnabledProperty); }
            set { SetValue(IsDotEnabledProperty, value); }
        }

        public static readonly BindableProperty VisibleCountProperty = BindableProperty.Create(nameof(VisibleCount), typeof(int), typeof(Keyboard), defaultValue:36,
			propertyChanging: VisibleCountChanging, propertyChanged: VisibleCountChanged);

	    public static readonly BindableProperty IsCanDeleteProperty = BindableProperty.Create(nameof(IsCanDelete),
	        typeof (bool), typeof (Keyboard), defaultValue: true);

	    public static readonly BindableProperty IsDotEnabledProperty = BindableProperty.Create(nameof(IsDotEnabled),
	        typeof (bool), typeof (Keyboard), defaultValue: true);

		private static void VisibleCountChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var el = (Keyboard)bindable;
			el.ResizeButton();
		}

		private static void VisibleCountChanging(BindableObject bindable, object oldValue, object newValue)
		{
			if ((int) newValue < 2 || (int) newValue > 36)
				return;
			bindable.SetValue(VisibleCountProperty, newValue);
		}

		int marg_bot = 0;
		int marg_top = 0;


		Button newButton(string text)
		{
			var but =  new Button()
			{
				Text = text,
				IsVisible = true,
				FontSize = 10,
				BackgroundColor = Color.Gray,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Margin = new Thickness(0, marg_top, 0, marg_bot),
			};

            but.Clicked += ButtonClickedKeyboard;
		    return but;

		}

	    private void ButtonClickedKeyboard(object sender, EventArgs eventArgs)
	    {
	        var but = sender as Button;
            if (but.Text == "<-")
                OnDeleteButtonClicked();
            else
	            OnButtonClicked(but.Text[0]);
	    }

	    public void CreateKeyboard()
		{

			Button temp;

#if WINDOWS_PHONE_APP
			marg_bot = -6;
			marg_top = -15;
#endif
			//col = Color.FromArgb(0xff, 0x3f, 0x3f, 0x3f);

			for (int i = 0; i < 36; i++)
			{
				temp = newButton(letters[i].ToString());
				//temp.Style = style;
				//temp.Click += Button_Click_1;
				buttons.Add(temp);
			}
			temp = newButton(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
            //temp.Style = style;
            //temp.Click += Button_Click_Dot;
            temp.SetBinding(Button.IsEnabledProperty, nameof(IsDotEnabled), BindingMode.TwoWay);
            temp.BindingContext = this;
            buttons.Add(temp);
			temp = newButton("<-");
            temp.SetBinding(Button.IsEnabledProperty, nameof(IsCanDelete), BindingMode.TwoWay);
	        temp.BindingContext = this;
			//temp.Style = style;
			buttons.Add(temp);
			currentVisibleCount = buttons.Count - 2;
		}

		// Resize for current height and width, using count current visible elements
		public void ResizeButton()
		{
			ResizeButton(VisibleCount+2);
		}

		// Resize for current height and width
		public void ResizeButton(int countKeys)
		{
			ResizeButton(layout.Height, layout.Width, countKeys);
		}


		public void ResizeButton(double height, double width, int countKeys)
		{
			double maxHeight = height - 10;
			double maxWidth = width - 10;
			double areaForKeyboard = maxHeight * maxWidth;

			double a = Math.Sqrt(areaForKeyboard / countKeys);
			int row = 0;
			int column = 0;
			double h = a;
			double w = a;
			while (row * column < countKeys)
			{
				a -= 5;
				row = (int)(maxHeight / a);
				column = (int)(maxWidth / a);
			}

			h = a;
			while (row * column - countKeys - 1 >= row - 1)
				column--;
			w = maxWidth / column;

			//row = (int)((double)(countKeys / column));
			if ((countKeys / column) - row > 0)
				row++;
			//if ((w) * row <= maxHeight) // marg_bot + marg_top - correction for buttons in WP8.1
			//	h = w;
			//else
			h = (maxHeight) / row;
			//while ((h) * row <= maxHeight + marg_top*row)
			//	h += 5;
			int correction = 0;
#if WINDOWS_PHONE_APP
				correction +=15;
#endif
			for (int i = 0; i < buttons.Count; i++)
			{
				(buttons[i]).HeightRequest = h + correction;
				(buttons[i]).WidthRequest = w;
				//((Button)panel.Children[i]).UpdateLayout();
				(buttons[i]).FontSize = h * 0.5;
			}
			//	((WrapPanel)panel).ItemHeight = h;
			//	((WrapPanel)panel).ItemWidth = w;

			RearangeButtons(row, column, buttons.Take(VisibleCount).Union(buttons.Skip(letters.Length)).ToList());

		}

		void ExtendMatrixLayoutByCount(int rows)
		{
			//var row = new StackLayout[rows];
			for (int i = 0; i < rows; i++)
			{
				layout.Children.Add(new StackLayout()
				{
					Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand,
					Spacing = 0
				});
			}
			totalRows += rows;
		}

		void SetMatrixLayout(int rows)
		{
			if (rows < 0)
				return;
			if (rows > totalRows)
				ExtendMatrixLayoutByCount(rows - totalRows);
			for (int i = 0; i < rows; i++)
				layout.Children[i].IsVisible = true;
			for (int i = rows; i < layout.Children.Count; i++)
				layout.Children[i].IsVisible = false;

			visibleRows = rows;
		}

		void RearangeButtons(int rows, int columns, List<Button> buttons)
		{
			if (rows < 0)
				return;
			if (rows == visibleRows && columns == totalColumns && currentVisibleCount == buttons.Count - 2)
				return;
			SetMatrixLayout(rows);
			for (int i = rows; i < totalRows; i++)
				((StackLayout)layout.Children[i]).Children.Clear();

			int ibutton = 0;

			for (int i = 0; i < rows; i++)
			{
				((StackLayout) layout.Children[i]).Children.Clear();
				for (int j = 0; j < columns && ibutton < buttons.Count; j++)
					((StackLayout) layout.Children[i]).Children.Add(buttons[ibutton++]);
			}
			totalColumns = columns;
			currentVisibleCount = buttons.Count - 2;
		}

		public void HideAll()
		{
			for (int i = 0; i < buttons.Count; i++)
				buttons[i].IsVisible = false;
		}

		public void SetVisibleButton(int count, bool needResize = false)
		{
			if (count < 0)
				return;
			if (count != VisibleCount)
			{
				var temp = buttons;
				for (int i = 0; i < temp.Count - 2 || i < VisibleCount; i++)
					if (i < count)
					{
						temp[i].IsVisible = true;

					}
					else
					{
						temp[i].IsVisible = false;

					}
				VisibleCount = count;
			}
			if (needResize)
				ResizeButton(VisibleCount + 2);
		}

		private void Keyboard_OnSizeChanged(object sender, EventArgs e)
		{
			ResizeButton();
		}

	    public event EventHandler<char> ButtonClicked;
	    public event EventHandler DeleteButtonClicked; 

	    protected virtual void OnButtonClicked(char e)
	    {
	        ButtonClicked?.Invoke(this, e);
	    }


	    protected virtual void OnDeleteButtonClicked()
	    {
	        DeleteButtonClicked?.Invoke(this, EventArgs.Empty);
	    }
	}
}
