using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace methodAnalysisHierarchies
{
    public interface IReader 
    {
        public (int, int) ReadRowsAndCols();
        public double[,] ReadMatrix(int rows,int columns);
        public List<double> ReadRow(int countOfNumbers);
    }
    public class Reader: IReader
    {
        public (int,int) ReadRowsAndCols() 
        {
            Console.WriteLine("enter number of rows");
            var valueRows = Console.ReadLine();
            int.TryParse(valueRows, out int rows);
            Console.WriteLine("enter number of columns");
            var valueCols = Console.ReadLine();
            int.TryParse(valueCols, out int columns);
            return (rows, columns);
        }
        public double[,] ReadMatrix(int rows,int columns) 
        {
            var matrix = new double[rows, columns];
            for (int rownum=0; rownum < rows; rownum++)
            {
                Console.WriteLine($"enter {rownum} row of matrix");
                var rowValues= ReadRow(columns);

                for (int colnum = 0; colnum < columns; colnum++)
                {
                    matrix[rownum, colnum] = rowValues[colnum];
                }
            }
            return matrix;

        }
        public List<double> ReadRow(int countOfNumbers)
        {
            var text = Console.ReadLine();
            if (text != null || text != "")
            {
                var matches = AhpBuilder.GetMatches(AhpBuilder.negNumMatcher, text);
                if (matches.Count != countOfNumbers)
                {
                    Console.Write("Values not correct"); return new List<double>();
                }
                return AhpBuilder.ConvertMatchesToDoubles(matches);
            }
            else
            {
                Console.WriteLine("Exit program and enter again");
            }
            return new List<double>();

        }
    }
}
