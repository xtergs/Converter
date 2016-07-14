using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using NumberConverter.Components;
using Xamarin.Forms;
using XLabs.Platform.Device;

namespace NumberConverter
{
   
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        private Editor lastFocusedEditor;
        private int _fromBase;
        private int _toBase;
        private bool _isCanDelete;
        private bool _isDotEnabled;
        private int _selectedIndex;
        private int _selectedIndex2;
        private string _firstFieldInputText;

        public string FirstFieldInputText
        {
            get { return _firstFieldInputText; }
            set
            {
                value = value.ToUpper();
                
                _firstFieldInputText = value;
                OnPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value; 
                OnPropertyChanged();
                if (value > 0)
                    FromBase = lsit[value];
            }
        }

        public int SelectedIndex2
        {
            get { return _selectedIndex2; }
            set
            {
                _selectedIndex2 = value;
                OnPropertyChanged();
                if (value > 0)
                    ToBase = lsit[value];
            }
        }

        public bool IsCanDelete
        {
            get { return _isCanDelete; }
            set
            {
                _isCanDelete = value;
                OnPropertyChanged();
            }
        }

        public bool IsDotEnabled
        {
            get { return _isDotEnabled; }
            set
            {
                _isDotEnabled = value;
                OnPropertyChanged();
            }
        }

        public int FromBase
        {
            get { return _fromBase; }
            set
            {
                if (_fromBase == value)
                    return;
                _fromBase = value;
                OnPropertyChanged();
                Recalculate();
            }
        }

        public int ToBase
        {
            get { return _toBase; }
            set
            {
                if (_toBase == value)
                    return;
                _toBase = value;
                OnPropertyChanged();
                Recalculate();
            }
        }

        public List<int> lsit { get; set; } = new List<int>() {2, 8, 10, 16,};

        public List<string> lsit1 => lsit.Select(x => x.ToString()).ToList();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            _fromBase = 10;
            _toBase = 2;
            IsCanDelete = false;
            lastFocusedEditor = entry1;
            foreach (var s in lsit1)
            {

                picker.Items.Add(s);
                picker2.Items.Add(s);
            }
        }

        void Recalculate()
        {
            if (string.IsNullOrWhiteSpace(lastFocusedEditor.Text))
            {
                IsCanDelete = false;
                return;
            }
            IsCanDelete = true;
            if (
                lastFocusedEditor.Text.Contains(
                    CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))
                IsDotEnabled = false;
            else
                IsDotEnabled = true;
            entry2.Text = Converter.Converter.ConvertTo((uint)FromBase, lastFocusedEditor.Text, (uint)ToBase);
        }

        private void Keyboard_OnButtonClicked(object sender, char e)
        {
            entry1.Text += e;
            Recalculate();
        }

        private void Keyboard_OnDeleteButtonClicked(object sender, EventArgs e)
        {
            lastFocusedEditor.Text = lastFocusedEditor.Text.Substring(0, lastFocusedEditor.Text.Length - 1);
            Recalculate();   
        }

        private void Entry1_OnFocused(object sender, FocusEventArgs e)
        {
            
        }

        private void Stepper1_OnClicked(object sender, EventArgs e)
        {
           
            picker.IsVisible = true;

        }

        private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            FromBase = lsit[picker.SelectedIndex];
            picker.IsVisible = false;
        }

        private async void RightTappRecognizer_OnRightTapp(object sender, EventArgs e)
        {
            var res = await DisplayActionSheet("Select a base", "Cancel", null, Enumerable.Range(2, 36).Select(x => x.ToString()).ToArray());
            if (res == "Cancel")
                return;
            var pic = (Picker) (sender as RightTappRecognizer).Parent;
            var bas = int.Parse(res);
            if (!pic.Items.Contains(bas.ToString()))
            {
                pic.Items.Add(bas.ToString());
                lsit.Add(bas);
            }
            pic.SelectedIndex = pic.Items.IndexOf(bas.ToString());
        }

        private void EditorSizeChanged(object sender,EventArgs e)
        {

            var editor = sender as Editor;
            editor.FontSize = editor.Height*0.3;
            picker.FontSize = editor.Height*0.3;
        }


        private void Entry1_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
    }
}
