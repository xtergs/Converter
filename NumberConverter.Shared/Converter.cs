using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Converter
{
	public class Converter
	{
		static Dictionary<char,int> letters = new Dictionary<char, int>();
		static Dictionary<int, char> revletters = new Dictionary<int, char>();

		static int countLoop = 10;

		static Converter()
		{
			string let = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
			for (int i = 0; i < let.Length; i++)
			{
				letters.Add(Char.ToUpper(let[i]),i);
				revletters.Add(i, let[i]);
			}
			
		}

		public static string ConvertTo(uint baseIn, string strIn, uint baseOut)
		{
			strIn = strIn.ToUpper();
			if (baseIn == baseOut)
				return strIn;
			string result;
			if (baseIn == 10)
				result = strIn;
			else
				result = ConvertToDecictimal(baseIn, strIn);
			if (baseOut == 10)
				return result;
			result = ConvertFromDecictimal(result, baseOut);
			return result.ToUpper();
		}

		static string ConvertToDecictimal(uint baseIn, string strIn)
		{
			int posSeper = strIn.IndexOf('.');
			if (posSeper < 0)
				posSeper = strIn.Length;
			ulong outValue = 0;
			for (int i = posSeper - 1,  j = 0; i >= 0; i--, j++ )
			{
				if (letters[strIn[i]] >= baseIn)
					throw BaseException("Digit in number >= base");
				outValue += (ulong)(letters[strIn[i]] * Math.Pow(baseIn, j));
			}
			ulong outValueDr = 0;
			for (int i = posSeper + 1, j = -1; i < strIn.Length; i++, j--)
			{
				outValueDr += (ulong)(letters[strIn[i]] * Math.Pow(baseIn, j));
			}
			string returnStr = outValue.ToString();
			if (outValueDr > 0)
			{
				returnStr += '.' + outValueDr.ToString();
			}
			return returnStr;
		}

		private static Exception BaseException(string p)
		{
			throw new Exception(p);
		}

		static string ConvertFromDecictimal(string strIn, uint baseOut)
		{
			int posSeper = strIn.IndexOf('.');
			double res;
			string returnStr = "";

				
			if (posSeper >=0)
			{
				

				double dres = 0;
				if (posSeper != strIn.Length)
					if (double.TryParse("0." + strIn.Substring(posSeper + 1), out dres))
					{
						for (int i = 0; i < countLoop; i++)
						{
							var a = dres * baseOut;
							int b = (int)a;
							returnStr += revletters[b];
							dres = a - b;
						}
					}
				returnStr += '.';
			}
			int pos = 0;
			if (posSeper < 0)
				pos = strIn.Length;
			else
				pos = posSeper;
			if ((res = double.Parse(strIn.Substring(0, pos))) > 0)
			{
				double rres = 0;
				while (res >= baseOut)
				{
					rres = Math.Floor(res / baseOut);
					returnStr += revletters[(int)(res - rres * baseOut)];
					res = rres;
				}
				returnStr = returnStr + revletters[(int)res];
			}
			else
				returnStr = "0";
			var temp = returnStr.ToCharArray();
		//	temp = temp.Reverse().ToArray<char>();
			if (returnStr.IndexOf('.') > 0)
				temp = temp.SkipWhile((char a) => { return a == '0'; }).ToArray<char>();
			if (temp[0] == '.')
				temp = temp.SkipWhile((char a) => { return a== '.';}).ToArray<char>();
			temp = temp.Reverse().ToArray<char>();
			temp = temp.SkipWhile((char a) => { return a == '0'; }).ToArray<char>();
			return new string(temp);
		}

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
