using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public SimplexTableElementsContainer ToCanonicalForm(InequalitySystem inequalitySystem) 
        {
            var countOfadditionalVariables = inequalitySystem.Inequalities.Count;
            foreach (var inequality in inequalitySystem.Inequalities)
            {
                if (inequality.Sign.Equals(Inequality.SignsCollection[1].ToString())) 
                {
                    countOfadditionalVariables -= 1;
                }
            }
            
            var additionalVariableList = new double[countOfadditionalVariables];
            var zeroes = new double[countOfadditionalVariables];
            var listOfListsOfCoefs = new List<List<double>>();
            var listOfSolutions = new List<double>();
            for (var i=0;i< inequalitySystem.Inequalities.Count; i++)
            {
                if (inequalitySystem.Inequalities[i].Sign.Equals(Inequality.SignsCollection[0].ToString()))
                {
                    additionalVariableList[i] = 1;
                    inequalitySystem.Inequalities[i].Coefs.AddRange(additionalVariableList);
                    
                }
                else
                if (inequalitySystem.Inequalities[i].Sign.Equals(Inequality.SignsCollection[1].ToString()))
                {

                    inequalitySystem.Inequalities[i].Coefs.AddRange(zeroes);
                }
                else
                if (inequalitySystem.Inequalities[i].Sign.Equals(Inequality.SignsCollection[2].ToString())) 
                {
                    additionalVariableList[i] = 1;
                    for (int j = 0; j < inequalitySystem.Inequalities[i].Coefs.Count; j++)
                    {
                        inequalitySystem.Inequalities[i].Coefs[j] *= -1.0;
                    }
                    inequalitySystem.Inequalities[i].Coefs.AddRange(additionalVariableList);
                    inequalitySystem.Inequalities[i].Solution *= -1.0;
                }
                Console.WriteLine($"{i} row coefs \t {string.Concat(inequalitySystem.Inequalities[i].Coefs.Select(coef=>string.Format("\t{0}",coef))) }");
                listOfListsOfCoefs.Add(inequalitySystem.Inequalities[i].Coefs);
                listOfSolutions.Add(inequalitySystem.Inequalities[i].Solution);
                additionalVariableList = new double[countOfadditionalVariables];

            }

            Console.WriteLine($"solutions {listOfSolutions.Count} \t {string.Concat(listOfSolutions.Select(coef => string.Format("\t{0}", coef)))}");

            var countOfAllElements = inequalitySystem.Inequalities.FirstOrDefault().Coefs.Count;
            var zerostoAddToTargetFunction = countOfAllElements - inequalitySystem.TargetFunctionCoefs.Count;
            inequalitySystem.TargetFunctionCoefs.AddRange(new double[zerostoAddToTargetFunction]);

            Console.WriteLine($"target function coefs \t {string.Concat(inequalitySystem.TargetFunctionCoefs.Select(coef => string.Format("\t{0}", coef)))}");

            return new SimplexTableElementsContainer()
            {
                MatrixCoefs = Matrix<double>.Build.DenseOfRows(listOfListsOfCoefs).ToArray(),
                 Solutions = listOfSolutions.ToArray(),
                  TargetFunctionCoefs = inequalitySystem.TargetFunctionCoefs.ToArray()
            };

        }
        public SimplexTableElementsContainer SolveSimplexMin(SimplexTableElementsContainer initialSimTabElCont) 
        {

            Func<double, int, bool> greaterThanOrEqual = (first, second) => first >= second;
            var solutionTable = _simplexMethod.Solve(initialSimTabElCont.MatrixCoefs, initialSimTabElCont.Solutions, initialSimTabElCont.TargetFunctionCoefs, greaterThanOrEqual, false);
            return solutionTable;
        }
        public SimplexTableElementsContainer SolveSimplexMax(SimplexTableElementsContainer initialSimTabElCont) 
        {

            Func<double, int, bool> greaterThanOrEqual = (first, second) => first <= second;
            var solutionTable = _simplexMethod.Solve(initialSimTabElCont.MatrixCoefs, initialSimTabElCont.Solutions, initialSimTabElCont.TargetFunctionCoefs, greaterThanOrEqual, true);
            return solutionTable;
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
