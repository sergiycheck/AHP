using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace methodAnalysisHierarchies
{
    public class SimplexMethod
    {
        private IMatrixPrinter _matrixPrinter;
        public SimplexMethod(IMatrixPrinter printer) 
        {
            _matrixPrinter = printer;
        }

        public double[] GetZiCoefs(int rows,int columns,double[] ZiCoefs,double[] coefOfBasicVariables,double[,] matrixCoefs) 
        {
            ZiCoefs = new double[columns];
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    ZiCoefs[i] = Math.Round(ZiCoefs[i]+coefOfBasicVariables[j] * matrixCoefs[j, i],3);
                }
            }
            return ZiCoefs;
        }
        public double[] GetCiMinusZiCoefs(int columns, double[] CiMinusZiCoefs, double[] maximizationFunctionCoefs, double[] ZiCoefs)
        {
            for (int i = 0; i < columns; i++)
            {
                CiMinusZiCoefs[i] = Math.Round(maximizationFunctionCoefs[i] - ZiCoefs[i],3);
            }
            return CiMinusZiCoefs;
        }
        public bool EvaluateIfObtimal(double [] CiMinusZiCoefs, Func<double, int, bool> checkOptimality) 
        {
            return CiMinusZiCoefs.All(elem => checkOptimality(elem, 0));
        }
        public double[] GetRatioCoefs(int rows,double[] ratioCoefs,double[]solutionCoefs,double[,]matrixCoefs,int keyColumnIndex) 
        {
            for (int i = 0; i < rows; i++)
            {
                if (matrixCoefs[i, keyColumnIndex] != 0)
                    ratioCoefs[i] = solutionCoefs[i] / matrixCoefs[i, keyColumnIndex];
                else
                    ratioCoefs[i] = double.MaxValue;
            }
            return ratioCoefs;
        }
        public double[,] AppendColumnToTheEnd(double[,] matrixCoefs, double[] solutionCoefs)
        {
            var columns = matrixCoefs.GetLength(1)+1;
            var rows = matrixCoefs.GetLength(0);
            var newMatrixCoefs = new double[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (j == columns-1) 
                    {
                        newMatrixCoefs[i, j] = solutionCoefs[i];
                    }
                    else 
                    {
                        newMatrixCoefs[i, j] = matrixCoefs[i, j];
                    }
                    
                }
            }
            return newMatrixCoefs;
        }
        public double[,] AppendRowToTheEnd(double[,] matrixCoefs, double[] maximizationFunctionCoefs)
        {
            var columns = matrixCoefs.GetLength(1);
            var rows = matrixCoefs.GetLength(0) + 1;
            var newMatrixCoefs = new double[rows, columns];
            var newMaximizationFunctionsCoefsList = maximizationFunctionCoefs.ToList();
            newMaximizationFunctionsCoefsList.Add(0);
            var newMaximizationFunctionsCoefs = newMaximizationFunctionsCoefsList.ToArray();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (i == rows - 1)
                    {
                        newMatrixCoefs[i, j] = newMaximizationFunctionsCoefs[j];
                    }
                    else
                    {
                        newMatrixCoefs[i, j] = matrixCoefs[i, j];
                    }

                }
            }
            return newMatrixCoefs;
        }
       

        public double[,] Solve(double[,] matrixCoefs,double[] solutionCoefs,double[] maximizationFunctionCoefs,Func<double,int,bool> checkOptimality,bool maximization) 
        {
            var columns = matrixCoefs.GetLength(1);
            var rows = matrixCoefs.GetLength(0);

            var coefOfBasicVariables = new double[rows];
            var ZiCoefs = new double[columns];
            ZiCoefs = GetZiCoefs(rows, columns, ZiCoefs, coefOfBasicVariables, matrixCoefs);
            var CiMinusZiCoefs = new double[columns];
            CiMinusZiCoefs = GetCiMinusZiCoefs(columns, CiMinusZiCoefs, maximizationFunctionCoefs,ZiCoefs);

            //var fullSimplexTable = Matrix<double>.Build.DenseOfArray(matrixCoefs);
            var rowsWithCiCoefs = rows + 1;
            var columnsWithSolutionCoefs = columns + 1;
            var extendedMatrix = new double[rowsWithCiCoefs, columnsWithSolutionCoefs];
            var counter = 0;
            while (!EvaluateIfObtimal(CiMinusZiCoefs,checkOptimality))//if check for optimal returns false start loop
            {
                counter++;
                if (counter >= 10)
                    break;
                //extendedMatrix = new double[rowsWithCiCoefs, columnsWithSolutionCoefs];
                for (int i = 0; i < rowsWithCiCoefs; i++)
                {
                    for (int j = 0; j < columnsWithSolutionCoefs; j++)
                    {
                        if(i< rowsWithCiCoefs - 1 && j< columnsWithSolutionCoefs - 1)
                            extendedMatrix[i, j] = matrixCoefs[i, j];
                    }
                }

                _matrixPrinter.Print(extendedMatrix);

                var max = 0.0;
                if(maximization)
                    max = CiMinusZiCoefs.Max();
                else
                    max = CiMinusZiCoefs.Min();

                var keyColumnIndex = CiMinusZiCoefs.ToList().IndexOf(max);
                var ratioCoefs = new double[rows];
                ratioCoefs = GetRatioCoefs(rows, ratioCoefs, solutionCoefs, matrixCoefs, keyColumnIndex);//change matrix coefs with new table
                _matrixPrinter.Print(ratioCoefs);

                var min = ratioCoefs.ToList().Where(el=>el>0).Min();
                var keyRowIndex = ratioCoefs.ToList().IndexOf(min);
                var keyElement = matrixCoefs[keyRowIndex, keyColumnIndex];
                Console.WriteLine("Key element {0} with index [row,column] = [{1},{2}]", keyElement,keyRowIndex,keyColumnIndex);

                //fullSimplexTable= fullSimplexTable.InsertColumn(columnsWithSolutionCoefs,solutionCoefs); //= Matrix<double>.Build.DenseOfArray(AppendColumnToTheEnd(matrixCoefs, solutionCoefs));
                //fullSimplexTable = fullSimplexTable.InsertRow(rowsWithCiCoefs,maximizationFunctionCoefs); //= Matrix<double>.Build.DenseOfArray(AppendRowToTheEnd(fullSimplexTable.ToArray(), maximizationFunctionCoefs));//increasing sizes here
                extendedMatrix = InsertNumsToColumn(extendedMatrix,columnsWithSolutionCoefs, solutionCoefs);
                extendedMatrix = InsertNumsToRow(extendedMatrix,rowsWithCiCoefs, maximizationFunctionCoefs);

                _matrixPrinter.Print(extendedMatrix);

                //change basics variables
                coefOfBasicVariables[keyRowIndex] = maximizationFunctionCoefs[keyColumnIndex];


                var oldSymplexTable = Matrix<double>.Build.DenseOfArray(extendedMatrix).ToArray();
                for (int i = 0; i < rowsWithCiCoefs; i++) 
                {
                    if (i == keyRowIndex)
                    {
                        for (int j = 0; j < columnsWithSolutionCoefs; j++)
                        {
                            Console.WriteLine($"{oldSymplexTable[keyRowIndex, j]} /{keyElement}");
                            extendedMatrix[keyRowIndex, j] =
                                Math.Round(
                                    extendedMatrix[keyRowIndex, j]/keyElement,3);
                        }
                    }
                    else 
                    {
                        for (int j = 0; j < columnsWithSolutionCoefs; j++)
                        {
                            Console.WriteLine($"{oldSymplexTable[i, j]}-({oldSymplexTable[i, keyColumnIndex]}*{oldSymplexTable[keyRowIndex, j]})/{keyElement}");
                            extendedMatrix[i, j] =Math.Round(
                                oldSymplexTable[i,j]-
                                (oldSymplexTable[i, keyColumnIndex] *
                                oldSymplexTable[keyRowIndex, j]) /
                                keyElement,3);
                        }
                    }

                    Console.WriteLine("-------------------");
                    
                }
                _matrixPrinter.Print(extendedMatrix);
                ZiCoefs = GetZiCoefs(rows, columnsWithSolutionCoefs, ZiCoefs, coefOfBasicVariables, extendedMatrix);
                _matrixPrinter.Print(ZiCoefs);
                CiMinusZiCoefs = GetCiMinusZiCoefs(columns, CiMinusZiCoefs, maximizationFunctionCoefs, ZiCoefs);
                _matrixPrinter.Print(CiMinusZiCoefs);
                Console.WriteLine("Coefficients of basic variables");
                _matrixPrinter.Print(coefOfBasicVariables);

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        matrixCoefs[i, j] = extendedMatrix[i, j];
                    }
                }
                for (int i = 0; i < rows; i++)//get solution coefs 
                { 
                    solutionCoefs[i] = extendedMatrix[i, columnsWithSolutionCoefs - 1];
                }


            }

            return extendedMatrix;
        }

        public double[,] InsertNumsToRow(double[,] extendedMatrix, int rowsWithCiCoefs, double[] maximizationFunctionCoefs)
        {
            if (maximizationFunctionCoefs.Length <= extendedMatrix.GetLength(1))
            {
                for (int j = 0; j < maximizationFunctionCoefs.Length; j++)
                {
                    extendedMatrix[rowsWithCiCoefs-1, j] = maximizationFunctionCoefs[j];
                }
            }
            return extendedMatrix;
        }

        public double[,] InsertNumsToColumn(double[,] extendedMatrix, int columnsWithSolutionCoefs, double[] solutionCoefs)
        {
            if(solutionCoefs.Length<= extendedMatrix.GetLength(0)) 
            {
                for (int i = 0; i < solutionCoefs.Length; i++)
                {
                    extendedMatrix[i, columnsWithSolutionCoefs - 1] = solutionCoefs[i];      
                }
            }
            return extendedMatrix;
        }
    }
}
