using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace UnitTestLibrary1
{
	[TestClass]
	public class ConverterUnitTest
	{
		[TestMethod]
		public void ConvertToTest()
		{
			//Arrange
			uint baseIn = 2;
			uint baseOut = 16;
			string str = "0000111000011101.000101001010000";

			//Act
			var result = Converter.Converter.ConvertTo(baseIn, str, baseOut );

			//Assert
			Assert.IsTrue(result == "E1D.14A");

			//Arrange
			baseIn = 2;
			baseOut = 30;
			str = "0000111000011101.000101001010000";

			//Act
			result = Converter.Converter.ConvertTo(baseIn, str, baseOut);

			//Assert
			Assert.IsTrue(result == "40D.2CF8NK4KIM");

			//Arrange
			baseIn = 2;
			baseOut = 30;
			str = "000001.001";

			//Act
			result = Converter.Converter.ConvertTo(baseIn, str, baseOut);

			//Assert
			Assert.IsTrue(result == "1.3MF");
		}
	}
}
