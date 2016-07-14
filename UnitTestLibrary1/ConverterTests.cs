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
		public byte baseIn { get; set; }
		public byte baseOut { get; set; }
		public string value { get; set; }
		public string resultO { get; set; }

		[TestMethod]
	    public void Convert2To10Test()
		{
			//Arrange
			Converter.Converter.Accurancy = 8;
			baseIn = 2;
			baseOut = 10;
			value = "1101.0011001111";

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
			baseIn = 2;
			baseOut = 25;
			value = "1101.0011001111";
			resultO = "d.518e5kll".ToUpper();

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
			 baseIn = 2;
			 baseOut = 25;
			 value = null;
			 resultO = "d.518e5kll".ToUpper();

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
			 baseIn = 2;
			 baseOut = 25;
			 value = "";
			 resultO = "0".ToUpper();
			
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
			 baseIn = 2;
			 baseOut = 25;
			 value = "0";
			 resultO = "0".ToUpper();

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
			 baseIn = 2;
			 baseOut = 25;
			 value = "354.123";
			 resultO = "234".ToUpper();

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
			 baseIn = 10;
			 baseOut = 27;
			 value = "20.354";
			 resultO = "k.9f1l322n".ToUpper();

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
			 baseIn = 10;
			 baseOut = 10;
			 value = "20.354";
			 resultO = "20.354".ToUpper();

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
			 baseIn = 10;
			 baseOut = 10;
			 value = "20.354";
			 resultO = "20.354".ToUpper();

			//Act
			var result = Converter.Converter.ConvertTo(baseIn, value, baseOut);

			//Assert
			Assert.IsTrue(result == resultO, String.Format("{0} != {1}", result, resultO));
		}
	}
}
