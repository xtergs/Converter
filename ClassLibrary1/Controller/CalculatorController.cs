using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace NumberConverter
{
	public class CalculatorController : ConverterController
	{
		private InputField input2;
		private int operation;

		public readonly Dictionary<string, int> OperationStr = new Dictionary<string, int>()
		{
			{"+", 0},
			{"-", 1},
			{"x", 2},
			{"*", 2},
			{"/", 3},
			{"^", 4},
			{"~", 5},
			{"<<", 6},
			{">>", 7},
			{"|", 8},
			{"&", 9},
			{"xor", 10},
		}; 

		public InputField Input2
		{
			get
			{
				if (input2 == null)
					input2 = new InputField();
				return input2;
			}
			set
			{
				if (Equals(value, input2)) return;
				input2 = value;
				OnPropertyChanged();
			}
		}

		public string OpStr
		{
			set { Operation = OperationStr[value]; }
		}

		public int Operation
		{
			get { return operation; }
			set
			{
				if (value == operation) return;
				operation = value;
				OnPropertyChanged();
				CalculateCommand.Execute();
			}
		}

		

		private string Operations(string In, string Out)
		{
			var slag = Converter.Converter.ConvertTo(Input.InputBase, new LongDouble(In), 10);
			//double firstslagD = double.Parse(slag);
			var slag2 = Converter.Converter.ConvertTo(Input2.InputBase, new LongDouble(Out), 10);
			//double secondslagD = double.Parse(slag);
			switch (Operation)
			{
					//+
				case 0:
					slag = (slag + slag2);
					break;
					//-
				case 1:
					slag = (slag - slag2);
					break;
					//*
				case 2:
					slag = (slag * slag2);
					break;
					// /
				case 3:
					{
						if (slag2.Integer == "0" && slag2.Fraction == "0")
						{
							var resourceLoader = new ResourceLoader();
							return resourceLoader.GetString("DivideByZero");
							//return "You can not divide by zero";
						}
						slag = (slag / slag2);
						break;
					}
				case 4: // ^
					if (slag2.IsDouble)
					{
						var resourceLoader = new ResourceLoader();
						return resourceLoader.GetString("PowInteger");
					}
					if (slag2.IntegerBig > int.MaxValue/2)
					{
						var resourceLoader = new ResourceLoader();
						return resourceLoader.GetString("IntegerMax") + " " + int.MaxValue/2;
					}
					slag = slag.Pow(int.Parse(slag2.Integer));
					break;
				case 5: //~
					;
					break;
				case 6: //<<
					if (slag2.IsDouble || slag.IsDouble)
					{
						var resourceLoader = new ResourceLoader();
						return resourceLoader.GetString("OperateOnlyInteger");
					}
					if (slag2.IntegerBig > int.MaxValue)
					{
						var resourceLoader = new ResourceLoader();
						return resourceLoader.GetString("IntegerMax") + " " + int.MaxValue;
					}
					slag = slag << int.Parse(slag2.Integer);
					break;
				case 7: //>>
					if (slag2.IsDouble || slag.IsDouble)
					{
						var resourceLoader = new ResourceLoader();
						return resourceLoader.GetString("OperateOnlyInteger");
					}
					if (slag2.IntegerBig > int.MaxValue)
					{
						var resourceLoader = new ResourceLoader();
						return resourceLoader.GetString("IntegerMax") + " " + int.MaxValue;
					}
					slag = slag >> int.Parse(slag2.Integer);
					break;
				case 8: //|
					if (slag2.IsDouble || slag.IsDouble)
					{
						var resourceLoader = new ResourceLoader();
						return resourceLoader.GetString("OperateOnlyInteger");
					}
					if (slag2.IntegerBig > int.MaxValue || slag.IntegerBig > int.MaxValue)
					{
						var resourceLoader = new ResourceLoader();
						return resourceLoader.GetString("IntegerMax") + " " + int.MaxValue;
					}
					slag = slag | slag2;
					break;
				case 9: //&
					if (slag2.IsDouble || slag.IsDouble)
					{
						var resourceLoader = new ResourceLoader();
						return resourceLoader.GetString("OperateOnlyInteger");
					}
					if (slag2.IntegerBig > int.MaxValue || slag.IntegerBig > int.MaxValue)
					{
						var resourceLoader = new ResourceLoader();
						return resourceLoader.GetString("IntegerMax") + " " + int.MaxValue;
					}
					slag = slag & slag2;
					break;
				case 10: //xor
					if (slag2.IsDouble || slag.IsDouble)
					{
						var resourceLoader = new ResourceLoader();
						return resourceLoader.GetString("OperateOnlyInteger");
					}
					if (slag2.IntegerBig > int.MaxValue || slag.IntegerBig > int.MaxValue)
					{
						var resourceLoader = new ResourceLoader();
						return resourceLoader.GetString("IntegerMax") + " " + int.MaxValue;
					}
					slag = LongDouble.XOR(slag, slag2);
					break;
			}
			return Converter.Converter.ConvertTo(10, slag, Outputs.InputBase).ToString();
		}

		private void Calculate()
		{
			if (!String.IsNullOrWhiteSpace(Input.Input) && !String.IsNullOrWhiteSpace(Input2.Input))
			{
				Converter.Converter.Accurancy = SettingsModelView.Settings.Precise;
				var resourceLoader = new ResourceLoader();
				if (!Converter.Converter.Validate((uint) Input.InputBase, Input.Input))
				{
					Outputs.Input = resourceLoader.GetString("LargeDigit");
					return;
				}
				if (!Converter.Converter.Validate((uint)Input2.InputBase, Input2.Input))
				{
					Outputs.Input = resourceLoader.GetString("LargeDigit");
					return;
				}
				Outputs.Input = Operations(Input.Input, Input2.Input);
			}
		}

		public ActionCommand Plus
		{
			get
			{
				return new ActionCommand(x =>
				{
					OpStr = "+";
					Calculate();
				});
			}
		}

		public ActionCommand Divide
		{
			get
			{
				return new ActionCommand(x =>
				{
					OpStr = "/";
					Calculate();
				}, p =>
				{
					
					return true;
				});
			}
		}

		public ActionCommand CalculateCommand
		{
			get
			{
				
				return new ActionCommand(x => Calculate());
			}
		}
	}
}
