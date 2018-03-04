using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static CFGraphUnitTest.Helper.Helper;

namespace CFGraphUnitTest
{
    [TestClass]
    public class Switch
    {
        [TestMethod]
        public void Switch1()
        {
            var code = @"
void ReadTemperature(const int TemperatureType){
	switch (TemperatureType){
	case 1: 
    {
		/*2*/   printf(""Temperature type - Hot, Temperature = %d\n"", TemperatureType);
	}
	break;
	case 2:
	{
		/*3*/      printf(""Temperature type - Normal, Temperature = %d\n"", TemperatureType);
	}
	break;
	default:
	{
		/*4*/      printf(""Temperature type - Cold, Temperature = %d\n"", TemperatureType);
	}
	break;
	}
}
";

            var result = new int[6, 6]
            {
                {0, 1, 0, 0, 0, 0},
                {0, 0, 1, 1, 1, 0},
                {0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void Switch2()

        {
            var code = @"
void ReadTemperature(const int TemperatureType)
{
	/*1*/   switch (TemperatureType) 
	{
	case 1:
	{
		/*2*/   printf(""Temperature type - Hot, Temperature = %d\n"", TemperatureType);
	}
	break;
	case 2:
	{
		/*3*/      printf(""Temperature type - Normal, Temperature = %d\n"", TemperatureType);
	}
	break;
	default:
	{
		/*4*/      printf(""Temperature type - Cold, Temperature = %d\n"", TemperatureType);
	}
	break;
	}
/*5*/   int a = 0;
}
";

            var result = new int[7, 7]
            {
                {0, 1, 0, 0, 0, 0, 0},
                {0, 0, 1, 1, 1, 0, 0},
                {0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }


        [TestMethod]
        public void Switch3()
        {
            var code = @"
void ReadTemperature(const int TemperatureType)
{
	/*1*/   switch (TemperatureType) 
	{
	case 1:
	{
		/*2*/   printf(""Temperature type - Hot, Temperature = %d\n"", TemperatureType);
	}
	break;
	case 2:
	{
		/*3*/      printf(""Temperature type - Normal, Temperature = %d\n"", TemperatureType);
	}
	break;
	}
/*4*/   int a = 0;
}
";

            var result = new int[6, 6]
            {
                {0, 1, 0, 0, 0, 0},
                {0, 0, 1, 1, 1, 0},
                {0, 0, 0, 0, 1, 0},
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
        public void Switch4()

        {
            var code = @"
void ReadTemperature(const int TemperatureType)
{
/*1*/   switch (TemperatureType) 
	    {
    	    case 1:
	        {
/*2*/          printf(""Temperature type - Hot, Temperature = %d\n"", TemperatureType);
	        }
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
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void Switch5()
        {
            var code = @"
void ReadTemperature(const int TemperatureType)
{
/*1*/	switch (TemperatureType) {
		case 1: {
/*2*/		if (TemperatureType + 1 == 3)
/*3*/			printf(""Temperature type - Hot, Temperature = %d\n"", TemperatureType - 1);
			else
/*4*/			printf(""Temperature type - Hot, Temperature = %d\n"", TemperatureType + 1);
		}
				break;
		case 2:
/*5*/		if (TemperatureType + 1 == 3) {
/*6*/			printf(""Temperature type - Hot, Temperature = %d\n"", TemperatureType - 1);
			}
			else {
/*7*/			printf(""Temperature type - Hot, Temperature = %d\n"", TemperatureType + 1);
			}
			break;
		default: {
/*8*/		if (0 == 0)
/*9*/			printf(""Temperature type - Hot, Temperature = %d\n"", TemperatureType - 1);
		}
		break;
		}
/*10*/	int a = 0;
}
";

            var result = new int[12, 12]
            {
                {0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0, 1, 0, 0, 1, 0, 0, 0},
                {0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }


        [TestMethod]
        public void Switch6()
        {
            var code = @"
        void ReadTemperature(const int TemperatureType)
        {
        /*1*/   switch (TemperatureType) 
        	    {
            	    case 1:
                    case 2:
        	        {
        /*2*/          printf(""Temperature type - Hot, Temperature = %d\n"", TemperatureType);
        	        }
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
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }

        [TestMethod]
        public void SwitchMin()
        {
            var code = @"
void ReadTemperature(const int TemperatureType)
{
/*1*/   switch (TemperatureType){
            case 1:
/*2*/           printf(""Temperature type - Hot, Temperature = %f\n"", TemperatureType);
		        break;
            default:
/*3*/           printf(""Temperature type - Cold, Temperature = %f\n"", TemperatureType);
		        break;
	    }
};";

            var result = new int[5, 5]
            {
                {0, 1, 0, 0, 0},
                {0, 0, 1, 1, 0},
                {0, 0, 0, 0, 1},
                {0, 0, 0, 0, 1},
                {0, 0, 0, 0, 0}
            };

            var matrix = GetMatrixFromCode(code);
            var message = Environment.NewLine + MatrixToString(result) + Environment.NewLine +
                          MatrixToString(matrix);
            Assert.IsTrue(Compare(matrix, result), message);
        }
    }
}