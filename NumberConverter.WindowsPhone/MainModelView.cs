using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberConverter
{
	class MainModelView
	{
		private ConverterController converterController;

		private CalculatorController calculatorController;

		public CalculatorController CalculatorController
		{
			get
			{
				if (calculatorController == null)
					calculatorController = new CalculatorController();
				return calculatorController;
			}
			set { calculatorController = value; }
		}

		public ConverterController ConverterController
		{
			get
			{
				if (converterController == null)
					converterController = new ConverterController();
				return converterController;
			}
			set { converterController = value; }
		}
	}
}
