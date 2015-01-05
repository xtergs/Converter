using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestPlatform;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using NumberConverter;
using NumberConverter.Exceptions;

namespace UnitTestLibrary1
{
    [TestClass]
	public class ConverterTests
	{
		[TestMethod]
	    public void Convert2To10Test()
		{
			//Arrange
			Converter.Converter.Accurancy = 8;
			uint baseIn = 2;
			uint baseOut = 10;
			string value = "1101.0011001111";

			//Act
			var result = Converter.Converter.ConvertTo(baseIn, value, baseOut);

			//Assert
			Assert.IsTrue(result == 13.2021484375.ToString());
		}
		[TestMethod]
		public void Convert2To25Test()
		{
			//Arrange
			Converter.Converter.Accurancy = 8;
			uint baseIn = 2;
			uint baseOut = 25;
			string value = "1101.0011001111";
			string resultO = "d.518e5kll".ToUpper();

			//Act
			var result = Converter.Converter.ConvertTo(baseIn, value, baseOut);

			//Assert
			Assert.IsTrue(result ==resultO, String.Format("{0} != {1}", result, resultO));
		}

		[TestMethod]
		public void Convertnull2To25Test()
		{
			//Arrange
			Converter.Converter.Accurancy = 8;
			uint baseIn = 2;
			uint baseOut = 25;
			string value = null;
			string resultO = "d.518e5kll".ToUpper();

			try
			{
				//Act
				var result = Converter.Converter.ConvertTo(baseIn, value, baseOut);
			}
			catch (NullReferenceException)
			{
				//Assert
				Assert.IsTrue(true);
				return;
			}
			Assert.Fail();
		}

		[TestMethod]
		public void ConvertEmpty2To25Test()
		{
			//Arrange
			Converter.Converter.Accurancy = 8;
			uint baseIn = 2;
			uint baseOut = 25;
			string value = "";
			string resultO = "0".ToUpper();
			
			//Act
			var result = Converter.Converter.ConvertTo(baseIn, value, baseOut);
			
			//Assert
			Assert.IsTrue(result == resultO, String.Format("{0} != {1}", result, resultO));
		}

		[TestMethod]
		public void ConvertZero2To25Test()
		{
			//Arrange
			Converter.Converter.Accurancy = 8;
			uint baseIn = 2;
			uint baseOut = 25;
			string value = "0";
			string resultO = "0".ToUpper();

			//Act
			var result = Converter.Converter.ConvertTo(baseIn, value, baseOut);

			//Assert
			Assert.IsTrue(result == resultO, String.Format("{0} != {1}", result, resultO));
		}

		[TestMethod]
		public void ConvertOver2To25Test()
		{
			//Arrange
			Converter.Converter.Accurancy = 8;
			uint baseIn = 2;
			uint baseOut = 25;
			string value = "354.123";
			string resultO = "234".ToUpper();

			try
			{
				//Act
				var result = Converter.Converter.ConvertTo(baseIn, value, baseOut);
			}
			catch (LargDigit)
			{
				//Assert
				Assert.IsTrue(true);
				return;
			}

			//Assert
			Assert.Fail();
		}

		[TestMethod]
		public void Convert10To25Test()
		{
			//Arrange
			Converter.Converter.Accurancy = 8;
			uint baseIn = 10;
			uint baseOut = 27;
			string value = "20.354";
			string resultO = "k.9f1l322n".ToUpper();

			//Act
			var result = Converter.Converter.ConvertTo(baseIn, value, baseOut);

			//Assert
			Assert.IsTrue(result == resultO, String.Format("{0} != {1}", result, resultO));
		}

		[TestMethod]
		public void Convert10To10Test()
		{
			//Arrange
			Converter.Converter.Accurancy = 8;
			uint baseIn = 10;
			uint baseOut = 10;
			string value = "20.354";
			string resultO = "20.354".ToUpper();

			//Act
			var result = Converter.Converter.ConvertTo(baseIn, value, baseOut);

			//Assert
			Assert.IsTrue(result == resultO, String.Format("{0} != {1}", result, resultO));
		}

		[TestMethod]
		public void Convert15To15Test()
		{
			//Arrange
			Converter.Converter.Accurancy = 8;
			uint baseIn = 10;
			uint baseOut = 10;
			string value = "20.354";
			string resultO = "20.354".ToUpper();

			//Act
			var result = Converter.Converter.ConvertTo(baseIn, value, baseOut);

			//Assert
			Assert.IsTrue(result == resultO, String.Format("{0} != {1}", result, resultO));
		}
	}
}
