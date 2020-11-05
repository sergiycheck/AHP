using System;
using System.Collections.Generic;
using System.Text;

namespace methodAnalysisHierarchies
{
    public class SimplexTableElementsContainer
    {
        public double [,] ExtendedMatrix { get; set; }
        public double [] ZiCoefs { get; set; }
        public double [] CiMinusZiCoefs { get; set; }
        public double [] CoefsOfBasicVariables { get; set; }
        public double [,] MatrixCoefs { get; set; }
        public double [] Solutions { get; set; }
        public double [] TargetFunctionCoefs { get; set; }
    }
}
