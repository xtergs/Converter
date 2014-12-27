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
				if (String.IsNullOrWhiteSpace(value))
				{
					integer = "0";
					return;
				}
				integer = value;
			}
		}

		public BigInteger IntegerBig { get { return BigInteger.Parse(integer); } }
		public decimal FractionDec { get { return decimal.Parse("0" + splitter + Fraction); } }

		public bool IsMinus { get; set; }

		public bool IsDouble
		{
			get
			{
				if (Fraction != "0" && Fraction.Length > 0) return true;
				return false;
			}
		}

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
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");
			var result = new LongDouble();

			//Represent double without dot
			string strf = first.Integer;
			if (first.IsDouble)
				strf += first.Fraction;
			string strs = second.Integer;
			if (second.IsDouble)
				strs += second.Fraction;
			int sub = Math.Abs(first.Fraction.Length - second.Fraction.Length);
			int sub2 = sub;
			if (first.IsDouble)
			{
				int subb = 0;
				if (second.IsDouble)
				{
					subb = first.Fraction.Length - second.Fraction.Length;
					if (subb > 0)
					while (subb > 0)
					{
						strs += "0";
						subb--;
					}
					if (subb < 0)
						while (subb<0)
						{
							strf += "0";
							subb++;
						}
				}
				else
				{
					subb = first.Fraction.Length;
					while (subb > 0)
					{
						strs += "0";
						subb--;
					}
				}
			}

			//while (strf.Length < strs.Length)
			//{
			//	strf += "0";
			//}
			//while (strs.Length < strf.Length)
			//{
			//	strs += "0";
			//}
			var firstInteg = BigInteger.Parse(strf);
			var secondInteg = BigInteger.Parse(strs);

			BigInteger remainder;
			//firstInteg /= secondInteg;
			firstInteg = BigInteger.DivRem(firstInteg, secondInteg, out remainder);
			StringBuilder fract = new StringBuilder();
			BigInteger buf;
			int loop = 20;
			while (remainder > 0 && loop > 0)
			{
				remainder *= 10;
				buf = BigInteger.DivRem(remainder, secondInteg, out remainder);
				fract.Append(buf);
				loop--;
			}
			//secondInteg = BigInteger.DivRem(remainder, secondInteg, out remainder);
			////Result = 0
			//if (firstInteg == 0)
			//	return result;

			strf = firstInteg.ToString();
			if (fract.Length > 0)
				strs = fract.ToString();

			// Count digits in fraction
			// 2.0 is not double
			// 2.03 is double
			int countDigFraction = 0;
			if (first.IsDouble)
				countDigFraction += first.Fraction.Length;
			if (second.IsDouble)
				countDigFraction += second.Fraction.Length;

			//Count of digits in the fraction > length of the result
			//while (countDigFraction > strf.Length)
			//{
			//	strf = "0" + strf;
			//}

			// 02 and countDigFraction = 2 is 0.02
			//if (countDigFraction == strf.Length)
			//{
			//	result.Integer = "0";
			//	result.Fraction = strf;
			//	return result;
			//}

			result.Fraction = strs;
			result.Integer = strf;
			return result;
		}

		public static LongDouble operator *(LongDouble first, LongDouble second)
		{
			if (first == null) 
				throw new ArgumentNullException("first");
			if (second == null) 
				throw new ArgumentNullException("second");
			var result = new LongDouble();
			var firstInteg =BigInteger.Parse(first.integer + first.Fraction);
			var secondInteg = BigInteger.Parse(second.Integer + second.Fraction);
			//decimal firstFraction = first.FractionDec;
			//decimal seconFraction = second.FractionDec;
			firstInteg *= secondInteg;
			if (firstInteg == 0)
				return new LongDouble();
			string str = firstInteg.ToString();
			int countDigFraction = (first.Fraction.Length + second.Fraction.Length);
			//count of digits in the fraction > length of the result
			while (countDigFraction > str.Length)
			{
				str = "0" + str;
			}
			if (countDigFraction == str.Length)
			{
				result.Integer = "0";
				result.Fraction = str;
				return result;
			}

			result.Fraction = str.Substring(str.Length - (first.Fraction.Length + second.Fraction.Length));
			result.Integer = str.Substring(0, str.Length - (first.Fraction.Length + second.Fraction.Length));
			//int buffer = (int)firstFraction;
			//firstFraction -= buffer;
			//result.Fraction = firstFraction.ToString().Substring(2);
			
			//firstInteg *= secondInteg;
			//firstInteg += buffer;
			//result.Integer = firstInteg.ToString();
			return result;
		}

		public static bool operator <(LongDouble value1, LongDouble value2)
		{
			if (value1.IsMinus && !value2.IsMinus)
				return true;
			if (!value1.IsMinus && value2.IsMinus)
				return false;

			//Both are positive
			if (!value1.IsMinus && !value2.IsMinus)
			{
				//Integer
				if (value1.IntegerBig < value2.IntegerBig)
					return true;

				//Fraction
				if (value1.FractionDec < value2.FractionDec)
					return true;
				
				//if both are equal or value1 > value2
				return false;
			}
			else //Both are negative
			{
				//Integer
				if (value1.IntegerBig > value2.IntegerBig)
					return true;

				//Fraction
				if (value1.FractionDec > value2.FractionDec)
					return true;

				//if both are equal or -value1 < -value2
				return false;
			}
		}

		public static bool operator >(LongDouble value1, LongDouble value2)
		{
			return !(value1 < value2);
		}

		public static LongDouble operator &(LongDouble value1, LongDouble value2)
		{
			if (value1 == null)
				throw new ArgumentNullException("value1");
			if (value2 == null)
				throw new ArgumentNullException("value2");
			if (value2.IsDouble || value1.IsDouble)
				throw new Exception("can operate only on integer");

			var result = new LongDouble();
			result.Integer = (value1.IntegerBig & value2.IntegerBig).ToString();

			return result;
		}

		public static LongDouble operator |(LongDouble value1, LongDouble value2)
		{
			if (value1 == null)
				throw new ArgumentNullException("value1");
			if (value2 == null)
				throw new ArgumentNullException("value2");
			if (value2.IsDouble || value1.IsDouble)
				throw new Exception("can operate only on integer");

			var result = new LongDouble();
			result.Integer = (value1.IntegerBig | value2.IntegerBig).ToString();

			return result;
		}

		public static LongDouble operator <<(LongDouble value1, int value2)
		{
			if (value1 == null)
				throw new ArgumentNullException("value1");
			if (value1.IsDouble)
				throw new Exception("can operate only on integer");

			var result = new LongDouble();
			result.Integer = (value1.IntegerBig << value2).ToString();

			return result;
		}

		public static LongDouble operator >>(LongDouble value1, int value2)
		{
			if (value1 == null)
				throw new ArgumentNullException("value1");
			if (value1.IsDouble)
				throw new Exception("can operate only on integer");

			var result = new LongDouble();
			result.Integer = (value1.IntegerBig >> value2).ToString();

			return result;
		}

		public static LongDouble operator ~(LongDouble value1)
		{
			if (value1 == null)
				throw new ArgumentNullException("value1");
			if (value1.IsDouble)
				throw new Exception("can operate only on integer");

			var result = new LongDouble();
			result.Integer = (~value1.IntegerBig).ToString();

			return result;
		}

		public LongDouble Pow(int value1)
		{
			if (value1 == 0)
				return new LongDouble("1");
			var result = new LongDouble(this.ToString());
			if (value1 > 0)
				result = Pow(this, value1);
			else
			{
				value1 *= -1;
				for (int i = 1; i < value1; i++)
					result *= this;
				result = new LongDouble("1")/result;
			}
			return result;
		}

		private LongDouble Pow(LongDouble value1, int value)
		{
			if (value == 1)
				return value1;

			LongDouble temp;
			temp = Pow(value1, value/2);
			temp *= temp;
			if (value%2 != 0)
			{
				temp *= value1;
			}

			return temp;
		}

		public static LongDouble XOR(LongDouble value1, LongDouble value2)
		{
			if (value1 == null)
				throw new ArgumentNullException("value1");
			if (value2 == null)
				throw new ArgumentNullException("value2");
			if (value2.IsDouble || value1.IsDouble)
				throw new Exception("can operate only on integer");

			var result = new LongDouble();
			result.Integer = (value1.IntegerBig ^ value2.IntegerBig).ToString();
			return result;
		}

		public static string Splitter { get { return splitter; } }

		bool isLong;

		public bool IsLong { get { return isLong; } }
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			if ((integer != "0" || fraction != "0")&& IsMinus)
				builder.Append("-");
			if (fraction != "0" && !String.IsNullOrWhiteSpace(fraction))
				builder.Append( integer.ToString().ToUpper() + splitter + fraction.ToString().ToUpper());
			else
				builder.Append( integer.ToString().ToUpper());
			return builder.ToString();
		}

		public string Fraction
		{
			get { return fraction; }
			set
			{
				if (value.IndexOf(splitter) >=0)
					value = value.Substring(value.IndexOf(splitter)+1);
				value = new string(value.Reverse().SkipWhile((char a) => { return a == '0'; }).Reverse().ToArray<char>()); ;
				if (String.IsNullOrWhiteSpace(value))
					fraction = "0";
				else
					fraction = value;

			}
		}
	}
}
