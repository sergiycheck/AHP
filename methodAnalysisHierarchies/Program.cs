using System;
using System.Collections.Generic;
using AnalysisMatrix;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;


namespace methodAnalysisHierarchies
{
    class Program
    {


        static void TestMathNumeric()
        {
            var m = Matrix<double>.Build.DenseOfArray(
                new double[,] { { 1, (double)1/3,(double) 1/2 },
                                { 3, 1, 3 },
                                { 2, (double)1/3, 1 } });
            Console.WriteLine(m);
            var evd = m.Evd(Symmetricity.Asymmetric);
            Console.WriteLine(evd.EigenVectors);

        }
        static void TestMathNumeric2()
        {
            var m = Matrix<double>.Build.DenseOfArray(
                new double[,] { { 4, -5,7 },
                                { 1, -4, 9 },
                                { -4, 0, 5 } });
            Console.WriteLine(m);
            var evd = m.Evd(Symmetricity.Asymmetric);
            Console.WriteLine(evd.EigenVectors);

        }

        static void InitFromCodeAnalyze()
        {
            
            var twoxtwomatrix = new double[,] {
                { 1, 3 }, 
                { 1f/4f, 1 } };

            var threexthreematrix = new double[,] { 
                                { 1, (double)1/3,(double) 1/2 },
                                { 3,            1,          3 },
                                { 2,  (double)1/3,          1 } };



            var fourxfourmatrix = new double[,] {
                {    1.0, 1.0 / 3.0,  2.0,     4.0 },
                {    3.0,       1.0,  5.0,     3.0 },
                {1.0/2.0,   1.0/5.0,  1.0, 1.0/3.0 },
                {1.0/4.0,   1.0/3.0,  3.0,     1.0 }};

            //1 compare creterais make matrix

            //2 compare choices make matrix

            //A - speed
            //B - color
            //C - Volume
            //D - Fuel consumption
            //if a to b = 2 than b to a = 1/2
            //if b to c = 3 than c to b = 1/3

            //                /*A*/  /*B*/  /*C*/ /*D*/
            //  var creteria2dArr = new double[,] {
            ///*A*/      {    1.0,   2.0,  5.0,  7.0 },
            ///*B*/      {    0.50,  1.0,  3.0,  5.0 },
            ///*C*/      {    0.20,  0.33, 1.0,  2.0 },
            ///*D*/      {    0.14,  0.2,  0.5,  1.0 }};
            
                           /*S*/       /*C*/    /*V*/ /*F*/
            var creteria2dArr = new double[,] {
          /*S*/      {    1.0,          2,      2.0,  1.0/5.0 },
          /*C*/      {    0.50,         1.0,    3.0,  1.0/5.0 },
          /*V*/      {    0.50,     1.0/3.0,    1.0,  1.0/3.0 },
          /*F*/      {    5.0,          5.0,    3.0,  1.0     }
            };

            //alternatives choices
            //SEDAN
            //COUPE
            //SPORTS
            //Crossover

            //compare on each criteria each alternative choice (4 measurement)

            //speed
            double[,] compareCarsBySpeed = new double[,]
                {  /*SEDAN*/  /*COUPE*/ /*SPORTS*/     /*Crossover*/
            /*SEDAN*/{1,        0.5,    0.5,                1.0/3.0},
            /*COUPE*/{2,        1,      1.0/3.0,                0.5},
           /*SPORTS*/{2,        3,          1,                  0.2},
        /*crossover*/{3,        2,          5,                    1},
                };

            //color
            double[,] compareCarsByColor = new double[,]
                {   /*SEDAN*/  /*COUPE*/    /*SPORTS*/     /*Crossover*/
            /*SEDAN*/{1,        1.0/7.0,    1.0/3.0,               0.5},
            /*COUPE*/{7,            1,       1.0/4.0,          1.0/3.0},
           /*SPORTS*/{3,            4,           1,            1.0/2.0},
        /*crossover*/{2,            3,           2,                  1},
                };

            double[,] compareCarsByVolume = new double[,]
                {   /*SEDAN*/  /*COUPE*/    /*SPORTS*/    /*Crossover*/
            /*SEDAN*/{1,            3.0,          2,         1.0/2.0},
            /*COUPE*/{1.0/3.0,        1,         3.0,              2},
           /*SPORTS*/{1.0/2.0,    1.0/3.0,         1,        1.0/4.0},
        /*crossover*/{2,             0.5,          4,              1}
                };

            double[,] compareCarsByConsumption = new double[,]
                {       /*SEDAN*/    /*COUPE*/    /*SPORTS*/  /*Crossover*/
            /*SEDAN*/{       1,        4,              4,      1.0/2.0},
            /*COUPE*/{  1.0/4.0,        1,              3,            2},
           /*SPORTS*/{  1.0/4.0,  1.0/3.0,             1,      1.0/3.0},
        /*crossover*/{        2,      0.5,             3,            1}
                };

            //double[,] compareCarsByConsumption = new double[,]
            //    {
            //        {          1,   9,  9},
            //        {    1.0/9.0,   1,  2},
            //        {    1.0/9.0, 0.2,  1}
            //    };

            List<double[,]> choicesList = new List<double[,]>()
            {
                compareCarsBySpeed,
                compareCarsByColor,
                compareCarsByVolume,
                compareCarsByConsumption
            };
            var alternatives = new List<string>() { 
                "SEDAN", 
                "COUPE", 
                "SPORTS",
                "CROSSOOVER" };


            var ahpmodel = new AhpModel();
            ahpmodel
                .CriteriaMatrixStatisticsJob(creteria2dArr);

            ahpmodel.AddChoiceList(choicesList);
            var Gvect = ahpmodel.GetGlobalVector();
            Console.WriteLine(Gvect);
            ahpmodel.PrintAlternativesNumbers(alternatives);

        }
 
        static void AhpBuilder() 
        {
            var builder = new  AhpBuilder();
            var creteria2dArr = builder.BuildCriteriasMultidimentionalArray();

            List<double[,]> choicesList = builder.BuildAndGetAllChoicesList();


            var ahpmodel = new AhpModel();
            ahpmodel
                .CriteriaMatrixStatisticsJob(creteria2dArr);

            ahpmodel.AddChoiceList(choicesList);
            var Gvect = ahpmodel.GetGlobalVector();
            Console.WriteLine(Gvect);
            var alternatives = builder.AlternativesList;
            ahpmodel.PrintAlternativesNumbers(alternatives);

        }
        static void ElectreBuilder() 
        {
            var electreBuilder = new Electre();
            var creteria2dArr = electreBuilder
                //.BuildCriteriasMultidimentionalArrayFromCode();
                .BuildCriteriasMultidimentionalArray();
            var agreementIndex = electreBuilder.AgreementIndex(creteria2dArr);
            var disAgreementIndex = electreBuilder.DisAgreementIndex(creteria2dArr);
            electreBuilder
                //.FinalDecision(agreementIndex, disAgreementIndex);
                .FinalDecisionFromMaxAverage(agreementIndex, disAgreementIndex);

        }
        static void Main(string[] args)
        {
            //InitFromCodeAnalyze();
            //TestMathNumeric2();
            //AhpBuilder();
            ElectreBuilder();


        }
    }
}
