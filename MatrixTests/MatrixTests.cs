using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace MatrixTests
{
    public class MatrixTests
    {
        [SetUp]
        public void Setup()
        {
        }
        public CultureInfo FormatProvider {
            get {
                var formatProvider = (CultureInfo)CultureInfo.InvariantCulture.Clone();
                formatProvider.TextInfo.ListSeparator = " ";
                return formatProvider;
            } }
        
        public Matrix<double> GetMatrix(int rows,int cols) 
        {
            var matrix = Matrix<double>.Build.Dense(rows, cols);
            var k = 0.0;
            for (var i = 0; i < matrix.RowCount; i++)
            {
                for (var j = 0; j < matrix.ColumnCount; j++)
                {
                    matrix[i, j] = k++;
                }
            }
            return matrix;
        }
        [Test]
        public void MatrixCreation2() 
        {
            var matrix = GetMatrix(3,5);
            Console.WriteLine(matrix);
        } 
        [Test]
        public void MatrixCreation()
        {
            // Create square matrix
            var matrix = new DenseMatrix(5);
            var k = 0;
            for (var i = 0; i < matrix.RowCount; i++)
            {
                for (var j = 0; j < matrix.ColumnCount; j++)
                {
                    matrix[i, j] = k++;
                }
            }
            Console.WriteLine(matrix);
        }
        [Test]
        public void InsertVector() 
        {
            var matrix = GetMatrix(3, 5);
            Console.WriteLine(matrix);
            // Create vector
            var vector = new DenseVector(new[] { 50.0, 51.0, 52.0, 53.0, 54.0 });
            Console.WriteLine(@"Sample vector");
            Console.WriteLine(vector.ToString("#0.00\t", FormatProvider));
            Console.WriteLine();

            // 1. Insert new column
            var result = matrix.InsertRow(3, vector);
            Console.WriteLine(@"1. Insert new column");
            Console.WriteLine(result.ToString("#0.00\t", FormatProvider));
            Console.WriteLine();
        }
        [Test]
        public void IncreaseDimensions()
        {
            var matrix = GetMatrix(3, 5);
            Console.WriteLine(matrix.ToString("#0.00\t", FormatProvider));

            var newMatrix = new DenseMatrix(matrix.RowCount+1, matrix.ColumnCount+1);
            newMatrix.SetSubMatrix(0, 0, matrix);
            Console.WriteLine(newMatrix.ToString("#0.00\t", FormatProvider));

            var vectorCol = new DenseVector(new[] { 50.0, 51.0, 52.0 });
            newMatrix.SetColumn(newMatrix.ColumnCount - 1, 0, vectorCol.Count, vectorCol);
            Console.WriteLine(newMatrix.ToString("#0.00\t", FormatProvider));

            var vectorRow = new DenseVector(new[] { 60.0, 61.0, 62.0 });
            newMatrix.SetRow(newMatrix.RowCount - 1, 0, vectorRow.Count, vectorRow);
            Console.WriteLine(newMatrix.ToString("#0.00\t", FormatProvider));

            var vectorlastRow = newMatrix.Row(newMatrix.RowCount - 1);
            Console.WriteLine(vectorlastRow.ToString("#0.00\t", FormatProvider));
            var vectorlastColumn = newMatrix.Column(newMatrix.ColumnCount - 1);
            Console.WriteLine(vectorlastColumn.ToString("#0.00\t", FormatProvider));
        }
        [Test]
        public void ZipVectors() 
        {
            var vectorCol = new DenseVector(new[] { 50.0, 51.0, 52.0 });
            var vectorRow = new DenseVector(new[] { 60.0, 61.0, 62.0 });
            var matrix = Matrix<double>.Build.DenseOfRowVectors(vectorCol);
            Console.WriteLine(matrix);
            var appmatrx = Matrix<double>.Build.DenseOfRowVectors(vectorRow);
            var newMatrix = matrix.Append(appmatrx);
            Console.WriteLine(newMatrix);
            Assert.AreEqual(vectorCol.Count + vectorRow.Count, newMatrix.RowCount * newMatrix.ColumnCount);
        }
    }
}