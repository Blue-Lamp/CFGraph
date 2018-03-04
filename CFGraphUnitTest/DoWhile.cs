using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static CFGraphUnitTest.Helper.Helper;

namespace CFGraphUnitTest
{
    [TestClass]
    public class DoWhile
    {
        [TestMethod]
        public void DoWhile1()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	do {
		TemperatureType--;
    } while (TemperatureType > 0);
}
";

            var result = new int[4, 4]
            {
                {0, 1, 0, 0},
                {0, 0, 1, 0},
                {0, 1, 0, 1},
                {0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void DoWhile2()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	do {
		TemperatureType--;
    } while (TemperatureType > 0);
    TemperatureType++;
}
";

            var result = new int[5, 5]
            {
                {0, 1, 0, 0, 0},
                {0, 0, 1, 0, 0},
                {0, 1, 0, 1, 0},
                {0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void DoWhile3()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	do {
		TemperatureType--;
    } while (TemperatureType > 0);
    TemperatureType++;
    TemperatureType--;
}
";

            var result = new int[6, 6]
            {
                {0, 1, 0, 0, 0, 0},
                {0, 0, 1, 0, 0, 0},
                {0, 1, 0, 1, 0, 0},
                {0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void DoWhile4()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	do {
		TemperatureType--;
		TemperatureType--;
    } while (TemperatureType > 0);
    TemperatureType--;
}
";

            var result = new int[6, 6]
            {
                {0, 1, 0, 0, 0, 0},
                {0, 0, 1, 0, 0, 0},
                {0, 0, 0, 1, 0, 0},
                {0, 1, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void DoWhile5()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	do {
		if(TemperatureType%2==1)
            TemperatureType--;
     } while (TemperatureType > 0);
    TemperatureType--;
}
";

            var result = new int[6, 6]
            {
                {0, 1, 0, 0, 0, 0},
                {0, 0, 1, 1, 0, 0},
                {0, 0, 0, 1, 0, 0},
                {0, 1, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void DoWhile6()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	do {
		if(TemperatureType%2==1)
            TemperatureType--;
		TemperatureType--;
     } while (TemperatureType > 0);
TemperatureType--;
}
";

            var result = new int[7, 7]
            {
                {0, 1, 0, 0, 0, 0, 0},
                {0, 0, 1, 1, 0, 0, 0},
                {0, 0, 0, 1, 0, 0, 0},
                {0, 0, 0, 0, 1, 0, 0},
                {0, 1, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }
    }
}
