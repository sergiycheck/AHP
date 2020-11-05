using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using methodAnalysisHierarchies;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace MatrixTests
{
    public class MatrixTests
    {
        [SetUp]
        public void Setup()
        {
        }
        public CultureInfo FormatProvider {
            get {
                var formatProvider = (CultureInfo)CultureInfo.InvariantCulture.Clone();
                formatProvider.TextInfo.ListSeparator = " ";
                return formatProvider;
            } }
        
        public Matrix<double> GetMatrix(int rows,int cols) 
        {
            var matrix = Matrix<double>.Build.Dense(rows, cols);
            var k = 0.0;
            for (var i = 0; i < matrix.RowCount; i++)
            {
                for (var j = 0; j < matrix.ColumnCount; j++)
                {
                    matrix[i, j] = k++;
                }
            }
            return matrix;
        }
        [Test]
        public void GenerateMatrixTest() 
        {
            Matrix<double> m = Matrix<double>.Build.Random(3, 4);
            Console.WriteLine(m);
        }
        [Test]
        public void GenerateMatrixTest2() 
        {
            var random = new Random();
            var rows = 5;
            var cols = 7;
            var maxValueParsed = 10.0;
            var minValueParsed = -10.0;
            var matrix2d = new double[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix2d[i, j] =Math.Round( random.NextDouble() * (maxValueParsed - minValueParsed) + minValueParsed,2);
                }
            }

            Matrix<double> m = Matrix<double>.Build.DenseOfArray(matrix2d);
            Console.WriteLine(m);
        }



        [Test]
        public void MatrixCreation2() 
        {
            var matrix = GetMatrix(3,5);
            Console.WriteLine(matrix);
        } 
        [Test]
        public void MatrixCreation()
        {
            // Create square matrix
            var matrix = new DenseMatrix(5);
            var k = 0;
            for (var i = 0; i < matrix.RowCount; i++)
            {
                for (var j = 0; j < matrix.ColumnCount; j++)
                {
                    matrix[i, j] = k++;
                }
            }
            Console.WriteLine(matrix);
        }
        [Test]
        public void InsertVector() 
        {
            var matrix = GetMatrix(3, 5);
            Console.WriteLine(matrix);
            // Create vector
            var vector = new DenseVector(new[] { 50.0, 51.0, 52.0, 53.0, 54.0 });
            Console.WriteLine(@"Sample vector");
            Console.WriteLine(vector.ToString("#0.00\t", FormatProvider));
            Console.WriteLine();

            // 1. Insert new column
            var result = matrix.InsertRow(3, vector);
            Console.WriteLine(@"1. Insert new column");
            Console.WriteLine(result.ToString("#0.00\t", FormatProvider));
            Console.WriteLine();
        }
        [Test]
        public void IncreaseDimensions()
        {
            var matrix = GetMatrix(3, 5);
            Console.WriteLine(matrix.ToString("#0.00\t", FormatProvider));

            var newMatrix = new DenseMatrix(matrix.RowCount+1, matrix.ColumnCount+1);
            //var newMatrix = Matrix<double>.Build.DenseDiagonal(matrix.RowCount + 1, matrix.ColumnCount + 1,0);
            newMatrix.SetSubMatrix(0, 0, matrix);
            Console.WriteLine(newMatrix.ToString("#0.00\t", FormatProvider));

            var vectorCol = new DenseVector(new[] { 50.0, 51.0, 52.0 });
            newMatrix.SetColumn(newMatrix.ColumnCount - 1, 0, vectorCol.Count, vectorCol);
            Console.WriteLine(newMatrix.ToString("#0.00\t", FormatProvider));

            var vectorRow = new DenseVector(new[] { 60.0, 61.0, 62.0 });
            newMatrix.SetRow(newMatrix.RowCount - 1, 0, vectorRow.Count, vectorRow);
            Console.WriteLine(newMatrix.ToString("#0.00\t", FormatProvider));

            var vectorlastRow = newMatrix.Row(newMatrix.RowCount - 1);
            Console.WriteLine(vectorlastRow.ToString("#0.00\t", FormatProvider));
            var vectorlastColumn = newMatrix.Column(newMatrix.ColumnCount - 1);
            Console.WriteLine(vectorlastColumn.ToString("#0.00\t", FormatProvider));
        }
        [Test]
        public void ZipVectors() 
        {
            var vectorCol = new DenseVector(new[] { 50.0, 51.0, 52.0 });
            var vectorRow = new DenseVector(new[] { 60.0, 61.0, 62.0 });
            var matrix = Matrix<double>.Build.DenseOfRowVectors(vectorCol);
            Console.WriteLine(matrix);
            var appmatrx = Matrix<double>.Build.DenseOfRowVectors(vectorRow);
            var newMatrix = matrix.Append(appmatrx);
            Console.WriteLine(newMatrix);
            Assert.AreEqual(vectorCol.Count + vectorRow.Count, newMatrix.RowCount * newMatrix.ColumnCount);
        }

        //https://www.youtube.com/watch?v=M8POtpPtQZc&t=578s&ab_channel=KauserWise
        [Test]
        public void SimplexMethodTest1()
        {
            //F(X) = 120x1+80x2 -> max
            //10x1+20x2<=120
            //8x1+8x2<=80
            SimplexMethod _simplexMethod = new SimplexMethod(new MatrixConsolePrinter());
            var matrixCoefs = new double[,]
            {
                {10, 20, 1, 0 },
                {8,   8, 0, 1 }
            };
            var solutionCoefs = new double[] { 120, 80 };
            var maximizationFunctionCoefs = new double[] { 12, 16, 0, 0 };
            Func<double, int, bool> greaterThanOrEqual = (first, second) => first <= second;
            var solutionTable = _simplexMethod.Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, true);

            var solutionExpected = new double[,]
                {
                    {0,     1,     0.1,    -0.125,  2 },
                    {1,     0,    -0.1,     0.25,   8 },
                    {0,     16,    1.2,       -3,  -192}
                };
            
            //Assert.AreEqual(solutionExpected,solutionTable.ExtendedMatrix);
            Assert.AreEqual(128, solutionTable.ZiCoefs[solutionTable.ZiCoefs.Length - 1]);
            
        }
        
        //https://www.youtube.com/watch?v=SNc9NGCJmns&ab_channel=KauserWise      
        [Test]
        public void SimplexMethodTest2()
        {
            //F(X) = 2x1-3x2+6x3 -> min
            //3x1-1x2 2x3<=10
            //2x1+4x2>=-12
            //-4x1 + 3x2+8x3≤10

            SimplexMethod _simplexMethod = new SimplexMethod(new MatrixConsolePrinter());
            var matrixCoefs = new double[,]
            {
                {    3, -1,  2, 1, 0, 0 },
                {   -2, -4,  0, 0, 1, 0 },
                {   -4,  3,  8, 0, 0, 1 }
            };
            var solutionCoefs = new double[] { 7, 12, 10 };
            var maximizationFunctionCoefs = new double[] { 2, -3, 6, 0, 0, 0 };
            Func<double, int, bool> greaterThanOrEqual = (first, second) => first >= second;
            var solutionTable = _simplexMethod.Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, false);

            var solutionExpected = new double[,]
                {
                    {1,   0,	2.8	 ,   0.6,	 0,	    0.2	 ,   6.199 },	
                    {0,   0,	31.197,	4.399,	 1,	    2.798,	70.787},	
                    {0,   1,	6.399,	0.8	 ,   0,	    0.599,	11.596},	
                    {0,  -3,	0.401,	-1.2,    0,	    -0.4,	-2.397}
                    
                    
                };

            //Assert.AreEqual(solutionExpected, solutionTable.ExtendedMatrix);
            Assert.AreEqual(-22.39, solutionTable.ZiCoefs[solutionTable.ZiCoefs.Length - 1]);

        }
        [Test]
        public void SimplexMethodTest3()
        {
            SimplexMethod _simplexMethod = new SimplexMethod(new MatrixConsolePrinter());

            //working solution =-20
            //F(X) = -2x1-x2 -> min
            //3x1+2x2≥10
            //x1≤15
            //x1 + 2x2≤10
            //- 2x1 - 4x2 + x3 + x4 = 7
            //- 2x1 + x3 + 5x4 = 21

            var matrixCoefs = new double[,]
            {
                {  3,    2,   0,   0,  -1,   0,  0 },
                {  1,    0,   0,   0,   0,   1,  0 },
                {  1,    2,   0,   0,   0,   0,  1 },
                { -2,   -4,   1,   1,   0,   0,  0 },
                { -2,    0,   1,   5,   0,   0,  0 }
            };
            var solutionCoefs = new double[] { 10, 15, 10, 7, 21 };
            var maximizationFunctionCoefs = new double[] { -2, -1, 0, 0, 0, 0, 0 };


            Func<double, int, bool> greaterThanOrEqual = (first, second) => first >= second;
            var solutionTable = _simplexMethod
                .Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, false);

            var solutionExpected = new double[,]
                {
                    { 1,     2,     0,  0,  0,  0,  1,      10,     },
                    { 0,    -2,     0,  0,  0,  1,  -1,     5,      },
                    { 0,    4.003,  0,  0,  1,  0,  3.003,  20.021, },
                    { 0,    0.003,  1,  1,  0,  0,  2.003,  27.021, },
                    { 0,    4.003,  1,  5,  0,  0,  2.003,  41.021, },
                    {-2,    -1,     0,  0,  0,  0,  0,      6.667,  }


                };

            //Assert.AreEqual(solutionExpected, solutionTable.ExtendedMatrix);
            Assert.AreEqual(-20, solutionTable.ZiCoefs[solutionTable.ZiCoefs.Length - 1]);

        }
        [Test]
        public void SimplexMethodTest4() 
        {

            //F(X) = x1-6x2
            //2x1+x2≥5
            //x1 + 4x2≥6
            //2x1 - x2≤16
            //2x1 + 3x2≤32
            //- x1 + 4x2≤28

            var printer = new MatrixConsolePrinter();
            SimplexMethod _simplexMethod = new SimplexMethod(printer);
            var matrixCoefs = new double[,]
            {
                {2,   1,  -1,   0,    0,   0,   0 },
                {1,    4,   0,  -1,    0,   0,   0,},
                {2,   -1,   0,   0,    1,   0,   0,},
                {2,    3,   0,   0,    0,   1,   0, },
                {-1,   4,   0,   0,    0,   0,   1, }
            };
            var solutionCoefs = new double[] { 5, 6, 16, 32, 28 };


            var maximizationFunctionCoefs = new double[] { 1, -6, 0, 0, 0, 0, 0 };
            Func<double, int, bool> greaterThanOrEqual = (first, second) => first <= second;
            var solutionTable = _simplexMethod.Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, true);

            var solutionExpected = new double[,]
                {
                    {1 ,     -0.5,    0,       0,       0.5,     0,       0,       8   ,},
                    {0 ,     -2  ,    1,       0,       1  ,     0,       0,       11  ,},
                    {0 ,     -4.5,    0,       1,       0.5,     0,       0,       2   ,},
                    {0 ,     4   ,    0,       0,       -1 ,     1,       0,       16  ,},
                    {0 ,     3.5 ,    0,       0,       0.5,     0,       1,       36  ,},
                    {1 ,     -6  ,    0,       0,       0  ,     0,       0,       -2.5,}

                };

            //Assert.AreEqual(solutionExpected, solutionTable.ExtendedMatrix);
            Assert.AreEqual(8, solutionTable.ZiCoefs[solutionTable.ZiCoefs.Length - 1]);
        }
        [Test]
        public void SimplexMethodTest5() 
        {
            //lab2 sample 8
            //F(X) = 4x1+x2
            //x1+4x2≥7
            //-x1 + 4x2≤25
            //2x1 + 3x2≤30
            //4x1 - x3 + x4≤20
            //- 2x1 + x3 + x4 = 11

            var printer = new MatrixConsolePrinter();
            SimplexMethod _simplexMethod = new SimplexMethod(printer);

            var matrixCoefs = new double[,]
            {
                {  -1, -4,   0,   0,   1,   0,   0,   0, },
                {  -1,  4,   0,   0,   0,   1,   0,   0, },
                {   2,  3,   0,   0,   0,   0,   1,   0, },
                {   4,  0,  -1,   1,   0,   0,   0,   1, },
                {  -2,  0,   1,   1,   0,   0,   0,   0, }
            };
            var solutionCoefs = new double[] { -7, 25, 30, 20, 11 };
            var maximizationFunctionCoefs = new double[] { 4, 1, 0, 0, 0, 0, 0,0};//delete zero here error

            Func<double, int, bool> greaterThanOrEqual = (first, second) => first >= second;
            var solutionTable = _simplexMethod.Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, false);


            var solutionExpected = new double[,]
                {

                    {0.25 ,   1 ,  -0 , -0 , -0.25 , -0 , -0 , -0 , 1.75   ,},
                    {-2   ,   0 ,  0  , 0  , 1     ,  1 ,  0 ,  0 ,  18    ,},
                    {1.25 ,   0 ,  0  , 0  , 0.75  ,  0 ,  1 ,  0 ,  24.75 ,},
                    {4    ,   0 ,  -1 , 1  , 0     ,  0 ,  0 ,  1 ,  20    ,},
                    {-2   ,   0 ,  1  , 1  , 0     ,  0 ,  0 ,  0 ,  11    ,},
                    {3.75 ,   0 ,  0  , 0  , 0.25  ,  0 ,  0 ,  0 ,  -1.75 ,}
                };

            Assert.AreEqual(solutionExpected, solutionTable.ExtendedMatrix);

            Assert.AreEqual(1.75,Math.Round(solutionTable.ZiCoefs[solutionTable.ZiCoefs.Length - 1],2));
        }
        [Test]
        public void SimplexMethodTest6()
        {
            //F(X) = 3x1+2x2+x3
            //x1+x3≥4
            //2x1 + x2 + 2x3≥6
            //2x1 - x2 + 2x3≥2
            var printer = new MatrixConsolePrinter();
            SimplexMethod _simplexMethod = new SimplexMethod(printer);
            var matrixCoefs = new double[,]
            {
                {     -1,  0,   -1, 1,0,0},
                {     -2,  -1,   -2, 0, 1,0 },
                {     -2,  1,   -2 ,0 ,0, 1}
            };
            var solutionCoefs = new double[] { -4, -6, -2 };
            var maximizationFunctionCoefs = new double[] { 3, 2, 1,0,0,0 };
            Func<double, int, bool> greaterThanOrEqual = (first, second) => first >= second;
            var solutionTable = _simplexMethod.Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, false);
            Assert.AreEqual(4, Math.Round(solutionTable.ZiCoefs[solutionTable.ZiCoefs.Length - 1], 2));
        }
        [Test]
        public void SimplexMethodTest7() 
        {
            // F(X) = 4x1 + 2x2 + x3
            //- x1 - x2≤-10
            //2x1 + x2 - x3≤8
            var printer = new MatrixConsolePrinter();
            SimplexMethod _simplexMethod = new SimplexMethod(printer);
            var matrixCoefs = new double[,]
            {
                {     -1,  -1,   0, 1,0},
                {      2,   1,  -1, 0,1},
            };
            var solutionCoefs = new double[] { -10, 8};
            var maximizationFunctionCoefs = new double[] { 4, 2, 1, 0, 0};
            Func<double, int, bool> greaterThanOrEqual = (first, second) => first >= second;
            var solutionTable = _simplexMethod.Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, false);
            Assert.AreEqual(22, Math.Round(solutionTable.ZiCoefs[solutionTable.ZiCoefs.Length - 1], 2));
        }
        [Test]
        public void SimplexMethodTest8()
        {
            //  F(X) = x1+x3+3x4 
            //-2x1 - 5x2≤-15
            //8x2≤8
            //x1 + x2≤10
            //-2x1 - 2x2 + x3 + x4 = -3
            //-2x1 + x3 + 3x4 = 11
            var printer = new MatrixConsolePrinter();
            SimplexMethod _simplexMethod = new SimplexMethod(printer);
            var matrixCoefs = new double[,]
            {
                 { -2, -5,  0,  0,  1,  0,  0},
                 {  0,  8,  0,  0,  0,  1,  0},
                 {  1,  1,  0,  0,  0,  0,  1},
                 { -2, -2,  1,  1,  0,  0,  0},
                 { -2,  0,  1,  3,  0,  0,  0}

            };

            var solutionCoefs = new double[] { -15, 8,10,-3,11 };
            var maximizationFunctionCoefs = new double[] { 1, 0, 1, 3, 0,0,0 };
            Func<double, int, bool> greaterThanOrEqual = (first, second) => first >= second;
            var solutionTable = _simplexMethod.Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, false);
            Assert.AreEqual(26, Math.Round(solutionTable.ZiCoefs[solutionTable.ZiCoefs.Length - 1], 2));
        }
        [Test]
        public void SimplexMethodTest8SomeIterationsSkiped()
        {
           
            var printer = new MatrixConsolePrinter();
            SimplexMethod _simplexMethod = new SimplexMethod(printer);
            var matrixCoefs = new double[,]
            {
                 {-2,   -5, 0,  0,  1,  0,  0},
                 { 0,    8, 0,  0,  0,  1,  0},
                 { 1,    1, 0,  0,  0,  0,  1},
                 {-2,   -3, 1,  0,  0,  0,  0},
                 { 0,    1, 0,  1,  0,  0,  0}

            };

            var solutionCoefs = new double[] { -15, 8, 10, -10, 7 };
            var maximizationFunctionCoefs = new double[] { -3, 0, 0, 0, 0, 0,0 };
            Func<double, int, bool> greaterThanOrEqual = (first, second) => first >= second;
            var solutionTable = _simplexMethod.Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, false);
            Assert.AreEqual(26, Math.Round(solutionTable.ZiCoefs[solutionTable.ZiCoefs.Length - 1], 3));
        }
        [Test]
        public void SimplexMethodTest9()
        {
            //collegue task 
            //F(X) = x1+x2+x3
            //9x1 + 13x2 + 8x3≥1
            //3x1 + 10x2 + 8x3≥1
            //8x1 + 7x2 + 12x3≥1
            var printer = new MatrixConsolePrinter();
            SimplexMethod _simplexMethod = new SimplexMethod(printer);
            var matrixCoefs = new double[,]
            {
                 {-9,   -13,  -8 ,  1,   0,   0},
                 {-3,   -10,  -8 ,  0,   1,   0},
                 {-8,   -7 ,  -12,  0,   0,   1}

            };

            var solutionCoefs = new double[] { -1, -1, -1};
            var maximizationFunctionCoefs = new double[] { 1, 1, 1, 0, 0, 0};
            Func<double, int, bool> greaterThanOrEqual = (first, second) => first >= second;
            var solutionTable = _simplexMethod.Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, false);
            Assert.AreEqual(Math.Round(7.0/64.0,3), Math.Round(solutionTable.ZiCoefs[solutionTable.ZiCoefs.Length - 1], 3));
        }
        [Test]
        public void SimplexMethodTest10()
        {
            //F(X) = x1+x2+x3
            //9x1 + 13x2 + 8x3<=1
            //3x1 + 10x2 + 8x3<=1
            //8x1 + 7x2 + 12x3<=1
            var printer = new MatrixConsolePrinter();
            SimplexMethod _simplexMethod = new SimplexMethod(printer);
            var matrixCoefs = new double[,]
            {
                 { 9,    13,   8 ,  1,   0,   0},
                 { 3,    10,   8 ,  0,   1,   0},
                 { 8,    7 ,   12,  0,   0,   1}

            };

            var solutionCoefs = new double[] {  1,  1,  1 };
            var maximizationFunctionCoefs = new double[] { 1, 1, 1, 0, 0, 0 };
            Func<double, int, bool> greaterThanOrEqual = (first, second) => first <= second;
            var solutionTable = _simplexMethod.Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, true);
            Assert.AreEqual(Math.Round(5.0/44.0, 3), Math.Round(solutionTable.ZiCoefs[solutionTable.ZiCoefs.Length - 1], 3));
        }
        [Test]
        public void SimplexMethodTest11()
        {
            //Z(Y) = y1+y2+y3->max
            //2y2+7y3≤1
            //12y1 + 11y2 + y3≤1
            var printer = new MatrixConsolePrinter();
            SimplexMethod _simplexMethod = new SimplexMethod(printer);
            var matrixCoefs = new double[,]
            {
                 { 0,    2,   7 ,  1,   0},
                 { 12,    11,  1,  0,   1},

            };

            var solutionCoefs = new double[] { 1, 1 };
            var maximizationFunctionCoefs = new double[] {1, 1, 1, 0, 0 };
            Func<double, int, bool> greaterThanOrEqual = (first, second) => first <= second;
            var solutionTable = _simplexMethod.Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, true);
            Assert.AreEqual(Math.Round(3.0 / 14.0, 3), Math.Round(solutionTable.ZiCoefs[solutionTable.ZiCoefs.Length - 1], 3));
        }
        [Test]
        public void SimplexMethodTest12()
        {
            //12x2 ≥ 1
            //2x1 + 11x2 ≥ 1
            //7x1 + x2 ≥ 1
            //F(x) = x1 + x2 > min

            var printer = new MatrixConsolePrinter();
            SimplexMethod _simplexMethod = new SimplexMethod(printer);
            var matrixCoefs = new double[,]
            {
                 { 0,   -12,   1,  0,   0},
                 {-2,   -11,   0,  1,   0},
                 {-7,   -1,    0,  0,   1},

            };

            var solutionCoefs = new double[] { -1, -1, -1};
            var maximizationFunctionCoefs = new double[] { 1, 1, 0, 0, 0 };
            Func<double, int, bool> greaterThanOrEqual = (first, second) => first >= second;
            var solutionTable = _simplexMethod.Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, false);
            Assert.AreEqual(Math.Round(3.0 / 14.0, 3), Math.Round(solutionTable.ZiCoefs[solutionTable.ZiCoefs.Length - 1], 3));
        }





        [Test]
        public void SimplexMethodTestToCanonicalForm6() 
        {
            //lab2 sample 8
            //var matrixCoefs = new double[,]
            //{
            //    {     1,  4,   0,   0},
            //    {     1, -4,   0,   0},
            //    {    -2, -3,   0,   0 },
            //    {    -4,  0,   1,   -1},
            //    {    -2,  0,   1,   -1}
            //};
            //var solutionCoefs = new double[] { 7, -25, -30, -20, 11 };
            //var maximizationFunctionCoefs = new double[] { 4, 1 };
        }
        public void SimplexMethodDualMethodMinizization() 
        {

            //not working
            //var matrixCoefs = new double[,]
            //{
            //    {     1,  0,   1, -1,  0, 0 },
            //    {     2,  1,   2,  0, -1, 0 },
            //    {     2, -1,   2,  0,  0, -1 }
            //};
            //var solutionCoefs = new double[] {  4,  6,  2 };
            //var maximizationFunctionCoefs = new double[] { 3, 2, 1, 0, 0, 0 };


            //Func<double, int, bool> greaterThanOrEqual = (first, second) => first <= second;
            //var solutionTable = _simplexMethod
            //    .Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, true);

            //Z = 3x1+2x2+x3->min
            //x1+x3>=4
            //2x1+x2+2x3>=6
            //2x1-x2+2x3>=2
            //https://www.matburo.ru/Examples/Files/Simplex8.pdf

            //working solution = 4

            //var matrixCoefs = new double[,]
            //{
            //    {     1,  0,   1},
            //    {     2,  1,   2},
            //    {     2, -1,   2 }
            //};
            //var solutionCoefs = new double[] { 4, 6, 2 };
            //var maximizationFunctionCoefs = new double[] { 3, 2, 1 };



            //var matrix = Matrix<double>.Build.DenseOfArray(matrixCoefs);
            //var extendedMatrix = new DenseMatrix(matrix.RowCount + 1, matrix.ColumnCount + 1);

            //extendedMatrix.SetSubMatrix(0, 0, matrix);

            //var vectorSolutionCoefs = Vector<double>.Build.DenseOfArray(solutionCoefs);
            //var vectormaximizationFunctionCoefs = Vector<double>.Build.DenseOfArray(maximizationFunctionCoefs);
            //extendedMatrix.SetColumn(extendedMatrix.ColumnCount - 1, 0, vectorSolutionCoefs.Count, vectorSolutionCoefs);
            //extendedMatrix.SetRow(extendedMatrix.RowCount - 1, 0, vectormaximizationFunctionCoefs.Count, vectormaximizationFunctionCoefs);
            //Console.WriteLine(extendedMatrix);

            //var transposedMatrix = extendedMatrix.Transpose();
            //Console.WriteLine(transposedMatrix);
            //solutionCoefs = transposedMatrix.Column(transposedMatrix.ColumnCount - 1, 0, transposedMatrix.RowCount - 1).ToArray();
            //maximizationFunctionCoefs = transposedMatrix.Row(transposedMatrix.RowCount - 1, 0, transposedMatrix.ColumnCount).ToArray();

            //var matrixSolutionCoefs = Matrix<double>.Build.DenseOfRowVectors(Vector<double>.Build.DenseOfArray(solutionCoefs));
            //Console.WriteLine(matrixSolutionCoefs);
            //var matrixMaximizationFunctionCoefs = Matrix<double>.Build.DenseOfRowVectors(Vector<double>.Build.DenseOfArray(maximizationFunctionCoefs));
            //Console.WriteLine(matrixMaximizationFunctionCoefs);

            //var transposedMatrixWithoutSolutions = new DenseMatrix(transposedMatrix.RowCount, transposedMatrix.ColumnCount - 1);
            //transposedMatrixWithoutSolutions.SetSubMatrix(0, transposedMatrix.RowCount, 0, transposedMatrix.ColumnCount - 1, transposedMatrix);
            //Console.WriteLine(transposedMatrixWithoutSolutions);

            //var numberofRows = transposedMatrixWithoutSolutions.RowCount;
            //var canonicalMatrixList = new List<List<double>>();
            //for (int i = 0; i < numberofRows; i++)
            //{
            //    var rowList = new List<double>();
            //    var canonicalFormCoefs = new double[numberofRows];
            //    canonicalFormCoefs[i] = 1;
            //    for (int j = 0; j < transposedMatrixWithoutSolutions.ColumnCount; j++)
            //    {
            //        rowList.Add(transposedMatrixWithoutSolutions[i, j]);
            //    }
            //    rowList.AddRange(canonicalFormCoefs);
            //    canonicalMatrixList.Add(rowList);
            //}
            //var resultCanonicalMatrix = Matrix<double>.Build.DenseOfRows(canonicalMatrixList);
            //Console.WriteLine(resultCanonicalMatrix);

            //var CanonicalMatrix = new DenseMatrix(resultCanonicalMatrix.RowCount - 1, resultCanonicalMatrix.ColumnCount - 1);
            //CanonicalMatrix.SetSubMatrix(0, resultCanonicalMatrix.RowCount - 1, 0, resultCanonicalMatrix.ColumnCount - 1, resultCanonicalMatrix);
            //Console.WriteLine(CanonicalMatrix);
            //var maximizationFuncCoefsMatrix = resultCanonicalMatrix.Row(resultCanonicalMatrix.RowCount - 1, 0, resultCanonicalMatrix.ColumnCount - 1);
            //Console.WriteLine(maximizationFuncCoefsMatrix);
            //Console.WriteLine(matrixSolutionCoefs);


            //Func<double, int, bool> greaterThanOrEqual = (first, second) => first <= second;
            //var solutionTable = _simplexMethod
            //    .Solve(CanonicalMatrix.ToArray(), matrixSolutionCoefs.ToRowMajorArray(), maximizationFuncCoefsMatrix.ToArray(), greaterThanOrEqual, true);
        }
    }
}