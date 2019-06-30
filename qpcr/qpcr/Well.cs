using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qpcr
{
   public class Well
    {
        public string Reagent { get; set; }
        public string Sample { get; set; }
        public int Experiment { get; set; }

        public override string ToString()
        {
            return $"{Sample} | {Reagent} | {Experiment}";
        }
    }
}
