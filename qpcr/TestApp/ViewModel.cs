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
        public string Samples { get; set; } = "[['Sam 1', 'Sam 2', 'Sam 3'], ['Sam 1', 'Sam 3', 'Sam 4']]";
        public string Reagents { get; set; } = "[['Reag X', 'Reag Y'], ['Reag Y', 'Reag Z']]";
        public string Replicates { get; set; } = "[1, 3]";


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
