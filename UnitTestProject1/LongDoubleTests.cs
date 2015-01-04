using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
	[TestClass]
	public class LongDoubleTests
	{

		#region AND

		[TestMethod]
		public void IntegerPartAnd()
		{
			//Arrange
			LongDouble value1 = new LongDouble("10");
			LongDouble value2 = new LongDouble("8");

			//Act
			var result = value1 & value2;

			//Assert
			Assert.IsTrue(result.ToString() == "8");
		}

		[TestMethod]
		public void IntegerDoubleAnd()
		{
			//Arrange
			LongDouble value1 = new LongDouble("10.0000");
			LongDouble value2 = new LongDouble("8.0000");

			//Act
			var result = value1 & value2;

			//Assert
			Assert.IsTrue(result.ToString() == "8");
		}

		[TestMethod]
		[ExpectedException]
		public void DoubleAnd()
		{
			//Arrange
			LongDouble value1 = new LongDouble("10.0000");
			LongDouble value2 = new LongDouble("8.0000");

			//Act
			var result = value1 & value2;

			//Assert
			Assert.IsTrue(result.ToString() == "8");
		}

		#endregion


		[TestMethod]
		public void IntegerPartOr()
		{
			//Arrange
			LongDouble value1 = new LongDouble("2");
			LongDouble value2 = new LongDouble("1");

			//Act
			var result = value1 | value2;

			//Assert
			Assert.IsTrue(result.ToString() == "3");
		}

		[TestMethod]
		public void IntegerDoubleOr()
		{
			//Arrange
			LongDouble value1 = new LongDouble("2.0000");
			LongDouble value2 = new LongDouble("1.0000");

			//Act
			var result = value1 | value2;

			//Assert
			Assert.IsTrue(result.ToString() == "3");
		}

		[TestMethod]
		public void IntegerPartNo()
		{
			//Arrange
			LongDouble value1 = new LongDouble("-4");

			//Act
			var result = ~value1;

			//Assert
			Assert.IsTrue(result.ToString() == "3");
		}

		[TestMethod]
		public void LongDoublePow()
		{
			//Arrange
			LongDouble value1 = new LongDouble("4");
			//LongDouble value2 = new LongDouble("2");

			//Act
			var result = value1.Pow(2);

			//Assert
			Assert.IsTrue(result.ToString() == "16");
		}

	}
}
