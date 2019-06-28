using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qpcr
{
    public class Generator
    {
        public List<List<List<string>>> Generate(int plateSize, List<List<string>> samples, List<List<string>> reagents, List<int> replicates)
        {
            // validate input
            if (samples.Count != reagents.Count || samples.Count != replicates.Count)
            {
                throw new Exception("Not all lists have the same amount of experiments.");
            }
            int experimentsCount = samples.Count;

            // initialize end result
            var result = new List<List<List<string>>>();

            // generate for each experiment
            for (int experimentIndex = 0; experimentIndex < experimentsCount; experimentIndex++)
            {
                var currentSamples = samples[experimentIndex];
                var currentReagents = reagents[experimentIndex];
                var currentReplicates = replicates[experimentIndex];


                var currentExperimentResult = new List<List<string>>();
                foreach (var sample in currentSamples)
                {
                    foreach (var reagent in currentReagents)
                    {
                        currentExperimentResult.AddRange(Enumerable.Repeat(new List<string> { sample, reagent }, currentReplicates));
                    }
                }




                result.Add(new List<List<string>>());
            }


            return result;
        }
    }
}
