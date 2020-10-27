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
            //var solutionTable = Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, true);


            //var matrixCoefs = new double[,]
            //{
            //    {    1,  0,  1, 1, 0, 0 },
            //    {    2,  1,  2, 0, 1, 0 },
            //    {    2, -1,  2, 0, 0, 1 }
            //};
            //var solutionCoefs = new double[] { 4, 6, 2 };
            //var maximizationFunctionCoefs = new double[] { 3, -2, 1, 0, 0, 0 };
            //Func<double, int, bool> greaterThanOrEqual = (first, second) => first >= second;
            //var solutionTable = Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, false);


            //var matrixCoefs = new double[,]
            //{
            //    {    3, -1,  2, 1, 0, 0 },
            //    {   -2, -4,  0, 0, 1, 0 },
            //    {   -4,  3,  8, 0, 0, 1 }
            //};
            //var solutionCoefs = new double[] { 7, 12,10};
            //var maximizationFunctionCoefs = new double[] { 2, -3, 6, 0, 0, 0 };
            //Func<double, int, bool> greaterThanOrEqual = (first, second) => first >= second;
            //var solutionTable = Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, false);



            var matrixCoefs = new double[,]
            {
                {-2,  -1,   1,   0,    0,   0,   0 },
                {-1,  -4,   0,   1,    0,   0,   0,},
                {2,   -1,   0,   0,    1,   0,   0,},
                {2,    3,   0,   0,    0,   1,   0, },
                {-1,   4,   0,   0,    0,   0,   1, }
            };
            var solutionCoefs = new double[] { -5, -6, 16, 32, 28 };
            var maximizationFunctionCoefs = new double[] { 1, -6, 0, 0, 0, 0, 0 };
            Func<double, int, bool> greaterThanOrEqual = (first, second) => first <= second;
            var solutionTable = Solve(matrixCoefs, solutionCoefs, maximizationFunctionCoefs, greaterThanOrEqual, true);
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
                ratioCoefs[i] = solutionCoefs[i] / matrixCoefs[i,keyColumnIndex];
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
        public void Print(double [,] matrix)
        {
            Console.WriteLine($"Matrix [rows,columns] [{matrix.GetLength(0)},{matrix.GetLength(1)}]");
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                Console.Write("\n");
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("{0}\t", matrix[i, j]);
                }

            }
            Console.Write("\n");

        }
        public void Print(double[] vector)
        {
            Console.WriteLine($"vector [columns] [{vector.Length}]");
            for (var i = 0; i < vector.Length; i++)
            {
                Console.Write("{0}\t", vector[i]);
            }
            Console.Write("\n");
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
            while (!EvaluateIfObtimal(CiMinusZiCoefs,checkOptimality))//if check for optimal returns false start loop
            {

                //extendedMatrix = new double[rowsWithCiCoefs, columnsWithSolutionCoefs];
                for (int i = 0; i < rowsWithCiCoefs; i++)
                {
                    for (int j = 0; j < columnsWithSolutionCoefs; j++)
                    {
                        if(i< rowsWithCiCoefs - 1 && j< columnsWithSolutionCoefs - 1)
                            extendedMatrix[i, j] = matrixCoefs[i, j];
                    }
                }

                Print(extendedMatrix);

                var max = 0.0;
                if(maximization)
                    max = CiMinusZiCoefs.Max();
                else
                    max = CiMinusZiCoefs.Min();

                var keyColumnIndex = CiMinusZiCoefs.ToList().IndexOf(max);
                var ratioCoefs = new double[rows];
                ratioCoefs = GetRatioCoefs(rows, ratioCoefs, solutionCoefs, matrixCoefs, keyColumnIndex);//change matrix coefs with new table
                Print(ratioCoefs);

                var min = ratioCoefs.ToList().Where(el=>el>0).Min();
                var keyRowIndex = ratioCoefs.ToList().IndexOf(min);
                var keyElement = matrixCoefs[keyRowIndex, keyColumnIndex];
                Console.WriteLine("Key element {0} with index [row,column] = [{1},{2}]", keyElement,keyRowIndex,keyColumnIndex);

                //fullSimplexTable= fullSimplexTable.InsertColumn(columnsWithSolutionCoefs,solutionCoefs); //= Matrix<double>.Build.DenseOfArray(AppendColumnToTheEnd(matrixCoefs, solutionCoefs));
                //fullSimplexTable = fullSimplexTable.InsertRow(rowsWithCiCoefs,maximizationFunctionCoefs); //= Matrix<double>.Build.DenseOfArray(AppendRowToTheEnd(fullSimplexTable.ToArray(), maximizationFunctionCoefs));//increasing sizes here
                extendedMatrix = InsertNumsToColumn(extendedMatrix,columnsWithSolutionCoefs, solutionCoefs);
                extendedMatrix = InsertNumsToRow(extendedMatrix,rowsWithCiCoefs, maximizationFunctionCoefs);

                Print(extendedMatrix);

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
                Print(extendedMatrix);
                ZiCoefs = GetZiCoefs(rows, columnsWithSolutionCoefs, ZiCoefs, coefOfBasicVariables, extendedMatrix);
                Print(ZiCoefs);
                CiMinusZiCoefs = GetCiMinusZiCoefs(columns, CiMinusZiCoefs, maximizationFunctionCoefs, ZiCoefs);
                Print(CiMinusZiCoefs);
                Console.WriteLine("Coefficient of basic variables");
                Print(coefOfBasicVariables);

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
