using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace methodAnalysisHierarchies
{
    public class Inequality
    {
        public static readonly string [] SignsCollection =  { "<=", "=", ">=" };
        public List<double> Coefs { get; set; }
        private string _sign;
        public  string Sign { get => _sign;
            set 
            {
                if (SignsCollection.Contains(value)) 
                {
                    _sign = value;
                }
                else 
                {
                    throw new ArgumentException($"This value is not valid {value}/ Try this values {string.Join(" ",SignsCollection)}");
                }
            }
        }
        public double Solution { get; set; }
        
        
    }
    public class InequalitySystem 
    {
        public InequalitySystem() 
        {
            Inequalities = new List<Inequality>();
            TargetFunctionCoefs = new List<double>();
        }
        public List<Inequality> Inequalities { get; set; }
        public List<double> TargetFunctionCoefs { get; set; }
    }


}
