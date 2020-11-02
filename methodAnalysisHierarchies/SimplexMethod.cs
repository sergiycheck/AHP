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
        public double[] GetRatioCoefs(int rows,double[] ratioCoefs,double[]solutionCoefs,double[,]matrixCoefs,int keyColumnOrRowIndex,bool column) 
        {
            for (int i = 0; i < rows; i++)
            {
                if (column) 
                {
                    if (matrixCoefs[i, keyColumnOrRowIndex] != 0)
                        ratioCoefs[i] = solutionCoefs[i] / matrixCoefs[i, keyColumnOrRowIndex];                  
                    else
                        ratioCoefs[i] = double.MaxValue;                   
                }
                else 
                {
                    if (matrixCoefs[keyColumnOrRowIndex,i] != 0)
                        ratioCoefs[i] = (-1) * solutionCoefs[i] / matrixCoefs[keyColumnOrRowIndex, i];//-1 was here and it worked somehow 
                    else
                        ratioCoefs[i] = double.MaxValue;                
                }
                    
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
       

        public void ExtendMatrix(ref double [,] matrixToExtend,double[,] oldMatrix, int newRows, int newCols) 
        {
            for (int i = 0; i < newRows; i++)
            {
                for (int j = 0; j < newCols; j++)
                {
                    if (i < newRows - 1 && j < newCols - 1)
                        matrixToExtend[i, j] = oldMatrix[i, j];
                }
            }
        }
        public void GaussinEleminationStep(ref double[,] extendedMatrix, int rowsWithCiCoefs, int columnsWithSolutionCoefs, int keyRowIndex, int keyColumnIndex, double keyElement) 
        {
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
                                extendedMatrix[keyRowIndex, j] / keyElement, 3);
                    }
                }
                else
                {
                    for (int j = 0; j < columnsWithSolutionCoefs; j++)
                    {
                        Console.WriteLine($"{oldSymplexTable[i, j]}-({oldSymplexTable[i, keyColumnIndex]}*{oldSymplexTable[keyRowIndex, j]})/{keyElement}");
                        extendedMatrix[i, j] = Math.Round(
                            oldSymplexTable[i, j] -
                            (oldSymplexTable[i, keyColumnIndex] *
                            oldSymplexTable[keyRowIndex, j]) /
                            keyElement, 3);
                    }
                }

                Console.WriteLine("-------------------");

            }
        }
        public void CopyElements(ref double[,] matrixToCopy, double [,] matrixFromCopy,int rows,int cols) 
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrixToCopy[i, j] = matrixFromCopy[i, j];
                }
            }
        }
        public void CopyColumnFromMatrix(ref double[] solutionCoefs, double[,] extendedMatrix, int rows, int columnsWithSolutionCoefs)
        {
            for (int i = 0; i < rows; i++)//get solution coefs 
            {
                solutionCoefs[i] = extendedMatrix[i, columnsWithSolutionCoefs - 1];
            }
        }
        public void CopyRowFromMatrix(ref double[] maximizationFuncCoefs, double[,] extendedMatrix, int columns, int maximizationFuncRow)
        {
            for (int i = 0; i < columns; i++)//get solution coefs 
            {
                maximizationFuncCoefs[i] = extendedMatrix[maximizationFuncRow-1,i];
            }
        }

        public SimplexTableElementsContainer Solve(double[,] matrixCoefs,double[] solutionCoefs,double[] maximizationFunctionCoefs,Func<double,int,bool> checkOptimality,bool maximization) 
        {
            
            var columns = matrixCoefs.GetLength(1);
            var rows = matrixCoefs.GetLength(0);


            var coefOfBasicVariables = new double[rows];
            var ZiCoefs = new double[columns];
            ZiCoefs = GetZiCoefs(rows, columns, ZiCoefs, coefOfBasicVariables, matrixCoefs);
            var CiMinusZiCoefs = new double[columns];
            CiMinusZiCoefs = GetCiMinusZiCoefs(columns, CiMinusZiCoefs, maximizationFunctionCoefs,ZiCoefs);

            var rowsWithCiCoefs = rows + 1;
            var columnsWithSolutionCoefs = columns + 1;
            var extendedMatrix = new double[rowsWithCiCoefs, columnsWithSolutionCoefs];
            var counter = 0;
            var oldMaximizationFuncCoefs = maximizationFunctionCoefs;

            var extendedMaxFuncCoefs = new double[columnsWithSolutionCoefs];
            for (int i = 0; i < maximizationFunctionCoefs.Length; i++)
            {
                extendedMaxFuncCoefs[i] = maximizationFunctionCoefs[i];// (-1)* maximization function coefs with negative sign first time ((/*(-1)*/) not working with negative multiplication)
            }

            ExtendMatrix(ref extendedMatrix, matrixCoefs, rowsWithCiCoefs, columnsWithSolutionCoefs);
            _matrixPrinter.Print(extendedMatrix);
            extendedMatrix = InsertNumsToColumn(extendedMatrix,columnsWithSolutionCoefs, solutionCoefs);
            extendedMatrix = InsertNumsToRow(extendedMatrix,rowsWithCiCoefs, maximizationFunctionCoefs);
            _matrixPrinter.Print(extendedMatrix);
            var matrixExtended = Matrix<double>.Build.DenseOfArray(extendedMatrix);
            while (!EvaluateIfObtimal(CiMinusZiCoefs,checkOptimality)||!matrixExtended.Column(columnsWithSolutionCoefs-1).All(el => el >= 0))//if check for optimal returns false then start loop
            {
                counter++;
                if (counter >= 10)
                    break;

                // find key element with its row and column
                var max = 0.0;
                var keyColumnIndex = 0;
                var keyRowIndex = 0;
                if (solutionCoefs.All(el => el >= 0)) 
                {
                    max = (maximization) ? CiMinusZiCoefs.Max() : CiMinusZiCoefs.Min();
                    keyColumnIndex = CiMinusZiCoefs.ToList().IndexOf(max);
                    var ratioCoefsColumn = new double[rows];
                    ratioCoefsColumn = GetRatioCoefs(rows, ratioCoefsColumn, solutionCoefs, matrixCoefs, keyColumnIndex,true);
                    _matrixPrinter.Print(ratioCoefsColumn);
                    var pisitiveMin = ratioCoefsColumn.ToList().Where(el => el > 0).Min();
                    keyRowIndex = ratioCoefsColumn.ToList().IndexOf(pisitiveMin);
                }
                else 
                {
                    max = (double)solutionCoefs.Min();
                    keyRowIndex = solutionCoefs.ToList().IndexOf(max);
                    var ratioCoefsRow = new double[columns];
                    ratioCoefsRow = GetRatioCoefs(columns, ratioCoefsRow, extendedMaxFuncCoefs, matrixCoefs, keyRowIndex,false);
                    _matrixPrinter.Print(ratioCoefsRow);
                    var pisitiveMin = ratioCoefsRow.ToList().Where(el => el > 0).Min();
                    keyColumnIndex = ratioCoefsRow.ToList().IndexOf(pisitiveMin);
                }

                var keyElement = matrixCoefs[keyRowIndex, keyColumnIndex];
                Console.WriteLine("Key element {0} with index [row,column] = [{1},{2}]", keyElement,keyRowIndex,keyColumnIndex);

                
                //change basics variables
                coefOfBasicVariables[keyRowIndex] = oldMaximizationFuncCoefs[keyColumnIndex];

                //Gaussian elimination
                GaussinEleminationStep( ref extendedMatrix, rowsWithCiCoefs, columnsWithSolutionCoefs, keyRowIndex, keyColumnIndex, keyElement);
                //we have to get new maximization function coefs

                CopyRowFromMatrix(ref extendedMaxFuncCoefs, extendedMatrix, columnsWithSolutionCoefs, rowsWithCiCoefs);

                _matrixPrinter.Print(extendedMatrix);
                ZiCoefs = GetZiCoefs(rows, columnsWithSolutionCoefs, ZiCoefs, coefOfBasicVariables, extendedMatrix);
                _matrixPrinter.Print(ZiCoefs);

                CiMinusZiCoefs = GetCiMinusZiCoefs(columns, CiMinusZiCoefs, oldMaximizationFuncCoefs, ZiCoefs);
                _matrixPrinter.Print(CiMinusZiCoefs);
                Console.WriteLine("Coefficients of basic variables");
                _matrixPrinter.Print(coefOfBasicVariables);
                CopyElements(ref matrixCoefs, extendedMatrix, rows, columns);
                CopyColumnFromMatrix(ref solutionCoefs, extendedMatrix, rows, columnsWithSolutionCoefs);
                matrixExtended = Matrix<double>.Build.DenseOfArray(extendedMatrix);
            }
            var simplexContainer = new SimplexTableElementsContainer();
            simplexContainer.ExtendedMatrix = extendedMatrix;
            simplexContainer.ZiCoefs = ZiCoefs;
            simplexContainer.CiMinusZiCoefs = CiMinusZiCoefs;
            simplexContainer.CoefsOfBasicVariables = coefOfBasicVariables;
            simplexContainer.MatrixCoefs = matrixCoefs;
            simplexContainer.Solutions = solutionCoefs;
            
            return simplexContainer;
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
