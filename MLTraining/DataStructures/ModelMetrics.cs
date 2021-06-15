using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace MLTraining.DataStructures
{
    public class ModelMetrics
    {
        public MulticlassClassificationMetrics Metrics;
        public string Name;

        public ModelMetrics(MulticlassClassificationMetrics metrics, string name)
        {
            Metrics = metrics;
            Name = name;
        }
    }
}
