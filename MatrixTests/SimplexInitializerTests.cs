using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using methodAnalysisHierarchies;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;


namespace MatrixTests
{
    public class SimplexInitializerTests
    {
        public InequalitySystem GetSimplexElemContainer1() 
        {
            var inequalitySystem = new InequalitySystem()
            {
                Inequalities = new List<Inequality>()
                {
                    new Inequality()
                    {
                        Coefs = new List<double>() { 9, 13, 8 },
                        Sign = ">=",
                        Solution = 1
                    },
                    new Inequality()
                    {
                        Coefs = new List<double>() { 3, 10, 8 },
                        Sign = ">=",
                        Solution = 1
                    },
                    new Inequality()
                    {
                        Coefs = new List<double>() { 8, 7, 12 },
                        Sign = ">=",
                        Solution = 1
                    },

                },
                TargetFunctionCoefs = new List<double> { 1, 1, 1 }
            };
            return inequalitySystem;

        }
        [Test]
        public void SimplexMethodInitializedToCanonicalForm()
        {
            var mockPrinter = new Mock<IMatrixPrinter>();
            var mockSimplexMethod = new Mock<SimplexMethod>(mockPrinter.Object);
            var initializef = new SimplexMethodInitializer(mockSimplexMethod.Object);

            var simplexElemCont = initializef.ToCanonicalForm(GetSimplexElemContainer1());
            Assert.IsAssignableFrom<SimplexTableElementsContainer>(simplexElemCont);
            var matrixCoefs = new double[,]
            {
                 {-9,   -13,  -8 ,  1,   0,   0},
                 {-3,   -10,  -8 ,  0,   1,   0},
                 {-8,   -7 ,  -12,  0,   0,   1}

            };

            var solutionCoefs = new double[] { -1, -1, -1 };
            var maximizationFunctionCoefs = new double[] { 1, 1, 1, 0, 0, 0 };
            Assert.AreEqual(matrixCoefs, simplexElemCont.MatrixCoefs);
            Assert.AreEqual(solutionCoefs, simplexElemCont.Solutions);
            Assert.AreEqual(maximizationFunctionCoefs, simplexElemCont.TargetFunctionCoefs);
        }
        public InequalitySystem GetSimplexElemContainerv2()
        {
            var inequalitySystem = new InequalitySystem()
            {
                Inequalities = new List<Inequality>()
                {
                    new Inequality()
                    {
                        Coefs = new List<double>() { 9, 13, 8 },
                        Sign = "<=",
                        Solution = 1
                    },
                    new Inequality()
                    {
                        Coefs = new List<double>() { 3, 10, 8 },
                        Sign = "<=",
                        Solution = 1
                    },
                    new Inequality()
                    {
                        Coefs = new List<double>() { 8, 7, 12 },
                        Sign = "<=",
                        Solution = 1
                    },

                },
                TargetFunctionCoefs = new List<double> { 1, 1, 1 }
            };
            return inequalitySystem;

        }
        [Test]
        public void SimplexMethodInitializedToCanonicalFormv2()
        {
            var mockPrinter = new Mock<IMatrixPrinter>();
            var mockSimplexMethod = new Mock<SimplexMethod>(mockPrinter.Object);
            var initializef = new SimplexMethodInitializer(mockSimplexMethod.Object);

            var simplexElemCont = initializef.ToCanonicalForm(GetSimplexElemContainerv2());
            Assert.IsAssignableFrom<SimplexTableElementsContainer>(simplexElemCont);
            var matrixCoefs = new double[,]
            {
                 { 9,    13,   8 ,  1,   0,   0},
                 { 3,    10,   8 ,  0,   1,   0},
                 { 8,    7 ,   12,  0,   0,   1}

            };

            var solutionCoefs = new double[] { 1, 1, 1 };
            var maximizationFunctionCoefs = new double[] { 1, 1, 1, 0, 0, 0 };
            Assert.AreEqual(matrixCoefs, simplexElemCont.MatrixCoefs);
            Assert.AreEqual(solutionCoefs, simplexElemCont.Solutions);
            Assert.AreEqual(maximizationFunctionCoefs, simplexElemCont.TargetFunctionCoefs);
        }
        public InequalitySystem GetSimplexElemContainer2()
        {
            var inequalitySystem = new InequalitySystem()
            {
                Inequalities = new List<Inequality>()
                {
                    new Inequality()
                    {
                        Coefs = new List<double>() { 10, 20 },
                        Sign = "<=",
                        Solution = 120
                    },
                    new Inequality()
                    {
                        Coefs = new List<double>() { 8, 8},
                        Sign = "<=",
                        Solution = 80
                    },


                },
                TargetFunctionCoefs = new List<double> { 12, 16, }
            };
            return inequalitySystem;

        }
        [Test]
        public void SimplexMethodInitializedToCanonicalForm2()
        {
            var mockPrinter = new Mock<IMatrixPrinter>();
            var mockSimplexMethod = new Mock<SimplexMethod>(mockPrinter.Object);
            var initializef = new SimplexMethodInitializer(mockSimplexMethod.Object);

            var simplexElemCont = initializef.ToCanonicalForm(GetSimplexElemContainer2());
            Assert.IsAssignableFrom<SimplexTableElementsContainer>(simplexElemCont);
            var matrixCoefs = new double[,]
            {
                {10, 20, 1, 0 },
                {8,   8, 0, 1 }
            };
            var solutionCoefs = new double[] { 120, 80 };
            var maximizationFunctionCoefs = new double[] { 12, 16, 0, 0 };
            Assert.AreEqual(matrixCoefs, simplexElemCont.MatrixCoefs);
            Assert.AreEqual(solutionCoefs, simplexElemCont.Solutions);
            Assert.AreEqual(maximizationFunctionCoefs, simplexElemCont.TargetFunctionCoefs);
        }


