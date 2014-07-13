using NumberConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Converter
{
	public class Converter
	{
		static Dictionary<char,int> letters = new Dictionary<char, int>();
		static Dictionary<int, char> revletters = new Dictionary<int, char>();

		static int countLoop = 10; //accuracy

		static Converter()
		{
			string let = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			for (int i = 0; i < let.Length; i++)
			{
				letters.Add(Char.ToUpper(let[i]),i);
				revletters.Add(i, let[i]);
			}
			
		}

		public static LongDouble ConvertTo(uint baseIn, LongDouble valueIn, uint baseOut)
		{
			//strIn = strIn.ToUpper();
			if (baseIn == baseOut)
			{
				Validate(baseIn, valueIn);
				return valueIn;
			}
			LongDouble result;
			if (baseIn == 10)
			{
				Validate(baseIn, valueIn);
				result = valueIn;
			}
			else
				result = ConvertToDecictimal(baseIn, valueIn);
			if (baseOut == 10)
				return result;
			result = ConvertFromDecictimal(result, baseOut);
			return result;
		}

		public static LongDouble ConvertToDecictimal(uint baseIn, LongDouble valueIn)
		{
			BigInteger outValue = new BigInteger();
			//convert integer part
			for (int i = valueIn.Integer.Length-1, j = 0; i >= 0; i--, j++)
			{
				if (letters[valueIn.Integer[i]] >= baseIn)
					throw BaseException("Digit in number >= base");
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
					throw BaseException("Digit in number >= base");
				if (valueIn.IsLong)
					throw new NotImplementedException();
				else
					outValueDr += (letters[valueIn.Fraction[i]] * Math.Pow(baseIn, j));
			}
			//string returnStr = outValueDr.ToString();
			//if (outValueDr > 0)
			//{
			//	var x = outValueDr.ToString();
			//	var d = x.Skip(2);
			//	returnStr += '.'; ;
			//	foreach (var a in x.Skip(2))
			//	{
			//		returnStr += a.ToString();
			//	}
			//}
			string xx = "0";
			if (outValueDr >0)
				xx = outValueDr.ToString().Substring(2);
			return new LongDouble(outValue.ToString(), xx);
		}

		static LongDouble ConvertFromDecictimal(LongDouble valueIn, uint baseOut)
		{
		//	double res;
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
							var a = dres * baseOut;
							int b = (int)a;
							returnStr.Append(revletters[b]);
							dres = a - b;
						}
					}
					returnValue.Fraction = returnStr.ToString();
					returnStr.Clear();
			//	returnStr += '.';
			
			//int pos = 0;
			//if (posSeper < 0)
			//	pos = strIn.Length;
			//else
			//	pos = posSeper;
			// convert int part
			BigInteger rus;
					if ((rus = BigInteger.Parse(valueIn.Integer)) >= 0)
					{
						BigInteger rres = 0;									//int != 0
						while (rus >= baseOut)
						{
							rres =  rus / baseOut;
						//	rres = Math.Floor(res / baseOut);
							returnStr.Append(revletters[(int)(rus - rres * baseOut)]);
							rus = rres;
						}
						returnStr.Append(revletters[(int)rus]);
					}
					//else ;
			returnValue.Integer = new string(returnStr.ToString().Reverse().ToArray<char>());
				//returnStr += "0";		// int == 0
			//var temp = returnStr.ToCharArray();
			////	temp = temp.Reverse().ToArray<char>();
			//if (returnStr.IndexOf('.') > 0)
			//	temp = temp.SkipWhile((char a) => { return a == '0'; }).ToArray<char>();
			//if (temp[0] == '.')
			//	temp = temp.SkipWhile((char a) => { return a == '.'; }).ToArray<char>();
			//temp = temp.Reverse().ToArray<char>();
			//temp = temp.SkipWhile((char a) => { return a == '0'; }).ToArray<char>();
			//return new string(temp);
			returnValue.IsMinus = valueIn.IsMinus;
			return returnValue;
		}


		public static string ConvertTo(uint baseIn, string strIn, uint baseOut)
		{
			return ConvertTo(baseIn, new LongDouble(strIn), baseOut).ToString();
			//strIn = strIn.ToUpper();
			//if (baseIn == baseOut)
			//{
			//	Validate(baseIn, strIn);
			//	return strIn;
			//}
			//string result;
			//if (baseIn == 10)
			//{
			//	Validate(baseIn, strIn);
			//	result = strIn;
			//}
			//else
			//	result = ConvertToDecictimal(baseIn, strIn);
			//if (baseOut == 10)
			//	return result;
			//result = ConvertFromDecictimal(result, baseOut);
			//return result.ToUpper();
		}

		//static void Validate(uint baseIn, string str)
		//{
		//	for (int i = 0; i < str.Length; i++)
		//		if (str[i] != '.' && letters[str[i]] >= baseIn )
		//			throw BaseException("Digit in number >= base");
		//}

		static void Validate(uint baseIn, LongDouble value)
		{
			if (value.Integer.Any((char a) => { return letters[a] >= baseIn; }) || value.Fraction.Any((char a) => letters[a] >= baseIn))
				throw BaseException("Digit in number >= base");
		}

		//static string ConvertToDecictimal(uint baseIn, string strIn)
		//{
		//	int posSeper = strIn.IndexOf('.');
		//	if (posSeper < 0)
		//		posSeper = strIn.Length;
		//	ulong outValue = 0;
		//	for (int i = posSeper - 1,  j = 0; i >= 0; i--, j++ )
		//	{
		//		if (letters[strIn[i]] >= baseIn)
		//			throw BaseException("Digit in number >= base");
		//		outValue += (ulong)(letters[strIn[i]] * Math.Pow(baseIn, j));
		//	}
		//	double outValueDr = 0;
		//	for (int i = posSeper + 1, j = -1; i < strIn.Length; i++, j--)
		//	{
		//		if (letters[strIn[i]] >= baseIn)
		//			throw BaseException("Digit in number >= base");
		//		outValueDr += (letters[strIn[i]] * Math.Pow(baseIn, j));
		//	}
		//	string returnStr = outValue.ToString();
		//	if (outValueDr > 0)
		//	{
		//		var x = outValueDr.ToString();
		//		var d = x.Skip(2);
		//		returnStr += '.'; ;
		//		foreach (var a in x.Skip(2))
		//		{
		//			returnStr += a.ToString();
		//		}
		//	}
		//	return returnStr;
		//}

		private static Exception BaseException(string p)
		{
			throw new Exception(p);
		}

		//static string ConvertFromDecictimal(string strIn, uint baseOut)
		//{
		//	int posSeper = strIn.IndexOf('.');
		//	double res;
		//	string returnStr = "";

		//	// convert decimital part	
		//	if (posSeper >=0)
		//	{
				

		//		double dres = 0;
		//		if (posSeper != strIn.Length)
		//			if (double.TryParse("0." + strIn.Substring(posSeper + 1), out dres))
		//			{
		//				for (int i = 0; i < countLoop; i++)
		//				{
		//					if (dres == 0)
		//						break;
		//					var a = dres * baseOut;
		//					int b = (int)a;
		//					returnStr = revletters[b] + returnStr;
		//					dres = a - b;
		//				}
		//			}
		//		returnStr += '.';
		//	}
		//	int pos = 0;
		//	if (posSeper < 0)
		//		pos = strIn.Length;
		//	else
		//		pos = posSeper;
		//	// convert int part
		//	if ((res = double.Parse(strIn.Substring(0, pos))) > 0) 
		//	{								
		//		double rres = 0;									//int != 0
		//		while (res >= baseOut)
		//		{
		//			rres = Math.Floor(res / baseOut);
		//			returnStr += revletters[(int)(res - rres * baseOut)];
		//			res = rres;
		//		}
		//		returnStr = returnStr + revletters[(int)res];
		//	}
		//	else
		//		returnStr += "0";		// int == 0
		//	var temp = returnStr.ToCharArray();
		////	temp = temp.Reverse().ToArray<char>();
		//	if (returnStr.IndexOf('.') > 0)
		//		temp = temp.SkipWhile((char a) => { return a == '0'; }).ToArray<char>();
		//	if (temp[0] == '.')
		//		temp = temp.SkipWhile((char a) => { return a== '.';}).ToArray<char>();
		//	temp = temp.Reverse().ToArray<char>();
		//	temp = temp.SkipWhile((char a) => { return a == '0'; }).ToArray<char>();
		//	return new string(temp);
		//}

		//static int[] GetIntArray(string strIn)
		//{
		//	int[] outArray = new int[strIn.Length];
		//	for (int i = 0; i < strIn.Length; i++)
		//	{
		//		if (strIn[i] >= '0' && strIn[i] <= '9')
		//			outArray[i] = strIn[i] - '0';
		//		else
		//		{

		//		}
		//	}
		//}

	}
}
