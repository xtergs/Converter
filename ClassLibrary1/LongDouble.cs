using System;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using NumberConverter.Exceptions;

namespace NumberConverter
{
	public class LongDouble
	{
		private static readonly string splitter = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
		private string fraction = "0";
		private string integer = "0";
		private bool isLong;

		private BigInteger interger = 0;
		private int power = 0;

		public LongDouble()
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

		public LongDouble(int integ, int fract)
			: this(integ.ToString(), fract.ToString())
		{
		}

		public LongDouble(double val)
			: this(val.ToString())
		{
		}

		public LongDouble(string integ, string fract)
		{
			Integer = integ;
			Fraction = fract;
		}

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
				value = new string(value.SkipWhile((char a) => { return a == '0'; }).ToArray());
				if (String.IsNullOrWhiteSpace(value))
				{
					integer = "0";
					return;
				}
				integer = value;
			}
		}

		public BigInteger IntegerBig
		{
			get { return BigInteger.Parse(integer); }
		}

		public decimal FractionDec
		{
			get { return decimal.Parse("0" + splitter + Fraction); }
		}

		public bool IsMinus { get; set; }

		public bool IsDouble
		{
			get
			{
				if (Fraction != "0" && Fraction.Length > 0) return true;
				return false;
			}
		}

		public static string Splitter
		{
			get { return splitter; }
		}

		public bool IsLong
		{
			get { return isLong; }
		}

		public string Fraction
		{
			get { return fraction; }
			set
			{
				if (value.IndexOf(splitter) >= 0)
					value = value.Substring(value.IndexOf(splitter) + 1);
				value = new string(value.Reverse().SkipWhile((char a) => { return a == '0'; }).Reverse().ToArray());
				;
				if (String.IsNullOrWhiteSpace(value))
					fraction = "0";
				else
					fraction = value;
			}
		}

		static bool equal(LongDouble first, LongDouble second)
		{
			if (first.IsMinus != second.IsMinus)
				return false;
			if (first.Fraction != second.Fraction)
				return false;
			if (first.Integer != second.Integer)
				return false;
			return true;
		}
		
		public static LongDouble operatorPlusMinus(LongDouble first, LongDouble second)
		{
			if (first.IsMinus == second.IsMinus)
				return Plus(first, second);
			else if (first < second)
			{
				return Minus(second, first);
			}
			else
			{
				return Minus(first, second);
			}
		}
		/// <summary>
		/// first - second, but first need to be larger, and positive
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		static LongDouble Minus(LongDouble first, LongDouble second)
		{
			var result = new LongDouble();
			decimal firstFraction = first.FractionDec;
			decimal seconFraction = second.FractionDec;
			bool mod = false;
			int buffer = 0;
			BigInteger firstInteg = first.IntegerBig;
			BigInteger secondInteg = second.IntegerBig;

			if (firstInteg == secondInteg)
			{
				firstFraction -= seconFraction;
				buffer = (int)firstFraction;
				//buffer *= -1;
				firstFraction += buffer;

				firstInteg = 0;
				result.IsMinus = firstFraction < 0;

				//firstInteg -= secondInteg + buffer;
			}
			else
			{
				if (firstInteg > secondInteg)
				{
					firstInteg--;
					firstFraction++;

					firstFraction -= seconFraction;
					buffer = (int)firstFraction;
					//buffer *= -1;
					firstFraction -= buffer;

					firstInteg -= secondInteg - buffer;
				}
				else
				{
					result.IsMinus = true;

					secondInteg--;
					seconFraction++;

					firstFraction -= seconFraction;
					buffer = (int)firstFraction;
					//buffer *= -1;
					firstFraction -= buffer;

					firstInteg -= secondInteg - buffer;
				}
			}


			//	//calculate fraction part
			//if (firstFraction == 0 && seconFraction == 0)
			//	;
			//else
			//{
			//	if (firstFraction != 0 && seconFraction != 0)
			//	{
			//		if (firstFraction < seconFraction)
			//		{
			//			firstFraction++;
			//			mod = true;
			//		}
			//		firstFraction -= seconFraction;
			//		buffer = (int)firstFraction;
			//		//buffer *= -1;
			//		firstFraction += buffer;
			//	}
			//	else
			//	{
			//		mod = true;
			//		if (seconFraction == 0)
			//		{
			//			firstFraction = 1 - firstFraction;
			//		}
			//		else if (firstFraction == 0)
			//		{
			//			mod = false;
			//			firstFraction = seconFraction;	
			//		}
			//		else
			//		{
			//			if (mod)
			//				firstFraction++;
			//			firstFraction -= seconFraction;

			//			buffer = (int) firstFraction;
			//			//buffer *= -1;
			//			firstFraction += buffer;
			//		}
			//	}
			//}



			//if (mod)
			//	firstInteg--;

			
			////if (!first.IsMinus && second.IsMinus)
			////{
			////	firstInteg++;
			////	firstFraction = 1 - firstFraction;
			////}


			//firstInteg += secondInteg + buffer;

			result.Integer = firstInteg.ToString();
			result.Fraction = firstFraction.ToString().Substring(2);
			//if (firstInteg < 0 || firstFraction < 0)
			//	result.IsMinus = true;

			return result;
		}

		/// <summary>
		/// first + second, but not consider the sign
		/// </summary>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		static LongDouble Plus(LongDouble first, LongDouble second)
		{
			var result = new LongDouble();
			decimal firstFraction = first.FractionDec;
			decimal seconFraction = second.FractionDec;

			//calculate fraction part
			firstFraction += seconFraction;

			var buffer = (int)firstFraction;
			firstFraction -= buffer;

			BigInteger firstInteg = first.IntegerBig;
			BigInteger secondInteg = second.IntegerBig;

			firstInteg += secondInteg + buffer;

			result.Integer = firstInteg.ToString();
			result.Fraction = firstFraction.ToString().Substring(2);
			result.IsMinus = first.IsMinus;

			return result;
		}

		public static LongDouble operator +(LongDouble first, LongDouble second)
		{
			return operatorPlusMinus(first, second);
		}

		public static LongDouble operator -(LongDouble first, LongDouble second)
		{
			//LongDouble result = new LongDouble();
			//var firstFraction = first.FractionDec;
			//var seconFraction = second.FractionDec;
			//firstFraction -= seconFraction;
			////int buffer = 0;
			//if (firstFraction < 0)
			//	result.IsMinus = true;
			////firstFraction -= buffer;
			//var tempStr = firstFraction.ToString();
			//result.Fraction = tempStr.Substring(tempStr.IndexOf(Splitter)+1);
			//var firstInteg = first.IntegerBig;
			//var secondInteg = second.IntegerBig;
			//firstInteg -= secondInteg;
			//result.Integer = firstInteg.ToString();
			return first + (second*new LongDouble(-1));
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
						while (subb < 0)
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


			BigInteger firstInteg = BigInteger.Parse(strf);
			BigInteger secondInteg = BigInteger.Parse(strs);

			BigInteger remainder;
			//firstInteg /= secondInteg;
			firstInteg = BigInteger.DivRem(firstInteg, secondInteg, out remainder);
			var fract = new StringBuilder();
			BigInteger buf;
			int loop = 20;
			while (remainder > 0 && loop > 0)
			{
				remainder *= 10;
				buf = BigInteger.DivRem(remainder, secondInteg, out remainder);
				fract.Append(buf);
				loop--;
			}

			string strFraction = "0";
			string strInteger = "0";

			strInteger = firstInteg.ToString();
			if (fract.Length > 0)
				strFraction = fract.ToString();

			// Count digits in fraction
			// 2.0 is not double
			// 2.03 is double
			int countDigFraction = 0;
			if (first.IsDouble)
				countDigFraction += first.Fraction.Length;
			if (second.IsDouble)
				countDigFraction += second.Fraction.Length;

			result.Fraction = strFraction;
			result.Integer = strInteger;
			if ((first.IsMinus && !second.IsMinus) || (!first.IsMinus && second.IsMinus))
				result.IsMinus = true;
			return result;
		}

		public static LongDouble operator *(LongDouble first, LongDouble second)
		{
			if (first == null)
				throw new ArgumentNullException("first");
			if (second == null)
				throw new ArgumentNullException("second");
			var result = new LongDouble();
			BigInteger firstInteg = BigInteger.Parse(first.integer + first.Fraction);
			BigInteger secondInteg = BigInteger.Parse(second.Integer + second.Fraction);
			//decimal firstFraction = first.FractionDec;
			//decimal seconFraction = second.FractionDec;

			//calculate integer part
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
			if ((first.IsMinus && !second.IsMinus) || (!first.IsMinus && second.IsMinus))
				result.IsMinus = true;
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
			//Integer
			if (value1.IntegerBig > value2.IntegerBig)
				return true;

			//Fraction
			if (value1.FractionDec > value2.FractionDec)
				return true;

			//if both are equal or -value1 < -value2
			return false;
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
				throw new ArgumentDoubleException("can operate only on integer");

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
				throw new ArgumentDoubleException("can operate only on integer");

			var result = new LongDouble();
			result.Integer = (value1.IntegerBig | value2.IntegerBig).ToString();

			return result;
		}

		public static LongDouble operator <<(LongDouble value1, int value2)
		{
			if (value1 == null)
				throw new ArgumentNullException("value1");
			if (value1.IsDouble)
				throw new ArgumentDoubleException("can operate only on integer");

			var result = new LongDouble();
			result.Integer = (value1.IntegerBig << value2).ToString();
			return result;
		}

		public static LongDouble operator >>(LongDouble value1, int value2)
		{
			if (value1 == null)
				throw new ArgumentNullException("value1");
			if (value1.IsDouble)
				throw new ArgumentDoubleException("can operate only on integer");

			var result = new LongDouble();
			result.Integer = (value1.IntegerBig >> value2).ToString();

			return result;
		}

		public static LongDouble operator ~(LongDouble value1)
		{
			if (value1 == null)
				throw new ArgumentNullException("value1");
			if (value1.IsDouble)
				throw new ArgumentDoubleException("can operate only on integer");

			var result = new LongDouble();
			result.Integer = (~(value1.IntegerBig)).ToString();

			return result;
		}

		public LongDouble Pow(int value1)
		{
			if (value1 == 0)
				return new LongDouble("1");
			var result = new LongDouble(ToString());
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

		public string GetAdditionCode(bool modifi = false)
		{
			return null;
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			if ((integer != "0" || fraction != "0") && IsMinus)
				builder.Append("-");
			if (fraction != "0" && !String.IsNullOrWhiteSpace(fraction))
				builder.Append(integer.ToUpper() + splitter + fraction.ToUpper());
			else
				builder.Append(integer.ToUpper());
			return builder.ToString();
		}

		public string ToString(int precise)
		{
			var builder = new StringBuilder();
			if ((integer != "0" || fraction != "0") && IsMinus)
				builder.Append("-");
			if (fraction != "0" && fraction != "")
			{
				string fractStr = fraction.ToUpper();
				builder.Append(integer.ToUpper() + splitter +
				               String.Format("{0}", precise >= fractStr.Length ? fractStr : fractStr.Substring(0, precise)));
			}
			else
				builder.Append(integer.ToUpper());
			return builder.ToString();
		}
	}
}