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

		public InputField Input
		{
			get
			{
				if (input == null)
					input = new InputField();
				return input;
			}
			set
			{
				if (Equals(value, input)) return;
				input = value;
				OnPropertyChanged();
			}
		}

		public virtual void Convert()
		{
			
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		public ActionCommand ConvertCommand
		{
			get
			{
				return new ActionCommand(x => Convert());
			}
		}
	}
}
