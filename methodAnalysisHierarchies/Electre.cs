using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace methodAnalysisHierarchies
{
    public class Electre
    {
        List<string> _criteriasList;
        List<string> _alternativesList;
        public List<string> CriteriasList { get => _criteriasList; }
        public List<string> AlternativesList { get => _alternativesList; }
        private int _numberOfCriterias;
        public int NumberOfCriterias { get=> _numberOfCriterias; }
        private string _allCriteriaNames;
        public string AllCriteriaNames { get => _allCriteriaNames;}
        public List<double> WeightsOFCriterias { get; private set; }
        private int _numberOfAlternatives;
        public int NuberOfAlternatives { get=> _numberOfAlternatives; }
        private string _allAlternativeNames;
        public string AllAlternativeNames { get=> _allAlternativeNames; }

        public double[,] BuildCriteriasMultidimentionalArrayFromCode()
        {
            _criteriasList = new List<string>();
            
            _numberOfCriterias = 4;
            _criteriasList = new List<string>() { "A", "B", "C","D"};
            _allCriteriaNames = "A B C D";

            
            WeightsOFCriterias = new List<double>() { 2, 2, 3, 2 };
            _alternativesList = new List<string>();
            
            _numberOfAlternatives = 3;
            _alternativesList = new List<string> { "math", "physics", "literature", "programing" };
            _allAlternativeNames = " math physics literature programing ";

            var creteria2dArr = new double[, ]{
                { 5,4,3,3 },
                { 1,2,5,2 },
                { 3,3,4,4 }
            };

            Console.WriteLine("Weights");
            Console.WriteLine(AllCriteriaNames);
            Console.WriteLine(Vector<double>.Build.DenseOfArray(WeightsOFCriterias.ToArray()));

            Console.WriteLine(Matrix<double>.Build.DenseOfArray(creteria2dArr));
            return creteria2dArr;
        }


        public double[,] BuildCriteriasMultidimentionalArray()
        {
            _criteriasList = new List<string>();
            ValuesFullFiller("criterias", ref _numberOfCriterias, ref _criteriasList, ref _allCriteriaNames);
            Console.WriteLine("enter weights of criterias");
            WeightsOFCriterias = ReadRow(NumberOfCriterias);
            _alternativesList = new List<string>();
            ValuesFullFiller("alternatives",ref _numberOfAlternatives, ref _alternativesList, ref _allAlternativeNames);

            var creteria2dArr = new double[NuberOfAlternatives,NumberOfCriterias];
            int rownum = 0;
            foreach (var name in _alternativesList)
            {
                Console.WriteLine($"Enter values of criterias");
                Console.WriteLine($"alternative {name} to {AllCriteriaNames}");

                var rowValuesOfCriterias = ReadRow(NumberOfCriterias);

                for (int colnum = 0; colnum < NumberOfCriterias; colnum++)
                {
                    creteria2dArr[rownum, colnum] = rowValuesOfCriterias[colnum];
                }
                rownum++;
            }
            Console.WriteLine("Weights");
            Console.WriteLine(AllCriteriaNames);
            Console.WriteLine(Vector<double>.Build.DenseOfArray(WeightsOFCriterias.ToArray()));

            Console.WriteLine(Matrix<double>.Build.DenseOfArray(creteria2dArr));
            return creteria2dArr;
        }
        public void ValuesFullFiller(string valueName, ref int numberToInit, ref List<string> listToInit, ref string valueToInit)
        {
            Console.WriteLine($"enter number of {valueName}");
            var value = Console.ReadLine();
            int.TryParse(value, out int criterias);

            numberToInit = criterias;

            FullFillList(criterias, listToInit, valueName);

            valueToInit = ConcatAllListNames(_criteriasList);
        }
        public List<double> ReadRow(int countOfNumbers)
        {
            var text = Console.ReadLine();
            if (text != null || text != "")
            {
                var matches = AhpBuilder.GetMatches(AhpBuilder.numMatcher, text);
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
        public void FullFillList(int num, List<string> valueList, string nameOfTheList)
        {
            for (int i = 0; i < num; i++)
            {
                Console.WriteLine($"enter name of {nameOfTheList} {i + 1}");
                valueList.Add(Console.ReadLine());
            }
        }
        private string ConcatAllListNames(List<string> valueList)
        {
            return string.Join(" ", valueList);
        }

        public double[,] AgreementIndex(double[,] creteria2dArr) 
        {
            var agreetmentIndex2dArr = Matrix<double>.Build.DenseOfArray(new double[NuberOfAlternatives, NuberOfAlternatives]);
            var weightsSum = Vector<double>.Build.DenseOfEnumerable(WeightsOFCriterias).Sum();
            for(int row = 0;row< _numberOfAlternatives; row++) 
            {
                var measuringRow = Matrix<double>.Build.DenseOfArray(creteria2dArr).Row(row);

                for (int compearingRowInd = 0; compearingRowInd < _numberOfAlternatives; compearingRowInd++)
                {
                    
                    if(row!= compearingRowInd) 
                    {
                        var compearingRow = Matrix<double>.Build.DenseOfArray(creteria2dArr).Row(compearingRowInd);
                        
                        var sum = 0.0;
                        for (int el = 0;el< _numberOfCriterias; el++) 
                        {
                            if(measuringRow[el]>= compearingRow[el]) 
                            {
                                sum += WeightsOFCriterias[el];
                            }
                        }

                        agreetmentIndex2dArr[row, compearingRowInd] = sum/ weightsSum;

                    }
                    else 
                    {
                        agreetmentIndex2dArr[row, compearingRowInd] = 0;
                    }

                }
            }
            Console.WriteLine("Agreement index matrix");
            Console.WriteLine(agreetmentIndex2dArr);
            return agreetmentIndex2dArr.ToArray();
        }
        public double[,] DisAgreementIndex(double[,] creteria2dArr)
        {
            var disAgreetmentIndex2dArr = Matrix<double>.Build.DenseOfArray(new double[NuberOfAlternatives, NuberOfAlternatives]);
            var length = (double)Matrix<double>.Build.DenseOfArray(creteria2dArr).ToColumnMajorArray().ToList().Distinct().Count()-1;
            for (int row = 0; row < _numberOfAlternatives; row++)
            {
                var measuringRow = Matrix<double>.Build.DenseOfArray(creteria2dArr).Row(row);

                for (int compearingRowInd = 0; compearingRowInd < _numberOfAlternatives; compearingRowInd++)
                {

                    if (row != compearingRowInd)
                    {
                        var compearingRow = Matrix<double>.Build.DenseOfArray(creteria2dArr).Row(compearingRowInd);

                        var substractionsList = new List<double>();
                        for (int el = 0; el < _numberOfCriterias; el++)
                        {
                            substractionsList.Add(compearingRow[el] - measuringRow[el]); 
                        }

                        var max = substractionsList.Max();

                        disAgreetmentIndex2dArr[row, compearingRowInd] = max / length;

                    }
                    else
                    {
                        disAgreetmentIndex2dArr[row, compearingRowInd] = 0;
                    }

                }
            }
            Console.WriteLine("Disagreement index matrix");
            Console.WriteLine(disAgreetmentIndex2dArr);
            return disAgreetmentIndex2dArr.ToArray();

        }
        public double[,] GetBinary(double[,] matrix,
                        Func<double,double,bool> compG,
                        double max,
                        double[,]matrixSecond,
                        Func<double, double, bool> compL,
                        double min) 
        {
            var binaryMatrix = new double[_numberOfAlternatives, _numberOfAlternatives];
            for (int i = 0; i < _numberOfAlternatives; i++)
            {
                for (int j = 0; j < _numberOfAlternatives; j++)
                {
                    if (compG(matrix[i, j], max)&& compL(matrixSecond[i,j],min))
                    {
                        binaryMatrix[i, j] = 1;
                    }
                    else
                    {
                        binaryMatrix[i, j] = 0;
                    }
                }
            }
            return binaryMatrix;
        }
        public double GetAverage(double [,] matrix) 
        {
            var agreeMatrix = Matrix<double>.Build
            .DenseOfArray(matrix)
            .ToColumnMajorArray();

            var countOfElsAgM = agreeMatrix.Where(el => el > 0).Count();
            var maxElAgerage = agreeMatrix.Sum() / countOfElsAgM;
            return maxElAgerage;
        }
        public void FinalDecision(double[,] agreementIndexMatrix, double[,] disAgreementIndexMatrix)
        {
            var binaryRepresentationIndex = new double[_numberOfAlternatives, _numberOfAlternatives];

            var max = (double)Matrix<double>.Build
                        .DenseOfArray(agreementIndexMatrix)
                        .ToColumnMajorArray()
                        .ToList().Max();

            var min = (double)Matrix<double>.Build
                        .DenseOfArray(disAgreementIndexMatrix)
                        .ToColumnMajorArray()
                        .ToList().Where(el => el > 0).Min();

            Func<double, double, bool> greaterThanOrEqual = (first, second) => first >= second;
            Func<double, double, bool> lessThanOrEqual = (first, second) => first <= second;

            binaryRepresentationIndex = GetBinary(agreementIndexMatrix,
                                                    greaterThanOrEqual,
                                                    max,
                                                    disAgreementIndexMatrix,
                                                    lessThanOrEqual,
                                                    min);

            Console.WriteLine(Matrix<double>.Build.DenseOfArray(binaryRepresentationIndex));
   
        }
        public void FinalDecisionFromMaxAverage(double[,] agreementIndexMatrix, double[,] disAgreementIndexMatrix) 
        {
            var binaryRepresentationIndex = new double[_numberOfAlternatives, _numberOfAlternatives];
            var max = GetAverage(agreementIndexMatrix);
            var min = GetAverage(disAgreementIndexMatrix);
            Func<double, double, bool> greaterThanOrEqual = (first, second) => first >= second;
            binaryRepresentationIndex = GetBinary(agreementIndexMatrix,
                                                    greaterThanOrEqual,
                                                    max,
                                                    disAgreementIndexMatrix,
                                                    greaterThanOrEqual,
                                                    min);
            Console.WriteLine(Matrix<double>.Build.DenseOfArray(binaryRepresentationIndex));
        }

    }
}
