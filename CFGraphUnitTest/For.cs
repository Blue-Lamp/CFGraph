using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static CFGraphUnitTest.Helper.Helper;

namespace CFGraphUnitTest
{
    [TestClass]
    public class For
    {
        [TestMethod]
        public void For1()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	for(int i = 0; i < TemperatureType; i++)
		TemperatureType--;
} 
";

            var result = new int[3, 3]
            {
                {0, 1, 1},
                {1, 0, 0},
                {0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void For2()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	for(int i = 0; i < TemperatureType; i++)
		TemperatureType--;
    TemperatureType++;
}
";

            var result = new int[4, 4]
            {
                {0, 1, 1, 0},
                {1, 0, 0, 0},
                {0, 0, 0, 1},
                {0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void For3()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	for(int i = 0; i < TemperatureType; i++)
		TemperatureType--;
    TemperatureType++;
    TemperatureType--;
}
";

            var result = new int[5, 5]
            {
                {0, 1, 1, 0, 0},
                {1, 0, 0, 0, 0},
                {0, 0, 0, 1, 0},
                {0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void For4()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	for(int i = 0; i < TemperatureType; i++){
		TemperatureType--;
		TemperatureType--;
    }
    TemperatureType--;
}
";

            var result = new int[5, 5]
            {
                {0, 1, 0, 1, 0},
                {0, 0, 1, 0, 0},
                {1, 0, 0, 0, 0},
                {0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void For5()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	for(int i = 0; i < TemperatureType; i++)
		if(TemperatureType%2==1)
            TemperatureType--;
    TemperatureType--;
}
";

            var result = new int[5, 5]
            {
                {0, 1, 0, 1, 0},
                {1, 0, 1, 0, 0},
                {1, 0, 0, 0, 0},
                {0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void For6()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	for(int i = 0; i < TemperatureType; i++){
		if(TemperatureType%2==1)
            TemperatureType--;
		TemperatureType--;
    }
TemperatureType--;
}
";

            var result = new int[6, 6]
            {
                {0, 1, 0, 0, 1, 0},
                {0, 0, 1, 1, 0, 0},
                {0, 0, 0, 1, 0, 0},
                {1, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }
    }
}