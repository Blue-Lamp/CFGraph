using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static CFGraphUnitTest.Helper.Helper;

namespace CFGraphUnitTest
{
    [TestClass]
    public class While
    {
        [TestMethod]
        public void While1()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	while (TemperatureType > 0)
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
        public void While2()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	while (TemperatureType > 0)
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
        public void While3()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	while (TemperatureType > 0)
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
        public void While4()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	while (TemperatureType > 0){
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
        public void While5()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	while (TemperatureType > 0)
		if(TemperatureType==5)
            break;
        else
            TemperatureType--;
} 
";

            var result = new int[4, 4]
            {
                {0, 1, 0, 1},
                {0, 0, 1, 0},
                {1, 0, 0, 0},
                {0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void WhileIf1()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	while (TemperatureType > 0)
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
        public void WhileIf2()
        {
            var code = @"
void ReadTemperature(int TemperatureType)
{
	while (TemperatureType > 0){
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