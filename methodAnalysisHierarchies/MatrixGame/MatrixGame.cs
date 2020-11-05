using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace methodAnalysisHierarchies.MatrixGame
{
    public class MatrixGame
    {
        public int  ADecisionCount { get; private set; }
        public int  BDecisionCount { get; private set; }
        public double  MinValueToSubstractAtTheEnd { get; private set; }

        public double GamePrice { get; set; }
        public List<double> PlayerAGameStrategy { get; set; }
        public List<double> PlayerBGameStrategy { get; set; }
        public SimplexMethodInitializer Initializer { get; private set; }
        public MatrixGame() { }
        public MatrixGame(SimplexMethodInitializer initializer) 
        {
            Initializer = initializer;
        }


        public bool CheckForSaddlePoint(double[,] matrixOfMatrixGame) 
        {
            var numericsMatrix = Matrix<double>.Build.DenseOfArray(matrixOfMatrixGame);

            var extendedMatrix = Matrix<double>.Build.DenseDiagonal(numericsMatrix.RowCount + 1, numericsMatrix.ColumnCount + 1, 0);
            extendedMatrix.SetSubMatrix(0, 0, numericsMatrix);
            for (int i = 0; i < extendedMatrix.RowCount-1; i++)
            {
                extendedMatrix[i, extendedMatrix.ColumnCount - 1] = extendedMatrix.Row(i).Minimum();
            }
            for (int j = 0; j < extendedMatrix.ColumnCount - 1; j++)
            {
                extendedMatrix[extendedMatrix.RowCount-1,j] = extendedMatrix.Column(j).Maximum();
            }
            Console.WriteLine(extendedMatrix);

            var minMax = extendedMatrix.Row(extendedMatrix.RowCount - 1,0,extendedMatrix.ColumnCount-1).Minimum();
            Console.WriteLine($"min max {minMax}");

            var maxMin = extendedMatrix.Column(extendedMatrix.ColumnCount - 1,0,extendedMatrix.RowCount-1).Maximum();
            Console.WriteLine($"max min {maxMin}");
            return minMax == maxMin;
        }
        public double[,] CheckForDominantColumnsAndRows(double[,] matrixOfMatrixGame)
        {
            var numericsMatrix = Matrix<double>.Build.DenseOfArray(matrixOfMatrixGame);
            for (int n = 0; n < numericsMatrix.RowCount-1; n++)
            {
                for (int i = n+1; i < numericsMatrix.RowCount; i++)
                {
                    var moreOrEqual = false;
                    for (int j = 0; j < numericsMatrix.ColumnCount; j++)
                    {
                        if (numericsMatrix[n, j] <= numericsMatrix[i, j])
                            moreOrEqual = true;
                        else
                            moreOrEqual = false;
                    }
                    if (moreOrEqual)
                    {
                        numericsMatrix = numericsMatrix.RemoveRow(n);
                        Console.WriteLine($"Removed {n} row");
                        Console.WriteLine(numericsMatrix);
                        break;
                    }
                }
            }


            for (int n = 0; n < numericsMatrix.ColumnCount - 1; n++)
            {
                for (int j = n+1; j < numericsMatrix.ColumnCount; j++)
                {
                    var lessOrEqual = false;
                    for (int i = 0; i < numericsMatrix.RowCount; i++)
                    {
                        if (numericsMatrix[i, n] >= numericsMatrix[i, j])
                            lessOrEqual = true;
                        else
                            lessOrEqual = false;
                    }
                    if (lessOrEqual)
                    {
                        numericsMatrix = numericsMatrix.RemoveColumn(n);
                        Console.WriteLine($"Removed {n} column");
                        Console.WriteLine(numericsMatrix);
                        break;
                    }
                }
            }
            return numericsMatrix.ToArray();

        }
        public double[,] GetRidOfNegativeValues(double[,] matrixOfMatrixGame)
        {
            var numericsMatrix = Matrix<double>.Build.DenseOfArray(matrixOfMatrixGame);
            var min = numericsMatrix.ToRowMajorArray().ToList().Min();
            MinValueToSubstractAtTheEnd = (-1) * min;
            Console.WriteLine("Min = {0}", MinValueToSubstractAtTheEnd);
            for (int i = 0; i < numericsMatrix.RowCount; i++)
            {
                for (int j = 0; j < numericsMatrix.ColumnCount; j++)
                {
                    numericsMatrix[i, j] += MinValueToSubstractAtTheEnd;
                }
            }
            Console.WriteLine(numericsMatrix);
            return numericsMatrix.ToArray();
        }
        public InequalitySystem MakeInequalitySystemForAPlayer(double[,] matrixOfMatrixGame)
        {
            var matrixGame = Matrix<double>.Build.DenseOfArray(matrixOfMatrixGame);
            var transposedMatrix = matrixGame.Transpose();
            var inequalitySystem = new InequalitySystem();
            for (int i = 0; i < transposedMatrix.RowCount; i++)
            {
                var row = transposedMatrix.Row(i);

                inequalitySystem.Inequalities.Add(
                        new Inequality()
                        {
                            Coefs = row.ToArray().ToList(),
                            Sign = ">=",
                            Solution = 1

                        }
                    );
            }
            foreach (var item in inequalitySystem.Inequalities)
            {
                Console.WriteLine($"{string.Concat(item.Coefs.Select(coef => string.Format("\t{0}", coef)))} {item.Sign} {item.Solution}");
            }
            var numberOfElementsInRow = transposedMatrix.ColumnCount;
            for (int i = 0; i < numberOfElementsInRow; i++)
            {
                inequalitySystem.TargetFunctionCoefs.Add(1);
            }
            Console.WriteLine($"F(x) = {string.Concat(inequalitySystem.TargetFunctionCoefs.Select(coef => string.Format("\t{0}", coef)))}");
            return inequalitySystem;
        }
        public void SetPriceOfTheGameAndGameStrategy(SimplexTableElementsContainer solutionTableElemCont)
        {
            PlayerAGameStrategy = new List<double>();
            GamePrice = Math.Round(1/solutionTableElemCont.ZiCoefs[solutionTableElemCont.ZiCoefs.Length - 1], 3);
            Console.WriteLine($"Game price = {GamePrice}");
            Console.WriteLine($"Game price - MinValueToSubstractAtTheEnd  = {GamePrice}-{MinValueToSubstractAtTheEnd} = {Math.Round(GamePrice- MinValueToSubstractAtTheEnd,3)}");
            for (int i = 0; i < solutionTableElemCont.Solutions.Length; i++)
            {
                if (solutionTableElemCont.CoefsOfBasicVariables[i] != 0) 
                {
                    PlayerAGameStrategy.Add(Math.Round(GamePrice*solutionTableElemCont.Solutions[i],3));
                }
                else 
                {
                    PlayerAGameStrategy.Add(0);
                }
            }
            Console.WriteLine($"Optimal game strategy of A player ({string.Concat(PlayerAGameStrategy.Select(coef => string.Format("\t{0}", coef)))})");
        }

        public void InitGame(double[,] matrixOfMatrixGame) 
        {
            var numericsMatrix = Matrix<double>.Build.DenseOfArray(matrixOfMatrixGame);
            ADecisionCount = numericsMatrix.RowCount;
            BDecisionCount = numericsMatrix.ColumnCount;

            if (!CheckForSaddlePoint(matrixOfMatrixGame)) 
            {
                matrixOfMatrixGame = CheckForDominantColumnsAndRows(matrixOfMatrixGame);
                if (Matrix<double>.Build.DenseOfArray(matrixOfMatrixGame).ToRowMajorArray().ToList().Where(el => el < 0).Count() > 0) 
                {
                    matrixOfMatrixGame = GetRidOfNegativeValues(matrixOfMatrixGame);
                }
                if (Initializer!=null) 
                {
                    var inequalitySystemForAPlayer = MakeInequalitySystemForAPlayer(matrixOfMatrixGame);

                    var simplexElemCont = Initializer.ToCanonicalForm(inequalitySystemForAPlayer);
                    var solutionTableElemCont = Initializer.SolveSimplexMin(simplexElemCont);
                    SetPriceOfTheGameAndGameStrategy(solutionTableElemCont);


                }
                
            }

        }
        public void InitGameFromCode() 
        {
            var matrixCoefs = new double[,]
            {
                 {3, -3, 2},
                 {7,  4, 1},
                 {-6, 2, 6}

            };
            InitGame(matrixCoefs);
        }

        public void InitGameFromConsole(IReader _reader) 
        {
            Console.WriteLine("Enter data of matrix game");
            (var rows, var cols) = _reader.ReadRowsAndCols();

            var matrix2d = _reader.ReadMatrix(rows, cols);
            var printer = new MatrixConsolePrinter();
            printer.Print(matrix2d);
            InitGame(matrix2d);
        }


    }

}
