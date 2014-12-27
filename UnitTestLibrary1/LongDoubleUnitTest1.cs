using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using NumberConverter;

namespace UnitTestLibrary1
{
    [TestClass]
    public class LongDoubleUnitTest1
    {
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
			var result = ~value1 ;

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

	    [TestMethod]
	    public void LongDoubleMult()
	    {
		    //Arrange
		    LongDouble value1 = new LongDouble("10.2");
		    LongDouble value2 = new LongDouble("22.45");
			LongDouble result = new LongDouble("228", "99");

		    //Act
		    value1 = value1*value2;

		    //Assert
			Assert.IsTrue(value1.ToString() == "228.99");

			//Arrange
			value1 = new LongDouble("00000.00000002");
			value2 = new LongDouble("22.45");

			//Act
			value1 = value1 * value2;

			//Assert
			Assert.IsTrue(value1.ToString() == "0.000000449");
	    }

		[TestMethod]
	    public void LongDoubleRShift()
	    {
			//Arrange
			LongDouble value1 = new LongDouble("10");
		    int value2 = 22;

			//Act
		    value1 = value1 >> value2;

			//Assert
			Assert.IsTrue(value1.ToString() == "0");
	    }

    }
}
