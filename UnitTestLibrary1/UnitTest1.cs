using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using NumberConverter;

namespace UnitTestLibrary1
{
    [TestClass]
    public class UnitTest1
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


    }
}
