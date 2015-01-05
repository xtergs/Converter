using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NumberConverter.Annotations;
using Converter;
using Converter = Converter.Converter;

namespace NumberConverter
{
	public class InputField :INotifyPropertyChanged
	{
		private string input;
		private int inputeBaseIndex;
		private ObservableCollection<int> bases;

		public InputField()
		{
			Input = "";
			InputeBaseIndex = 0;

			
		}

		public string Input
		{
			get { return input; }
			set
			{
				if (value == input) return;
				input = value;
				OnPropertyChanged();
			}
		}

		public int InputeBaseIndex
		{
			get { return inputeBaseIndex; }
			set
			{
				if (value == inputeBaseIndex) return;
				inputeBaseIndex = value;
				OnPropertyChanged();
				InputBase = 0;
			}
		}

		public int InputBase
		{
			get { return Bases[InputeBaseIndex]; }
			set { OnPropertyChanged(); }
		}

		public ObservableCollection<int> Bases
		{
			get
			{
				if (bases == null)
				{
					bases = new ObservableCollection<int> {2, 8, 10, 16, 25, 36};
				}
				return bases;
			}
			set
			{
				if (Equals(value, bases)) return;
				bases = value;
				OnPropertyChanged();
			}
		}

		public ActionCommand AddNewBase
		{
			get
			{
				return new ActionCommand(x =>
				{
					if (!(x is int))
						return;
					int bas = (int) x;
					if (!Bases.Contains(bas))
					{
						Bases.Add(bas);
						InputeBaseIndex = Bases.Count - 1;
					}
					else
					{
						InputeBaseIndex = Bases.IndexOf(bas);
					}
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
	}
	public class ConverterControllerManyOutput : ConverterControllerBase
	{
		private ObservableCollection<InputField> outputs;


		public ObservableCollection<InputField> Outputs
		{
			get
			{
				if (outputs == null)
				{
					outputs = new ObservableCollection<InputField>();
					outputs.Add(new InputField());
				}
				return outputs;
			}
			set
			{
				if (Equals(value, outputs)) return;
				outputs = value;
				OnPropertyChanged();
			}
		}

		public override void Convert()
		{
			if (String.IsNullOrEmpty(Input.Input))
				return;
			try
			{
				global::Converter.Converter.Accurancy = Settings.Precise;
				for (int i = 0; i < Outputs.Count; i++)
					Outputs[i].Input = global::Converter.Converter.ConvertTo((uint)Input.InputBase, Input.Input, (uint)Outputs[i].InputBase);

			}
			catch (Exception e)
			{

				Outputs[0].Input = e.Message;
			}
		}
	}

	public class ConverterController : ConverterControllerBase
	{
		private InputField outputs;


		public InputField Outputs
		{
			get
			{
				if (outputs == null)
				{
					outputs = new InputField();
				}
				return outputs;
			}
			set
			{
				if (Equals(value, outputs)) return;
				outputs = value;
				OnPropertyChanged();
			}
		}

		public override void Convert()
		{
			if (String.IsNullOrEmpty(Input.Input))
				return;
			try
			{
				global::Converter.Converter.Accurancy = Settings.Precise;
				Outputs.Input = global::Converter.Converter.ConvertTo((uint)Input.InputBase, Input.Input, (uint)Outputs.InputBase);

			}
			catch (Exception e)
			{

				Outputs.Input = e.Message;
			}
		}
	}
}
