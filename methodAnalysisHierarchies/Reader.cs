using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace methodAnalysisHierarchies
{
    public class Reader
    {
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
    }
}
