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

            // generate all wells
            var allWells = GenerateAllWells(experimentsCount, samples, reagents, replicates);

            // order them by sample/reagent
            var orderedWells = allWells.OrderBy(w => w.Sample).ThenBy(w => w.Reagent);

            // generate plates
            var plates = GeneratePlates(orderedWells, plateSize);

            return plates;
        }

        private List<Well[,]> GeneratePlates(IEnumerable<Well> orderedWells, int plateSize)
        {
            var result = new List<Well[,]>();
            int currentRowIndex = 0;
            int currentColumnIndex = -1;
            int maxRowCount;
            int maxColumnCount;

            Well[,] plate = GetPlate(plateSize, out maxRowCount, out maxColumnCount);
            result.Add(plate);

            foreach (Well well in orderedWells)
            {
                if (currentColumnIndex < maxColumnCount - 1)
                {
                    currentColumnIndex++;
                }
                else
                {
                    currentColumnIndex = 0;
                    if (currentRowIndex < maxRowCount - 1)
                    {
                        currentRowIndex++;
                    }
                    else
                    {
                        plate = GetPlate(plateSize, out maxRowCount, out maxColumnCount);
                        result.Add(plate);
                        currentColumnIndex = 0;
                        currentRowIndex = 0;
                    }
                }

                plate[currentRowIndex, currentColumnIndex] = well;
            }

            return result;
        }

        private IEnumerable<Well> GenerateAllWells(int experimentsCount, List<List<string>> samples, List<List<string>> reagents, List<int> replicates)
        {
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
                            yield return well;
                        }
                    }
                }
            }
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
