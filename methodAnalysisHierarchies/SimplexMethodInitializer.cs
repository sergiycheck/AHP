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

        }
        public void ToCanonicalForm() 
        {

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
