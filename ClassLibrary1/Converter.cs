using NumberConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using NumberConverter.Exceptions;

namespace Converter
{
	public class Converter
	{
		static Dictionary<char, int> letters = new Dictionary<char, int>();
		static Dictionary<int, char> revletters = new Dictionary<int, char>();

		static int countLoop = 10; //accuracy

		public static int Accurancy
		{
			get { return countLoop; }
			set
			{
				if (value < 0)
					return;
				countLoop = value;
			}
		}

		static Converter()
		{
			string let = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			for (byte i = 0; i < let.Length; i++)
			{
				letters.Add(Char.ToUpper(let[i]),i);
				revletters.Add(i, let[i]);
			}
			
		}

		public static LongDouble ConvertTo(byte baseIn, LongDouble valueIn, byte baseOut)
		{
			if (baseIn == baseOut)
			{
				if (!Validate(baseIn, valueIn))
					throw new LargDigit("Digit in number >= base");
				return valueIn;
			}
			LongDouble result;
			if (baseIn == 10)
			{
				if (!Validate(baseIn, valueIn))
					throw new LargDigit("Digit in number >= base");
				result = valueIn;
			}
			else
				result = ConvertToDecictimal(baseIn, valueIn);
			if (baseOut == 10)
				return result;
			result = ConvertFromDecictimal(result, baseOut);
			return result;
		}

		public static LongDouble ConvertToDecictimal(byte baseIn, LongDouble valueIn)
		{
			BigInteger outValue = new BigInteger();

			//convert integer part
			for (int i = valueIn.Integer.Length-1, j = 0; i >= 0; i--, j++)
			{
				if (letters[valueIn.Integer[i]] >= baseIn)
					throw new LargDigit("Digit in number >= base");
				if (valueIn.IsLong)
					throw new NotImplementedException();
				else
					outValue += (letters[valueIn.Integer[i]] * BigInteger.Pow(baseIn, j));
			}
			double outValueDr = 0;

			//convert fraction part
			for (int i = 0, j = -1; i < valueIn.Fraction.Length; i++, j--)
			{
				if (letters[valueIn.Fraction[i]] >= baseIn)
					throw new LargDigit("Digit in number >= base");
				if (valueIn.IsLong)
					throw new NotImplementedException();
				else
					outValueDr += (letters[valueIn.Fraction[i]] * Math.Pow(baseIn, j));
			}

			string xx = "0";
			if (outValueDr >0)
				xx = outValueDr.ToString().Substring(2);
			return new LongDouble(outValue.ToString(), xx);
		}

		private static LongDouble ConvertFromDecictimal(LongDouble valueIn, byte baseOut)
		{
			StringBuilder returnStr = new StringBuilder();
			var returnValue = new LongDouble();
			returnValue.IsMinus = valueIn.IsMinus;
			// convert decimital part	

			decimal dres = 0;

			if (decimal.TryParse("0" + LongDouble.Splitter + valueIn.Fraction, out dres))
			{
				for (int i = 0; i < countLoop; i++)
				{
					if (dres == 0)
						break;
					var a = dres*baseOut;
					int b = (int) a;
					returnStr.Append(revletters[b]);
					dres = a - b;
				}
			}
			returnValue.Fraction = returnStr.ToString();
			returnStr.Clear();

			BigInteger rus;
			if ((rus = BigInteger.Parse(valueIn.Integer)) >= 0)
			{
				BigInteger rres = 0; //int != 0
				while (rus >= baseOut)
				{
					rres = rus/baseOut;
					returnStr.Append(revletters[(int) (rus - rres*baseOut)]);
					rus = rres;
				}
				returnStr.Append(revletters[(int) rus]);
			}
			returnValue.Integer = new string(returnStr.ToString().Reverse().ToArray<char>());

			returnValue.IsMinus = valueIn.IsMinus;
			return returnValue;
		}


		public static string ConvertTo(byte baseIn, string strIn, byte baseOut)
		{
			return ConvertTo(baseIn, new LongDouble(strIn), baseOut).ToString();
		}

		public static bool Validate(byte baseIn, LongDouble value)
		{
			if (value.Integer.Any((char a) => { return letters[a] >= baseIn; }) ||
			    value.Fraction.Any((char a) => letters[a] >= baseIn))
				return false;
			return true;
		}

		public static bool Validate(uint baseIn, string value)
		{
			if (value.Any((a) =>
			{
				return  a != LongDouble.Splitter[0] && letters[a] >= baseIn;
			}))
				return false;
			return true;
		}

		private static Exception BaseException(string p)
		{
			throw new Exception(p);
		}
	}
}
