using System;
using System.Collections.Generic;
using System.Text;

namespace methodAnalysisHierarchies
{
    public interface IMatrixPrinter 
    {
        public void Print(double[,] matrix);
        public void Print(double[] vector);
    }
    public class MatrixConsolePrinter:IMatrixPrinter
    {
        public void Print(double[,] matrix)
        {
            Console.WriteLine($"Matrix [rows,columns] [{matrix.GetLength(0)},{matrix.GetLength(1)}]");
            for (var i = 0; i < matrix.GetLength(0); i++)
            {
                Console.Write("\n");
                for (var j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write("{0}\t", Math.Round(matrix[i, j],3));
                }

            }
            Console.Write("\n");

        }
        public void Print(double[] vector)
        {
            Console.WriteLine($"vector [columns] [{vector.Length}]");
            for (var i = 0; i < vector.Length; i++)
            {
                Console.Write("{0}\t", Math.Round(vector[i],3));
            }
            Console.Write("\n");
        }
    }
}
