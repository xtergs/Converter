using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NumberConverter.Annotations;


namespace NumberConverter
{
	public class ConverterControllerBase : INotifyPropertyChanged
	{
		private InputField input;

		public SettingsModelView Settings
		{
			get { return SettingsModelView.Settings; }
		}

		public InputField Input
		{
			get { return input ?? (input = new InputField()); }
			set
			{
				if (value == null)
					return;
				if (Equals(value, input)) return;
				input = value;
				OnPropertyChanged();
			}
		}

		public virtual void Convert()
		{
			
		}

		private string BackSpace(string str)
		{
			if (String.IsNullOrEmpty(str))
				return "";
			str = str.Remove(str.Length - 1);
			return str;
		}

		public ActionCommand BackSpaceCommand
		{
			get
			{
				return new ActionCommand(x =>
				{
					Input.Input = BackSpace(Input.Input);
				}, o =>
				{
					if (String.IsNullOrEmpty(Input.Input))
						return false;
					return true;
				});
			}
		}

		public ActionCommand ClearCommand
		{
			get
			{
				return new ActionCommand(x =>
				{
					x = "";
				}, o =>
				{
					if (o!= null && o is string)
						if (String.IsNullOrEmpty((string) o))
							return false;
					return true;
				});
			}
		}

		public ActionCommand DotCommand
		{
			get
			{
				return new ActionCommand(x=>
				x += LongDouble.Splitter, o =>
				{
					if (!(o is string))
						return false;
					string str = (string) o;
					if (String.IsNullOrEmpty(str))
						return true;
					if (str.IndexOf(LongDouble.Splitter) < 0)
						return false;
					return true;

				});
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		public virtual ActionCommand ConvertCommand
		{
			get
			{
				return new ActionCommand(x => Convert());
			}
		}
	}
}
