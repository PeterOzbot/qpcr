using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qpcr
{
    public class Generator
    {
        public List<Well[,]> Generate(int plateSize, List<List<string>> samples, List<List<string>> reagents, List<int> replicates)
        {
            // validate input and get experiment count
            if (samples.Count != reagents.Count || samples.Count != replicates.Count)
            {
                throw new Exception("Not all lists have the same amount of experiments.");
            }
            int experimentsCount = samples.Count;

            // initialize end result
            var result = new List<Well[,]>();
            int currentRowIndex = 0;
            int currentColumnIndex = 0;
            int maxRowCount;
            int maxColumnCount;
            Well[,] plate = GetPlate(plateSize, out maxRowCount, out maxColumnCount);
            result.Add(plate);

            // generate for each experiment
            for (int experimentIndex = 0; experimentIndex < experimentsCount; experimentIndex++)
            {
                // get experiment data
                var currentSamples = samples[experimentIndex];
                var currentReagents = reagents[experimentIndex];
                var currentReplicates = replicates[experimentIndex];

                // create combinations and fill plate
                foreach (var sample in currentSamples)
                {
                    foreach (var reagent in currentReagents)
                    {
                        foreach (var well in Enumerable.Repeat(new Well { Sample = sample, Reagent = reagent, Experiment = experimentIndex }, currentReplicates))
                        {
                            if (IsPlateFull(currentRowIndex, currentColumnIndex, maxRowCount, maxColumnCount))
                            {
                                result.Add(plate);
                                plate = GetPlate(plateSize, out maxRowCount, out maxColumnCount);
                            }

                            plate[currentRowIndex, currentColumnIndex] = well;
                            currentColumnIndex++;

                            //if(currentRowIndex == plate[0])
                        }
                    }
                    currentRowIndex++;
                    currentColumnIndex = 0;
                }
            }


            return result;
        }

        private bool IsPlateFull(int currentRowIndex, int currentColumnIndex, int maxRowCount, int maxColumnCount)
        {
            return currentRowIndex == maxRowCount - 1 && currentColumnIndex == maxColumnCount - 1;
        }

        private Well[,] GetPlate(int plateSize, out int rowCount, out int columnCount)
        {
            switch (plateSize)
            {
                case 96:
                    rowCount = 8;
                    columnCount = 12;
                    break;
                case 384:
                    rowCount = 16;
                    columnCount = 24;
                    break;
                default:
                    throw new Exception("Plate size not supported.");
            }

            return new Well[rowCount, columnCount];
        }
    }
}
