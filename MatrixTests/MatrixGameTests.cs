using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using methodAnalysisHierarchies;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using methodAnalysisHierarchies.MatrixGame;
namespace MatrixTests
{
    public class MatrixGameTests
    {
        [Test]
        public void MatrixGameSaddlePointTest() 
        {
            var matrixCoefs = new double[,]
            {
                 { 5,  -8,  7,   -6,  0},
                 { 8,  -5,  9,   -3,  2},
                 {-2,   7, -3,    6, -4},

            };
            var matrixGame = new MatrixGame();
            Assert.IsFalse(matrixGame.CheckForSaddlePoint(matrixCoefs));
        }
        [Test]
        public void CheckForDominantColumnsAndRowsTest()
        {
            var matrixCoefs = new double[,]
            {
                 { 5,  -8,  7,   -6,  0},
                 { 8,  -5,  9,   -3,  2},
                 {-2,   7, -3,    6, -4},

            };
            var matrixCoefsExpected = new double[,]
            {
                 { -5,  -3,  2},
                 {  7,   6, -4},

            };
            var matrixGame = new MatrixGame();
            Assert.AreEqual(matrixCoefsExpected, matrixGame.CheckForDominantColumnsAndRows(matrixCoefs));
        }
        [Test]
        public void GetRidOfNegativeValuesTest()
        {
            
            var matrixCoefs = new double[,]
            {
                 { -5,  -3,  2},
                 {  7,   6, -4},

            };
            var matrixCoefsExpected = new double[,]
            {
                  { 0,  2,  7,},
                  { 12, 11, 1},

            };
            var matrixGame = new MatrixGame();
            Assert.AreEqual(matrixCoefsExpected, matrixGame.GetRidOfNegativeValues(matrixCoefs));
        }
        [Test]
        public void MakeInequalitySystemForAPlayerTest()
        {

            var matrixCoefs = new double[,]
            {
                  { 0,  2,  7,},
                  { 12, 11, 1},

            };
            var matrixGame = new MatrixGame();
            matrixGame.MakeInequalitySystemForAPlayer(matrixCoefs);
        }
        [Test]
        public void InitGameTest() 
        {
            //https://math.semestr.ru/games/gamesimplex.php
            var matrixCoefs = new double[,]
            {
                 { 5,  -8,  7,   -6,  0},
                 { 8,  -5,  9,   -3,  2},
                 {-2,   7, -3,    6, -4},

            };
            var matrixGame = new MatrixGame(
                new SimplexMethodInitializer(
                    new SimplexMethod(
                        new MatrixConsolePrinter())));
            matrixGame.InitGame(matrixCoefs);
            Assert.AreEqual(Math.Round(14.0 / 3.0, 2), Math.Round(matrixGame.GamePrice, 2));
        }
        [Test]
        public void InitGameTest2() 
        {
            var matrixCoefs = new double[,]
            {
                 {3, -3, 2},
                 {7,  4, 1},
                 {-6, 2, 6}

            };
            var matrixGame = new MatrixGame(
                new SimplexMethodInitializer(
                    new SimplexMethod(
                        new MatrixConsolePrinter())));
            matrixGame.InitGame(matrixCoefs);
            Assert.AreEqual(Math.Round(26.0/3.0, 1), Math.Round(matrixGame.GamePrice, 1));
        }
        [Test]
        public void InitGameTest3()
        {
            //https://math.semestr.ru/games/linear-programming.php
            var matrixCoefs = new double[,]
            {
                 {0, 0.3, 0.5},
                 {0.6, 0.1, -0.1},
                 {0.4, 0.2, 0.1}

            };
            var matrixGame = new MatrixGame(
                new SimplexMethodInitializer(
                    new SimplexMethod(
                        new MatrixConsolePrinter())));
            matrixGame.InitGame(matrixCoefs);
            Assert.AreEqual(Math.Floor(0.25), Math.Floor(matrixGame.GamePrice));
        }
    }
}
