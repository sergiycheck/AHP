using System;

namespace AnalysisMatrix 
{
    public class ConsistentyRatio 
    {
        private readonly static double[] arr = { 0.0, 0.0, 0.58, 0.90, 1.12, 1.24, 1.32, 1.41, 1.45, 1.49 };
        public double this[int row]
        {
            get => arr[row];
        }
    }
}