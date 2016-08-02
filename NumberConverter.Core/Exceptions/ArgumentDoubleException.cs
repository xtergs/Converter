using System;
using System.Collections.Generic;
using System.Text;

namespace NumberConverter.Exceptions
{
    public class ArgumentDoubleException : Exception
    {
	    public ArgumentDoubleException(string Message)
			:base(Message)
	    {
		    
	    }
    }
}
