using System;
using System.Collections.Generic;
using System.Text;

namespace NumberConverter.Exceptions
{
    public class LargDigit : Exception
    {
	    public LargDigit(string message)
			:base(message)
	    {
		   
	    }

		public LargDigit()
			:base()
		{ }
    }
}
