using Newtonsoft.Json;
using qpcr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TestApp
{
    public class ViewModel
    {
        public int PlateSize { get; set; } = 96;
        public string Samples { get; set; } = " [['Sample-1', 'Sample-2', 'Sample-3'], ['Sample-1', 'Sample-2', 'Sample-3']]";
        public string Reagents { get; set; } = " [['<Pink>'], ['<Green>']]";
        public string Replicates { get; set; } = "[3, 2]";


        public void Generate()
        {
            var replicates = JsonConvert.DeserializeObject<List<int>>(Replicates);
            var reagents = JsonConvert.DeserializeObject<List<List<string>>>(Reagents);
            var samples = JsonConvert.DeserializeObject<List<List<string>>>(Samples);

            var generator = new Generator();
            var result = generator.Generate(PlateSize, samples, reagents, replicates);
        }
    }
}
