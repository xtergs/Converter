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
		public void DoubleNullAnd()
		{
			//Arrange
			LongDouble value1 = new LongDouble("10.0000");
			LongDouble value2 = new LongDouble("8.0000");

			//Act
			try
			{
				var result = value1 & null;
			}
			catch (ArgumentNullException)
			{
				//Assert
				Assert.IsTrue(true);
				return;
			}
			//Assert
			Assert.Fail();
		}


		[TestMethod]
		public void DoubleAnd()
		{
			//Arrange
			LongDouble value1 = new LongDouble("10.0000");
			LongDouble value2 = new LongDouble("8.2000");

			//Act
			try
			{
				var result = value1 & value2;
			}
			catch (ArgumentDoubleException)
			{
				//Assert
				Assert.IsTrue(true);
				return;
			}
			//Assert
			Assert.Fail();
		}

		[TestMethod]
		public void DoubleAndInv()
		{
			//Arrange
			LongDouble value1 = new LongDouble("10.2000");
			LongDouble value2 = new LongDouble("8.0000");

			//Act
			try
			{
				var result = value1 & value2;
			}
			catch (ArgumentDoubleException)
			{
				//Assert
				Assert.IsTrue(true);
				return;
			}
			//Assert
			Assert.Fail();
		}

		#endregion

		#region OR
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
		public void DoubleNullOr()
		{
			//Arrange
			LongDouble value1 = new LongDouble("10.0000");
			LongDouble value2 = new LongDouble("8.0000");

			//Act
			try
			{
				var result = value1 | null;
			}
			catch (ArgumentNullException)
			{
				//Assert
				Assert.IsTrue(true);
				return;
			}
			//Assert
			Assert.Fail();
		}


		[TestMethod]
		public void DoubleOr()
		{
			//Arrange
			LongDouble value1 = new LongDouble("10.0000");
			LongDouble value2 = new LongDouble("8.2000");

			//Act
			try
			{
				var result = value1 | value2;
			}
			catch (ArgumentDoubleException)
			{
				//Assert
				Assert.IsTrue(true);
				return;
			}
			//Assert
			Assert.Fail();
		}

		[TestMethod]
		public void DoubleOrInv()
		{
			//Arrange
			LongDouble value1 = new LongDouble("10.2000");
			LongDouble value2 = new LongDouble("8.0000");

			//Act
			try
			{
				var result = value1 | value2;
			}
			catch (ArgumentDoubleException)
			{
				//Assert
				Assert.IsTrue(true);
				return;
			}
			//Assert
			Assert.Fail();
		}

		#endregion

		#region No

		[TestMethod]
		public void IntegerPartNo()
		{
			//Arrange
			LongDouble value1 = new LongDouble("-4");

			//Act
			var result = ~value1 ;

			//Assert
			Assert.IsTrue(result.ToString() == "-3");
		}

		[TestMethod]
		public void IntegerPartPlausNo()
		{
			//Arrange
			LongDouble value1 = new LongDouble("4");

			//Act
			var result = ~value1;

			//Assert
			Assert.IsTrue(result.ToString() == "5");
		}

	    [TestMethod]
	    public void IntegerDoublePartNo()
	    {
		    //Arrange
		    LongDouble value1 = new LongDouble("-4.324");

		    try
		    {
			    //Act
			    var result = ~value1;

		    }
		    catch (ArgumentDoubleException)
		    {
			    //Assert
			    Assert.IsTrue(true);
			    return;
		    }
		    //Assert
		    Assert.Fail();
	    }

	    #endregion

		#region Pow
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
		#endregion


		#region operator +

		[TestMethod]
		public void IntPlus()
		{
			//Arrange
			LongDouble value1 = new LongDouble("4");
			LongDouble value2 = new LongDouble("2");

			//Act
			var result = value1 + value2;

			//Assert
			Assert.IsTrue(result.ToString() == "6");
		}

		[TestMethod]
		public void IntDoublePlus()
		{
			//Arrange
			LongDouble value1 = new LongDouble("4.0002");
			LongDouble value2 = new LongDouble("2.03");

			//Act
			var result = value1 + value2;

			//Assert
			Assert.IsTrue(result.ToString() == "6.0302");
		}

		[TestMethod]
		public void MinusIntPlus()
		{
			//Arrange
			LongDouble value1 = new LongDouble("-4");
			LongDouble value2 = new LongDouble("2");

			//Act
			var result = value1 + value2;

			//Assert
			Assert.IsTrue(result.ToString() == "-2");
		}

		[TestMethod]
		public void IntMinusMinusPlus()
		{
			//Arrange
			LongDouble value1 = new LongDouble("-4");
			LongDouble value2 = new LongDouble("-2");

			//Act
			var result = value1 + value2;

			//Assert
			Assert.IsTrue(result.ToString() == "-6");
		}

		[TestMethod]
		public void IntMinusDoublePlus()
		{
			//Arrange
			double val1 = -4.003;
			double val2 = 2;
			LongDouble value1 = new LongDouble(val1);
			LongDouble value2 = new LongDouble(val2);

			//Act
			var result = value1 + value2;

			//Assert
			Assert.IsTrue(result.ToString() == (val1 + val2).ToString(), String.Format("{0} != {1}", result, val1 + val2));
		}

		[TestMethod]
		public void DoublePlus()
		{
			//Arrange
			double val1 = 0.802;
			double val2 = 0.43;
			LongDouble value1 = new LongDouble(val1);
			LongDouble value2 = new LongDouble(val2);

			//Act
			var result = value1 + value2;

			//Assert
			Assert.IsTrue(result.ToString() == (val1 + val2).ToString());
		}

		[TestMethod]
		public void MinusDoublePlus()
		{
			//Arrange
			double val1 = -0.802;
			double val2 = 0.43;
			LongDouble value1 = new LongDouble(val1);
			LongDouble value2 = new LongDouble(val2);

			//Act
			var result = value1 + value2;

			//Assert
			Assert.IsTrue(result.ToString() == (val1 + val2).ToString());
		}

		[TestMethod]
		public void ControlPlus()
		{
			//Arrange
			var rand = new Random();
			double val1 = rand.NextDouble() * rand.Next(-999,999);
			double val2 = rand.NextDouble() * rand.Next(-999,999);
			LongDouble value1 = new LongDouble(val1);
			LongDouble value2 = new LongDouble(val2);

			//Act
			var result = value1 + value2;

			//Assert
			Assert.IsTrue(result.ToString(13) == (val1 + val2).ToString("F13"),
				String.Format("{0} != {1} + {2} = {3:F13}", result.ToString(13), val1, val2, val1 + val2));
		}


		#endregion

		#region operator -

		[TestMethod]
		public void IntMinus()
		{
			//Arrange
			LongDouble value1 = new LongDouble("4");
			LongDouble value2 = new LongDouble("2");

			//Act
			var result = value1 - value2;

			//Assert
			Assert.IsTrue(result.ToString() == "2");
		}

		[TestMethod]
		public void IntDoubleMinus()
		{
			//Arrange
			double val1 = 4.0002;
			double val2 = 2.03;
			LongDouble value1 = new LongDouble(val1);
			LongDouble value2 = new LongDouble(val2);

			//Act
			var result = value1 - value2;

			//Assert
			Assert.IsTrue(result.ToString() == (val1 - val2).ToString(), String.Format("{0} != {1}", result, val1 - val2));
		}

		[TestMethod]
		public void MinusIntMinus()
		{
			//Arrange
			LongDouble value1 = new LongDouble("-4");
			LongDouble value2 = new LongDouble("2");

			//Act
			var result = value1 - value2;

			//Assert
			Assert.IsTrue(result.ToString() == "-6");
		}

		[TestMethod]
		public void IntMinusMinusMinus()
		{
			//Arrange
			LongDouble value1 = new LongDouble("-4");
			LongDouble value2 = new LongDouble("-2");

			//Act
			var result = value1 - value2;

			//Assert
			Assert.IsTrue(result.ToString() == "-2");
		}

		[TestMethod]
		public void IntMinusDoubleMinus()
		{
			//Arrange
			double val1 = -4.003;
			double val2 = 2;
			LongDouble value1 = new LongDouble(val1);
			LongDouble value2 = new LongDouble(val2);

			//Act
			var result = value1 - value2;

			//Assert
			Assert.IsTrue(result.ToString() == (val1 - val2).ToString());
		}

		[TestMethod]
		public void LargIntMinusDoubleMinus()
		{
			//Arrange
			const double val1 = 2.003;
			const double val2 = 4;
			LongDouble value1 = new LongDouble(val1);
			LongDouble value2 = new LongDouble(val2);

			//Act
			var result = value1 - value2;

			//Assert
			Assert.IsTrue(result.ToString() == (val1 - val2).ToString(), String.Format("{0} != {1}", result, val1 - val2));
		}

		#endregion

		#region operator *

		[TestMethod]
		public void IntMult()
		{
			//Arrange
			LongDouble value1 = new LongDouble("4");
			LongDouble value2 = new LongDouble("2");

			//Act
			var result = value1 * value2;

			//Assert
			Assert.IsTrue(result.ToString() == (4*2).ToString());
		}

		[TestMethod]
		public void IntDoubleMult()
		{
			//Arrange
			LongDouble value1 = new LongDouble("4.0002");
			LongDouble value2 = new LongDouble("2.03");

			//Act
			var result = value1 * value2;

			//Assert
			Assert.IsTrue(result.ToString() == (4.0002 * 2.03).ToString());
		}

		[TestMethod]
		public void MinusIntMult()
		{
			//Arrange
			LongDouble value1 = new LongDouble("-4");
			LongDouble value2 = new LongDouble("2");

			//Act
			var result = value1 * value2;

			//Assert
			Assert.IsTrue(result.ToString() == (-4*2).ToString());
		}

		[TestMethod]
		public void IntMinusMinusMult()
		{
			//Arrange
			LongDouble value1 = new LongDouble(-4);
			LongDouble value2 = new LongDouble(-2);

			//Act
			var result = value1 * value2;

			//Assert
			Assert.IsTrue(result.ToString() == (-4*-2).ToString());
		}

		[TestMethod]
		public void IntMinusDoubleMult()
		{
			//Arrange
			LongDouble value1 = new LongDouble(-4.003);
			LongDouble value2 = new LongDouble(2);

			//Act
			var result = value1 * value2;

			//Assert
			Assert.IsTrue(result.ToString() == (-4.003*2).ToString());
		}

		[TestMethod]
		public void ZeroIntMinusDoubleMult()
		{
			//Arrange
			double val1 = 2.003;
			double val2 = 0;
			LongDouble value1 = new LongDouble(val1);
			LongDouble value2 = new LongDouble(val2);

			//Act
			var result = value1 * value2;

			//Assert
			Assert.IsTrue(result.ToString() == (val1 * val2).ToString());
		}

		#endregion

		#region operator /

		[TestMethod]
		public void IntDivid()
		{
			//Arrange
			int val1 = 4;
			int val2 = 2;
			LongDouble value1 = new LongDouble(val1);
			LongDouble value2 = new LongDouble(val2);

			//Act
			var result = value1 / value2;

			//Assert
			Assert.IsTrue(result.ToString() == (val1 / val2).ToString());
		}

		[TestMethod]
		public void IntDoubleDivid()
		{
			//Arrange
			double val1 = 4.0002;
			double val2 = 2.03;
			LongDouble value1 = new LongDouble(val1);
			LongDouble value2 = new LongDouble(val2);

			//Act
			var result = value1 / value2;

			//Assert
			Assert.IsTrue(result.ToString(10) == (val1 / val2).ToString("F10"), String.Format("{0} != {1:F10}", result.ToString(10), val1 / val2));
		}

		[TestMethod]
		public void MinusIntDivid()
		{
			//Arrange
			double val1 = -4;
			double val2 = 2;
			LongDouble value1 = new LongDouble(val1);
			LongDouble value2 = new LongDouble(val2);

			//Act
			var result = value1 / value2;

			//Assert
			Assert.IsTrue(result.ToString() == (val1 / val2).ToString());
		}

		[TestMethod]
		public void IntMinusMinusDivid()
		{
			//Arrange
			double val1 = -4;
			double val2 = -2;
			LongDouble value1 = new LongDouble(val1);
			LongDouble value2 = new LongDouble(val2);

			//Act
			var result = value1 / value2;

			//Assert
			Assert.IsTrue(result.ToString() == (val1 / val2).ToString());
		}

		[TestMethod]
		public void IntMinusDoubleDivid()
		{
			//Arrange
			double val1 = -4.003;
			double val2 = 2;
			LongDouble value1 = new LongDouble(val1);
			LongDouble value2 = new LongDouble(val2);

			//Act
			var result = value1 / value2;

			//Assert
			Assert.IsTrue(result.ToString() == (val1 / val2).ToString());
		}

		[TestMethod]
		public void ZeroIntMinusDoubleDivid()
		{
			//Arrange
			double val1 = 2.003;
			double val2 = 0;
			LongDouble value1 = new LongDouble(val1);
			LongDouble value2 = new LongDouble(val2);

			try
			{
				//Act
				var result = value1/value2;
			}
			catch (DivideByZeroException)
			{
				//Assert
				Assert.IsTrue(true);
				return;
			}

			//Assert
			Assert.Fail();
		}

		[TestMethod]
		public void IntMinusDoubleZeroDivid()
		{
			//Arrange
			double val1 = 0;
			double val2 = 2;
			LongDouble value1 = new LongDouble(val1);
			LongDouble value2 = new LongDouble(val2);

			//Act
			var result = value1 / value2;

			//Assert
			Assert.IsTrue(result.ToString() == (val1 / val2).ToString());
		}

		#endregion
	}
}
