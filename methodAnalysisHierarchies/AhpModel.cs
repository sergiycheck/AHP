using AnalysisMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace methodAnalysisHierarchies
{
    public class AhpModel
    {
        private int _globalMatrixPrioritiesRows;
        private AnalysisMatrix.Matrix _matrix;
        private List<KeyValuePair<int, double>> _globalVectorAndAlterative;
        public AnalysisMatrix.Matrix CriteriaMatrix
        {
            get => _matrix;
            set 
            {
                //Check matrix
                var issquare = CheckIfSquare(value);
                var isDiagOne = CheckIfDiagonalElementsIsOne(value);
                if (issquare && isDiagOne)
                    _matrix = value;
                else
                    Console.WriteLine("Matrix not square or diagonal elements not equal to one");
            }
        }

        public Dictionary<int, AnalysisMatrix.Matrix> number_AlternativeChoiceCollection { get; set; }

        public AhpModel() 
        {
            number_AlternativeChoiceCollection = new Dictionary<int, Matrix>();
        }

        public bool CheckIfDiagonalElementsIsOne(Matrix matrix)
        {
            for(int i = 0; i < matrix.Rows; i++) 
            {
                if (matrix[i, i] != 1)
                    return false;
            }
            return true;
        }
        public bool CheckIfSquare(Matrix matrix)
        {
            return (matrix.Rows == matrix.Columns) ? true : false;
        }



        public void CriteriaMatrixStatisticsJob(double[,] creteria2dArr) 
        {
            var creteriaMatrix = new Matrix(
                creteria2dArr.GetLength(0),
                creteria2dArr.GetLength(1));

            creteriaMatrix.InitializeMatrix(creteria2dArr);
            
            CriteriaMatrix = creteriaMatrix;

            Console.WriteLine(Matrix<double>.Build.DenseOfArray(CriteriaMatrix.Arr));


            var vect = creteriaMatrix.GetOwnVectorByMultiplication();
            Console.WriteLine("Priority vector");
            Console.WriteLine(Vector<double>.Build.DenseOfArray(vect));
            ///

            var m1 = Matrix<double>.Build.DenseOfArray(creteriaMatrix.Arr);
            var vect1 = Vector<double>.Build.DenseOfArray(vect);
            var Y = m1.Multiply(vect1);
            var elem = m1.Evd(Symmetricity.Unknown);

            var lambdaMax = creteriaMatrix.GetMaxValueAsAverage(Y.ToArray(), vect);

            var evdvalsLambda = elem.EigenValues[0].Magnitude;
            if (evdvalsLambda < lambdaMax)
                lambdaMax = evdvalsLambda;

            var consistencyIndex = creteriaMatrix.GetConsistencyIndex(lambdaMax);
            var consistencyRatio = creteriaMatrix.GetConsistencyRatio(consistencyIndex);

            Console.WriteLine($"lambdaMax = {creteriaMatrix.LambdaMax}");
            Console.WriteLine($"Consistency index = {creteriaMatrix.ConsistencyIndex}");
            Console.WriteLine($"Consistency ratio = {creteriaMatrix.ConsistencyRatio}");

        }
        public void AddChoiceList(List<double[,]> choicesList) 
        {
            var wasset = false;   

            for (var i = 0; i < choicesList.Count; i++)
            {
                var choice = choicesList[i];
                //set number of rows
                if (!wasset)
                    _globalMatrixPrioritiesRows = choice.GetLength(0);

                var choiceMatrix = new Matrix(choice.GetLength(0), choice.GetLength(1));
                choiceMatrix.InitializeMatrix(choice);
                if (CheckIfDiagonalElementsIsOne(choiceMatrix)
                    &&
                    CheckIfSquare(choiceMatrix))
                {
                    //add all alternative choices and evaluate vector, lambdaMax
                    //consistencyIndex,consistencyRatio
                    AddAllAlternativeChoices(i, choiceMatrix);

                }
                else 
                {
                    Console.WriteLine("Matrix not square or diagonal elements not equal to one");
                }
            }
        }


        public void AddAllAlternativeChoices(int i, Matrix choiceMatrix) 
        {
            
            Console.WriteLine($"Choice number {i}");
            Console.WriteLine(Matrix<double>.Build.DenseOfArray(choiceMatrix.Arr));

            //gettring vector
            var vect = choiceMatrix.GetOwnVectorByMultiplication();
            Console.WriteLine("Priority vector");
            Console.WriteLine(Vector<double>.Build.DenseOfArray(vect));
            
            //multipty for lambdamax and other statistics
            var m1 = Matrix<double>.Build.DenseOfArray(choiceMatrix.Arr);
            var vect1 = Vector<double>.Build.DenseOfArray(vect);
            var vect2 = Vector<double>.Build.DenseOfArray(vect);
            var Y = m1.Multiply(vect1);
            Y = Y.PointwiseDivide(vect2);
            var elem = m1.Evd(Symmetricity.Unknown);


            //gettring values
            var lambdaMax = choiceMatrix.GetMaxValueAsAverage(Y.ToArray());

            var evdvalsLambda = elem.EigenValues[0].Magnitude;
            if (evdvalsLambda < lambdaMax)
                lambdaMax = evdvalsLambda;

            var consistencyIndex = choiceMatrix.GetConsistencyIndex(lambdaMax);
            var consistencyRatio = choiceMatrix.GetConsistencyRatio(consistencyIndex);

            Console.WriteLine($"lambdaMax = {choiceMatrix.LambdaMax}");
            Console.WriteLine($"Consistency index = {choiceMatrix.ConsistencyIndex}");
            Console.WriteLine($"Consistency ratio = {choiceMatrix.ConsistencyRatio}");

            number_AlternativeChoiceCollection.Add(i, choiceMatrix);
        }

        public Vector<double> GetGlobalVector() 
        {
            //composing matrix from local vector priorities alternatives

            var globalVector = Vector<double>.Build.Dense(_globalMatrixPrioritiesRows);
            _globalVectorAndAlterative = new List<KeyValuePair<int, double>>();

            Console.Write("all vectors of alternative choices \n");
            for(int j = 0;j< _globalMatrixPrioritiesRows; j++) 
            { 
                for (int l = 0;l<number_AlternativeChoiceCollection.Count;l++)
                {
                    Console.Write($"{number_AlternativeChoiceCollection[l].OwnVector[j]} \t");

                    globalVector[j] += 
                        number_AlternativeChoiceCollection[l].OwnVector[j]* CriteriaMatrix.OwnVector[l];
                }
                Console.Write("\n"); 
                _globalVectorAndAlterative.Add(new(j, globalVector[j]));
            }


            return globalVector;

        }

        public void PrintAlternativesNumbers(List<string> alternatives) 
        {
            _globalVectorAndAlterative.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            Console.WriteLine("Rank of alternatives");

            int i = 1;
            foreach(var el in _globalVectorAndAlterative) 
            {
                Console.Write($" {i} is {alternatives[el.Key]} {el.Key} \t");
                i++;
            }
            Console.Write("\n");
        }



    }
}
