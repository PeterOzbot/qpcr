using Newtonsoft.Json;
using qpcr;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestApp
{
    public class ViewModel : INotifyPropertyChanged
    {
        public int PlateSize { get; set; } = 96;
        //public string Samples { get; set; } = " [['Sample-1', 'Sample-2', 'Sample-3'], ['Sample-1', 'Sample-2', 'Sample-3']]";
        public string Samples { get; set; } = " [['Sample-1', 'Sample-2', 'Sample-3'], ['Sample-3']]";
        public string Reagents { get; set; } = " [['<Pink>', '<Red>', '<Green>'], ['<Green>', '<Red>']]";
        //public string Reagents { get; set; } = " [['<Pink>'], ['<Green>']]";
        public string Replicates { get; set; } = "[3, 20]";
        public List<List<List<Well>>> Result { get; set; }
        public Command GenerateCommand { get; set; }

        public ViewModel()
        {
            GenerateCommand = new Command(Generate);
        }


        public void Generate()
        {
            var replicates = JsonConvert.DeserializeObject<List<int>>(Replicates);
            var reagents = JsonConvert.DeserializeObject<List<List<string>>>(Reagents);
            var samples = JsonConvert.DeserializeObject<List<List<string>>>(Samples);

            var generator = new Generator();
            var generatorResult = generator.Generate(PlateSize, samples, reagents, replicates);

            PrepareDataForVisualisation(generatorResult);
        }

        private void PrepareDataForVisualisation(List<Well[,]> generatorResult)
        {
            Result = new List<List<List<Well>>>();
            foreach (var plate in generatorResult)
            {
                int length = plate.GetUpperBound(0) + 1;
                int width = plate.GetUpperBound(1) + 1;
                var currentPlate = new List<List<Well>>();
                for (int i = 0; i < plate.Length / width; i++)
                {
                    var rows = new List<Well>();
                    for (int k = 0; k < width; k++)
                    {
                        rows.Add(plate[i, k]);
                    }
                    currentPlate.Add(rows);
                }
                Result.Add(currentPlate);
            }

            NotifyPropertyChanged("Result");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
