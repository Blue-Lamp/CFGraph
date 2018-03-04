using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static CFGraphUnitTest.Helper.Helper;

namespace CFGraphUnitTest
{
    [TestClass]
    public class IfElse
    {
        [TestMethod]
        public void If1()
        {
            const string code = @"
void ReadTemperature (average){
	if (average > 40)
	{
		average = 1;
	}
}
";
            var result = new int[4, 4]
            {
                {0, 1, 0, 0},
                {0, 0, 1, 1},
                {0, 0, 0, 1},
                {0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine + MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void If2()
        {
            const string code = @"
void ReadTemperature (average){
	if (average > 40)
		average = 1;
}
";
            var result = new int[4, 4]
            {
                {0, 1, 0, 0},
                {0, 0, 1, 1},
                {0, 0, 0, 1},
                {0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine + MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void If3()
        {
            const string code = @"
void ReadTemperature (average){
	if (average > 40)
	{
		average = 1;
	}
    average++;
}
";
            var result = new int[5, 5]
            {
                {0, 1, 0, 0, 0},
                {0, 0, 1, 1, 0},
                {0, 0, 0, 1, 0},
                {0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine + MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void If4()
        {
            const string code = @"
void ReadTemperature (average){
	if (average > 40)
		average = 1;
    average++;
}
";
            var result = new int[5, 5]
            {
                {0, 1, 0, 0, 0},
                {0, 0, 1, 1, 0},
                {0, 0, 0, 1, 0},
                {0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine + MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void If5()
        {
            const string code = @"
void ReadTemperature (average){
	if (average > 40)
		average = 1;
    average++;
    average--;
}
";
            var result = new int[6, 6]
            {
                {0, 1, 0, 0, 0, 0},
                {0, 0, 1, 1, 0, 0},
                {0, 0, 0, 1, 0, 0},
                {0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine + MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void IfElse1()
        {
            const string code = @"
void ReadTemperature (average){
	if (average > 40)
	{
		average = 1;
	}else{
        average = 2;
    }
}
";
            var result = new int[5, 5]
            {
                {0, 1, 0, 0, 0},
                {0, 0, 1, 1, 0},
                {0, 0, 0, 0, 1},
                {0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine + MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void IfElse2()
        {
            const string code = @"
void ReadTemperature (average){
	if (average > 40)
		average = 1;
	else
        average = 2;
}
";
            var result = new int[5, 5]
            {
                {0, 1, 0, 0, 0},
                {0, 0, 1, 1, 0},
                {0, 0, 0, 0, 1},
                {0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine + MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void IfElse3()
        {
            const string code = @"
void ReadTemperature (average){
	if (average > 40)
	{
		average = 1;
	}else{
        average = 2;
    }
    average++;
}
";
            var result = new int[6, 6]
            {
                {0, 1, 0, 0, 0, 0},
                {0, 0, 1, 1, 0, 0},
                {0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine + MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void IfElse4()
        {
            const string code = @"
void ReadTemperature (average){
	if (average > 40)
		average = 1;
	else
        average = 2;
    average++;
}
";
            var result = new int[6, 6]
            {
                {0, 1, 0, 0, 0, 0},
                {0, 0, 1, 1, 0, 0},
                {0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine + MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void IfElse5()
        {
            var code = @"
void ReadTemperature (average){
	if (average > 40)
		average = 1;
	else
        average = 2;
    average++;
    average--;
}
}
";
            var result = new int[7, 7]
            {
                {0, 1, 0, 0, 0, 0, 0},
                {0, 0, 1, 1, 0, 0, 0},
                {0, 0, 0, 0, 1, 0, 0},
                {0, 0, 0, 0, 1, 0, 0},
                {0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine + MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void IfElseIf()
        {
            const string code = @"
void ReadTemperature (average){
/*1*/	if (average > 40)
/*2*/		average = 1;
        else 
/*3*/       if (average > 40)
/*4*/           average = 1;
/*5*/    average = 2;       
}
";
            var result = new int[7, 7]
            {
                {0, 1, 0, 0, 0, 0, 0},
                {0, 0, 1, 1, 0, 0, 0},
                {0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 1, 1, 0},
                {0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine + MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void IfElseIf2()
        {
            const string code = @"
short ReadTemperature (void){
/*1*/   if (average > 40){
/*2*/		TemperatureType = 1;
	    }
/*3*/	else if ((average < 40) &&
    			 (average > 10)){
/*4*/		TemperatureType = 2;
	    }
	    else{
/*5*/		TemperatureType = 3;
	    }
}
";
            var result = new int[7, 7]
            {
                {0, 1, 0, 0, 0, 0, 0},
                {0, 0, 1, 1, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 1, 1, 0},
                {0, 0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine + MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void IfElseIf3()
        {
            const string code = @"
short ReadTemperature (void){
        if (average > 40){
		    TemperatureType = 1;
	    }
	    else if ((average < 40) &&
			 (average > 10)){
		    TemperatureType = 2;
	    } else {
		    TemperatureType = 3;
        }
        average++;
}
";
            var result = new int[8, 8]
            {
                {0, 1, 0, 0, 0, 0, 0, 0},
                {0, 0, 1, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 1, 1, 0, 0},
                {0, 0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine + MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void IfIfElse5()
        {
            const string code = @"
void ReadTemperature (average){
/*1*/	if (average > 40){
/*2*/       if (average > 40)
/*3*/		    average = 1;
/*4*/		average = 1;
        }else
/*5*/       average = 2;
/*6*/   average++;
/*7*/   average--;
/*8*/   average--;
}
";
            var result = new int[10, 10]
            {
                {0, 1, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0, 1, 0, 0, 0, 0},
                {0, 0, 0, 1, 1, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 1, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 1, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine + MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }
    }
}