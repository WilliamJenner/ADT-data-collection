using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;

namespace OnsetPredictions.BarkScale
{
    public static class BarkScale
    {
        //public List<CriticalBandModel> scale;

        //public BarkScale()
        //{
        //    JArray o1 = JArray.Parse(File.ReadAllText(Path.Combine("C:\\source\\NAudioTest\\ConsoleApp1\\BarkScale", "barkScale.json")));
        //    scale = o1.ToObject<List<CriticalBandModel>>();
        //}

        public static List<CriticalBandModel> CriticalBands()
        {
            JArray jArray = JArray.Parse(File.ReadAllText(Path.Combine("C:\\source\\ADT\\OnsetPredictions\\BarkScale", "barkScale.json")));
            return jArray.ToObject<List<CriticalBandModel>>();
        }
    }
}