        public InequalitySystem GetSimplexElemContainer3()
        {
            var inequalitySystem = new InequalitySystem()
            {
                Inequalities = new List<Inequality>()
                {
                    new Inequality()
                    {
                        Coefs = new List<double>() {  3,    2,   0,   0, },
                        Sign = ">=",
                        Solution = 10
                    },
                    new Inequality()
                    {
                        Coefs = new List<double>() { 1,    0,   0,   0, },
                        Sign = "<=",
                        Solution = 15
                    },
                    new Inequality()
                    {
                        Coefs = new List<double>() { 1,    2,   0,   0, },
                        Sign = "<=",
                        Solution = 10
                    },
                    new Inequality()
                    {
                        Coefs = new List<double>() { -2,   -4,   1,   1,},
                        Sign = "=",
                        Solution = 7
                    },
                    new Inequality()
                    {
                        Coefs = new List<double>() { -2,    0,   1,   5, },
                        Sign = "=",
                        Solution = 21
                    },


                },
                TargetFunctionCoefs = new List<double> { -2, -1, 0, 0, }
            };
            return inequalitySystem;

        }
        [Test]
        public void SimplexMethodInitializedToCanonicalForm3()
        {
            var mockPrinter = new Mock<IMatrixPrinter>();
            var mockSimplexMethod = new Mock<SimplexMethod>(mockPrinter.Object);
            var initializef = new SimplexMethodInitializer(mockSimplexMethod.Object);

            var simplexElemCont = initializef.ToCanonicalForm(GetSimplexElemContainer3());
            Assert.IsAssignableFrom<SimplexTableElementsContainer>(simplexElemCont);
            var matrixCoefs = new double[,]
            {
                { -3,   -2,   0,   0,   1,   0,  0 },
                {  1,    0,   0,   0,   0,   1,  0 },
                {  1,    2,   0,   0,   0,   0,  1 },
                { -2,   -4,   1,   1,   0,   0,  0 },
                { -2,    0,   1,   5,   0,   0,  0 }
            };
            var solutionCoefs = new double[] {-10, 15, 10, 7, 21 };
            var maximizationFunctionCoefs = new double[] { -2, -1, 0, 0, 0, 0, 0 };
            Assert.AreEqual(matrixCoefs, simplexElemCont.MatrixCoefs);
            Assert.AreEqual(solutionCoefs, simplexElemCont.Solutions);
            Assert.AreEqual(maximizationFunctionCoefs, simplexElemCont.TargetFunctionCoefs);
        }
    }
}
