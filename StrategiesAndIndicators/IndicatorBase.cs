using System;
using System.Collections.Generic;
using System.Text;

namespace StrategiesAndIndicators
{
    public class IndicatorBase
    {
        public Data.IDataSeries Input { get; set; }
        public int BarsRequired { get; set; }
        public bool CalculateOnBarClose { get; set; }
        public List<IndicatorBase> Indicators { get; private set; }

        protected bool EqualsInput(Data.IDataSeries input)
        {
            throw new NotImplementedException();
        }

        public void Setup()
        {

        }
    }
}
