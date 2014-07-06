using System;
using System.Collections.Generic;
using System.Text;

namespace NumberConverter
{
	class LongDouble
	{
		ulong integer;
		ulong fraction;

		static string splitter = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

		public static string Splitter { get { return splitter; } }

		bool isLong;
		public string ToString()
		{
			if (isLong)
				throw new NotImplementedException();
			else
				return integer.ToString() + splitter + fraction.ToString();
		}
	}
}
