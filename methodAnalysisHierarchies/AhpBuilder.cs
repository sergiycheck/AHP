using AnalysisMatrix;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace methodAnalysisHierarchies
{
    class AhpBuilder
    {
        List<string> _criteriasList;
        List<string> _alternativesList;
        public List<string> CriteriasList { get => _criteriasList; }
        public List<string> AlternativesList { get => _alternativesList; }

        public int NumberOfCriterias { get; private set; }

        public const string numMatcher = @"[0-9]+\.?[0-9]*\/?[0-9]*\.?[0-9]*";

        public const string onlyNumMatcher = @"[0-9]+\.?[0-9]*";

        public AhpBuilder() 
        {
            _criteriasList = new List<string>();
            _alternativesList = new List<string>();
        }

        public void FullFillList(int num,List<string> valueList,string nameOfTheList) 
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

        public static MatchCollection GetMatches(string matchExpression, string text)
        {
            var regularExpression = new Regex(matchExpression);
            return regularExpression.Matches(text);
        }

        
        public static List<double> ConvertMatchesToDoubles(MatchCollection matches)
        {
            var row = new List<double>();
            foreach(Match match in matches) 
            {
                if (match.Value.Contains(@"/")) 
                {
                    var twoNumsMatch = GetMatches(onlyNumMatcher, match.Value);
                    double.TryParse(twoNumsMatch[0].Value,out double num1);
                    double.TryParse(twoNumsMatch[1].Value,out double num2);
                    row.Add(num1 / num2);
                }
                else 
                {
                    double.TryParse(match.Value, out double num);
                    row.Add(num);
                }
            }

            return row;

        }


        public double[,] BuildCriteriasMultidimentionalArray() 
        {
            Console.WriteLine("enter number of criterias");
            var value = /*"2";*/Console.ReadLine();
            int.TryParse(value, out int criterias);

            NumberOfCriterias = criterias;

            /*_criteriasList = new List<string>() { 
                "speed", 
                "color" };*/
            FullFillList(criterias, _criteriasList, "criterias");

            var allCriteriaNames = ConcatAllListNames(_criteriasList);

            var creteria2dArr = new double[_criteriasList.Count, _criteriasList.Count];
            int rownum = 0;
            foreach (var name in _criteriasList)
            {
                Console.WriteLine($"Enter values compare to other criterias");
                Console.WriteLine($"{name} compare to other {allCriteriaNames}");
                var text = Console.ReadLine();
                var matches = GetMatches(numMatcher, text);
                if (matches.Count != _criteriasList.Count)
                {
                    Console.Write("Values not correct"); return new double[,] { };
                }
                var row = ConvertMatchesToDoubles(matches);
                for (int colnum = 0; colnum < _criteriasList.Count; colnum++)
                {
                    creteria2dArr[rownum, colnum] = row[colnum];
                }
                rownum++;
            }
            Console.WriteLine(Matrix<double>.Build.DenseOfArray(creteria2dArr));
            return creteria2dArr;
        }

        public List<double[,]> BuildAndGetAllChoicesList()
        {
            Console.WriteLine("enter number of alternatives");
            var value = /*"2";*/ Console.ReadLine();
            int.TryParse(value, out int alternatives);

            /*_alternativesList = new List<string>() {"sedan", "priora" };*/
            FullFillList(alternatives, _alternativesList, "alternatives");

            var allAlternativeNames = ConcatAllListNames(_alternativesList);
            List<double[,]> choicesList = new List<double[,]>();

            for (int i = 0; i < NumberOfCriterias; i++) 
            {
                Console.WriteLine($"Compare alternatives by {_criteriasList[i]}");
                var Alternatives2dArr = new double[alternatives, alternatives];
                int rownum = 0;
                foreach (var alternativeName in _alternativesList)
                {
                    Console
                        .WriteLine($"Enter values compare to other alternative choices");
                    Console.WriteLine($"{alternativeName} compare to other {allAlternativeNames}");
                    var text = Console.ReadLine();
                    var matches = GetMatches(numMatcher, text);
                    if (matches.Count != _alternativesList.Count)
                    {
                        Console.Write("Values not correct"); return null;
                    }
                    var rowlist = ConvertMatchesToDoubles(matches);
                    for (int colnum = 0; colnum < _alternativesList.Count; colnum++)
                    {
                        Alternatives2dArr[rownum, colnum] = rowlist[colnum];
                    }
                    rownum++;
                }
                Console.WriteLine(Matrix<double>.Build.DenseOfArray(Alternatives2dArr));
                choicesList.Add(Alternatives2dArr);
            }


            return choicesList;

        }




        public void BuildAhpModel() 
        {
            Console.WriteLine("enter number of criterias");
            var value = "2";// Console.ReadLine();
            int.TryParse(value, out int criterias);

            NumberOfCriterias = criterias;

            Console.WriteLine("enter number of alternatives");
            value = "2";// Console.ReadLine();
            int.TryParse(value, out int alternatives);

            _criteriasList = new List<string>() { "speed", "color" };//FullFillList(criterias, _criteriasList, "criterias");
            _alternativesList = new List<string>() {"sedan","priora" };//FullFillList(alternatives, _alternativesList, "alternatives");

            var allCriteriaNames = ConcatAllListNames(_criteriasList);
            var allAlternativeNames = ConcatAllListNames(_alternativesList);

            var creteria2dArr = new double[_criteriasList.Count, _criteriasList.Count];
            int rownum = 0;
            foreach (var name in _criteriasList) 
            {
                Console.WriteLine($"Enter values compare to other criterias");
                Console.WriteLine($"{name} compare to other {allCriteriaNames}");
                var text = Console.ReadLine();
                var matches = GetMatches(numMatcher, text);
                if (matches.Count != _criteriasList.Count) 
                {
                    Console.Write("Values not correct");return;
                }
                var rowlist = ConvertMatchesToDoubles(matches);
                for(int colnum = 0;colnum< _criteriasList.Count; colnum++) 
                {
                    creteria2dArr[rownum, colnum] = rowlist[colnum];
                }
                rownum++;
            }
            Console.WriteLine(Matrix<double>.Build.DenseOfArray(creteria2dArr));

        }
    }
}
