using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Text;

namespace methodAnalysisHierarchies
{
    public class SimplexMethodInitializer
    {
        private IReader _reader;
        private SimplexMethod _simplexMethod;
        public SimplexMethodInitializer(IReader reader, SimplexMethod simplexMethod) 
        {
            _reader = reader;
            _simplexMethod = simplexMethod;
        }
        public SimplexMethodInitializer(SimplexMethod simplexMethod)
        {
            _simplexMethod = simplexMethod;
        }
        public void InitSimplexTableFromCode()
        {
            //for minimization problem all the cj-zj>=0 / zj-cj<=0
            //for maximization problem all the cj-zj<=0 / zj-cj>=0

            //var matrixCoefs = new double[,]
            //{
            //    {10, 20, 1, 0 },
            //    {8,   8, 0, 1 }
            //};
            //var solutionCoefs = new double[] { 120, 80 };
            //var maximizationFunctionCoefs = new double[] { 12, 16, 0, 0 };
            //Func<double, int, bool> greaterThanOrEqual = (first, second) => first <= second;
            //var solutionTable = _simplexMethod.Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, true);


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

            var matrixCoefs = new double[,]
            {
                {   -1,  -4,   0,   0, 1,   0,   0,   0, },
                {  -1,  4,   0,   0,   0,   1,   0,   0, },
                {   2,  3,   0,   0,   0,   0,   1,   0, },
                {   4,  0,  -1,   1,   0,   0,   0,   1, },
                {  -2,  0,   1,   1,   0,   0,   0,   0, }
            };
            var solutionCoefs = new double[] {  -7, 25, 30, 20, 11 };
            var maximizationFunctionCoefs = new double[] { -4, -1, 0, 0, 0, 0, 0, 0 };


            Func<double, int, bool> greaterThanOrEqual = (first, second) => first >= second;
            var solutionTable = _simplexMethod
                .Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, false);


            //var matrixCoefs = new double[,]
            //{
            //    {    3, -1,  2, 1, 0, 0 },
            //    {   -2, -4,  0, 0, 1, 0 },
            //    {   -4,  3,  8, 0, 0, 1 }
            //};
            //var solutionCoefs = new double[] { 7, 12, 10 };
            //var maximizationFunctionCoefs = new double[] { 2, -3, 6, 0, 0, 0 };
            //Func<double, int, bool> greaterThanOrEqual = (first, second) => first >= second;
            //var solutionTable = _simplexMethod.Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, false);



            //var matrixCoefs = new double[,]
            //{
            //    {-2,  -1,   1,   0,    0,   0,   0 },
            //    {-1,  -4,   0,   1,    0,   0,   0,},
            //    {2,   -1,   0,   0,    1,   0,   0,},
            //    {2,    3,   0,   0,    0,   1,   0, },
            //    {-1,   4,   0,   0,    0,   0,   1, }
            //};
            //var solutionCoefs = new double[] { -5, -6, 16, 32, 28 };

            //var matrixCoefs = new double[,]
            //{
            //    {2,   1,  -1,   0,    0,   0,   0 },
            //    {1,    4,   0,  -1,    0,   0,   0,},
            //    {2,   -1,   0,   0,    1,   0,   0,},
            //    {2,    3,   0,   0,    0,   1,   0, },
            //    {-1,   4,   0,   0,    0,   0,   1, }
            //};
            //var solutionCoefs = new double[] { 5, 6, 16, 32, 28 };


            //var maximizationFunctionCoefs = new double[] { 1, -6, 0, 0, 0, 0, 0 };
            //Func<double, int, bool> greaterThanOrEqual = (first, second) => first <= second;
            //var solutionTable = _simplexMethod.Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, true);
        }
        public void InitSimplexTableFromConsole()
        {
            Console.WriteLine("Enter coefficients of simplex table in Canonical form of a set of linear equations");
            (var rows, var cols) = _reader.ReadRowsAndCols();
            var matrix2d = _reader.ReadMatrix(rows,cols);
            var printer = new MatrixConsolePrinter();
            printer.Print(matrix2d);
            Console.WriteLine("Enter solution coefficients");
            var solutionCoefs = _reader.ReadRow(rows);
            printer.Print(solutionCoefs.ToArray());

            Console.WriteLine("Enter maximization Function Coefs coefficients");
            var maximizationFunctionCoefs = _reader.ReadRow(cols);
            printer.Print(maximizationFunctionCoefs.ToArray());
            Console.WriteLine("Maximization or minimization (Enter max or min)");
            var maxOrMin = Console.ReadLine();

            if (maxOrMin.Trim().Equals("max"))
            {
                Func<double, int, bool> greaterThanOrEqual = (first, second) => first <= second;
                var solutionTable = 
                    _simplexMethod
                    .Solve(matrix2d, solutionCoefs.ToArray(), maximizationFunctionCoefs.ToArray(), greaterThanOrEqual, true);
            }
            else if (maxOrMin.Trim().Equals("min")) 
            {
                Func<double, int, bool> greaterThanOrEqual = (first, second) => first >= second;
                var solutionTable =
                    _simplexMethod
                    .Solve(matrix2d, solutionCoefs.ToArray(), maximizationFunctionCoefs.ToArray(), greaterThanOrEqual, false);
            }
            else
            {
                Console.WriteLine("Not correct");
            }

        }
    }
}
