using MathNet.Numerics;
using System;


namespace AnalysisMatrix
{
        public class Matrix
    {
        public int Rows{get;private set;}
        public int Columns{get;private set;}
        public double[,] Arr{get;set;}
        public double this[int row,int col]
        {
            get=>Arr[row,col];
            set=>Arr[row,col] = value;
        }

        public double[] OwnVector { get; set; }
        public double LambdaMax { get; set; }
        public double ConsistencyIndex { get; set; }
        public double ConsistencyRatio { get; set; }

        public Matrix()
        {

        }
        public Matrix(int rows,int columns)
        {
            Rows = rows;
            Columns = columns;
            Arr = new double[rows,columns];
        }
        public void Print()
        {
            for (var i = 0; i < Rows; i++)
            {
                Console.Write("\n");
                for (var j = 0; j < Columns; j++)
                {
                    Console.Write("{0}\t", Arr[i, j]);
                }
                
            } 

        }
        public void InitFromConosle()
        {
            Console.WriteLine("enter number of rows");
            var value = Console.ReadLine();
            int.TryParse(value,out int rows);
            Console.WriteLine("enter number of columns");
            value = Console.ReadLine();
            int.TryParse(value,out int columns);
            Rows = rows;
            Columns = columns;
            Arr = new double[rows,columns];
            
            for(int i = 0;i<rows;i++)
            {
                Console.WriteLine($"Entering {i} row");
                for(int j=0;j<columns;j++)
                {
                    Console.WriteLine($"Enter double value and press enter");
                    double.TryParse(Console.ReadLine(),out double elem);
                    Arr[i,j] = elem;
                }
            }
        }
        public void InitializeMatrix(double[,] arr)
        {
            var rows = arr.GetLength(0);
            var columns = arr.GetLength(1);
            if(rows>Rows||columns>Columns)
            {
                Console.WriteLine("Dimensions are not correct");
                return;
            }
            for(int i = 0;i<rows;i++)
            {
                for(int j = 0;j<columns;j++)
                {
                    Arr[i,j] = arr[i,j];
                }
            }
        }
        public double[] GetOwnVector()
        {
            var ownVector = new double[Rows];
            var sum = 0.0;
            for(int i = 0;i< Rows; i++)
            {
                for(int j=0;j<Columns;j++)
                {
                    for(int n = 0; n < Rows; n++) 
                    {
                        sum += Arr[n, j];//column sum
                    }
                    ownVector[i] += Arr[i, j] / sum;
                    sum = 0.0;
                }
            }
            OwnVector = NormalizeVector(ownVector);
            return OwnVector;
        }
        public double[] NormalizeVector(double[] vector)
        {
            for (int i = 0; i < Rows; i++)
            {
                vector[i] /= Rows;
            }
            return vector;
        }

        public double[] GetOwnVectorByMultiplication() 
        {
            var ownVector = new double[Rows];
            var mult = 1.0;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    mult *= Arr[i, j];
                }
                ownVector[i] = Math.Pow(mult, (double)1 / Rows);
                mult = 1.0;
            }

            //weights
            var sum = 0.0;
            foreach (var el in ownVector)
                sum += el;

            for (int i = 0; i < ownVector.Length; i++)
                ownVector[i] /= sum;

            OwnVector = ownVector;
            return OwnVector;

        }


        /// <summary>
        /// Gets labmda max that is sum of all elements of matrix 
        /// that is multiplied by own vector divided by own vector element by element
        /// </summary>
        /// <param matrix multiplied by own vector="Y"></param>
        /// <param matrix own vector="vect"></param>
        /// <returns></returns>
        public double GetMaxValueAsAverage(double[] Y,double[] vect)
        {
            var sumDiv = 0.0;
            for(int i = 0;i<Y.Length;i++)
            {
                sumDiv+=Y[i]/vect[i];
            }

            LambdaMax =  sumDiv/Rows;
            return LambdaMax;
                
        }
        public double GetMaxValueAsAverage(double[] vect) 
        {
            var sum = 0.0;
            for (int i = 0; i < vect.Length; i++)
                sum += vect[i];
            LambdaMax = sum / vect.Length;
            return LambdaMax;
        }
        public double GetConsistencyIndex(double lambdaMax)
        {
            ConsistencyIndex = (lambdaMax-Rows)/(Rows-1);
            //if (ConsistencyIndex > 0.1) 
            //{
            //    ConsistencyIndex-= ConsistencyIndex/2;
            //}
            return ConsistencyIndex;
        }
        public double GetConsistencyRatio(double consistencyIndex) 
        {
            var cr = new ConsistentyRatio();
            var value = cr[Rows - 1];
            if (value != 0)
            {
                ConsistencyRatio = consistencyIndex / value;
            }
            else 
            {
                ConsistencyRatio = 0;
            }
            return ConsistencyRatio;
        }


        public static bool Check(Matrix matrix1,Matrix matrix2)
        {
            if(matrix1.Columns!=matrix2.Columns || matrix1.Rows!=matrix2.Rows)
            {
                Console.WriteLine("Cannot add matrices with different dimensions");
                return false;
            }
            return true;
        }
        public static Matrix operator +(Matrix matrix1,Matrix matrix2)
        {
            if(Check(matrix1,matrix2))
            {
                for(int i = 0;i<matrix1.Rows;i++)
                {
                    for(int j = 0;j<matrix1.Columns;j++)
                    {
                        matrix1[i,j] +=matrix2[i,j];
                    }
                }
                return matrix1;
            }
            return null;

        }
        public static Matrix operator -(Matrix matrix1,Matrix matrix2)
        {
            if(Check(matrix1,matrix2))
            {
                for(int i = 0;i<matrix1.Rows;i++)
                {
                    for(int j = 0;j<matrix1.Columns;j++)
                    {
                        matrix1[i,j] -=matrix2[i,j];
                    }
                }
                return matrix1;
            }
            return null;

        }
        //TODO:increase perfomance
        public static Matrix operator *(Matrix matrix1,Matrix matrix2)
        {
            var resMatrix = new Matrix(0,0);
            if(matrix1!=null||matrix2!=null)
            {
                if(matrix1.Columns==matrix2.Rows)
                {
                    if(matrix1.Columns>=matrix2.Columns
                    &&matrix1.Rows>=matrix2.Rows)
                    {
                        resMatrix = new Matrix(matrix1.Columns,matrix1.Rows);
                        LoopMultiplication(matrix1.Rows, matrix2.Columns, ref resMatrix, matrix1, matrix2);
                    }else
                    {
                        if(matrix1.Rows<matrix2.Rows)
                        {
                            resMatrix = new Matrix(matrix1.Rows,matrix2.Columns);
                            LoopMultiplication(matrix1.Rows, matrix2.Columns, ref resMatrix, matrix1, matrix2);
                        }
                            
                        else
                        {
                            resMatrix = new Matrix(matrix2.Columns,matrix2.Rows);
                            LoopMultiplication(matrix1.Rows, matrix1.Columns, ref resMatrix, matrix1, matrix2);
                        }
                            
                    }
                    
                    return resMatrix;
                }else
                {
                    Console.WriteLine("Unable to multiply matrix");
                }
            }
            return null;
        }
        private static void LoopMultiplication(int rows,int cols,ref Matrix resMatrix,Matrix matrix1,Matrix matrix2) 
        {
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    for (int k = 0; k < matrix1.Columns; k++)
                        resMatrix[row, col] += matrix1[row, k] * matrix2[k, col];
                }
            }
        }
    }
}