using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Numerics;

namespace NumberConverter
{
	public class LongDouble
	{
		string integer = "0";
		string fraction = "0";

		public string Integer
		{
			get { return integer; }
			set
			{
				if (value.Length > 0 && value[0] == '-')
				{
					value = value.Substring(1);
					IsMinus = true;
				}
				else
					IsMinus = false;
				value = new string(value.SkipWhile((char a) => { return a == '0'; }).ToArray<char>());
				if (value == "")
				{
					integer = "0";
					return;
				}
				integer = value;
			}
		}

		public BigInteger IntegerBig { get { return BigInteger.Parse(integer); } }
		public decimal FractionDec { get { return decimal.Parse("0" + splitter + Fraction); } }

		bool IsMinus { get; set; }

		static string splitter = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

		public LongDouble ()
		{
			
		}

		public LongDouble(string str)
		{
			int indexDot = str.IndexOf(splitter);
			if (indexDot >= 0)
			{
				Integer = str.Substring(0, indexDot);
				if (indexDot + 1 >= str.Length)
					Fraction = "0";
				else
					Fraction = str.Substring(indexDot + 1);
			}
			else
				Integer = str;
			
		}
		public LongDouble(string integ, string fract)
		{
			Integer = integ;
			Fraction =fract;
		}

		public static LongDouble operator +(LongDouble first, LongDouble second)
		{
			LongDouble result = new LongDouble();
			var firstFraction = first.FractionDec;
			var seconFraction = second.FractionDec;
			firstFraction += seconFraction;
			int buffer = (int)firstFraction;
			firstFraction -= buffer;
			result.Fraction = firstFraction.ToString().Substring(2);
			var firstInteg = first.IntegerBig;
			var secondInteg = second.IntegerBig;
			firstInteg += secondInteg + buffer;
			result.Integer = firstInteg.ToString();
			return result;
		}

		public static LongDouble operator -(LongDouble first, LongDouble second)
		{
			LongDouble result = new LongDouble();
			var firstFraction = first.FractionDec;
			var seconFraction = second.FractionDec;
			firstFraction -= seconFraction;
			//int buffer = 0;
			if (firstFraction < 0)
				result.IsMinus = true;
			//firstFraction -= buffer;
			var tempStr = firstFraction.ToString();
			result.Fraction = tempStr.Substring(tempStr.IndexOf(Splitter)+1);
			var firstInteg = first.IntegerBig;
			var secondInteg = second.IntegerBig;
			firstInteg -= secondInteg;
			result.Integer = firstInteg.ToString();
			return result;
		}

		public static LongDouble operator /(LongDouble first, LongDouble second)
		{
			LongDouble result = new LongDouble();
			var firstFraction = first.FractionDec;
			var seconFraction = second.FractionDec;
			firstFraction *= seconFraction;
			int buffer = (int)firstFraction;
			firstFraction -= buffer;
			result.Fraction = firstFraction.ToString().Substring(2);
			var firstInteg = first.IntegerBig;
			var secondInteg = second.IntegerBig;
			firstInteg *= secondInteg;
			firstInteg += buffer;
			result.Integer = firstInteg.ToString();
			return result;
		}

		public static LongDouble operator *(LongDouble first, LongDouble second)
		{
			LongDouble result = new LongDouble();
			var firstFraction = first.FractionDec;
			var seconFraction = second.FractionDec;
			firstFraction *= seconFraction;
			int buffer = (int)firstFraction;
			firstFraction -= buffer;
			result.Fraction = firstFraction.ToString().Substring(2);
			var firstInteg = first.IntegerBig;
			var secondInteg = second.IntegerBig;
			firstInteg *= secondInteg;
			firstInteg += buffer;
			result.Integer = firstInteg.ToString();
			return result;
		}

		public static string Splitter { get { return splitter; } }

		bool isLong;

		public bool IsLong { get { return isLong; } }
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			if (isLong)
				throw new NotImplementedException();
			else
			{
				if ((integer != "0" || fraction != "0")&& IsMinus)
					builder.Append("-");
				if (fraction != "0" && fraction != "")
					builder.Append( integer.ToString().ToUpper() + splitter + fraction.ToString().ToUpper());
				else
					builder.Append( integer.ToString().ToUpper());
				return builder.ToString();
			}
		}

		public string Fraction
		{
			get { return fraction; }
			set
			{
				if (value.IndexOf(splitter) >=0)
					value = value.Substring(value.IndexOf(splitter)+1);
				value = new string(value.Reverse().SkipWhile((char a) => { return a == '0'; }).Reverse().ToArray<char>()); ;
				if (value == "")
					fraction = "0";
				else
					fraction = value;

			}
		}
	}
}
