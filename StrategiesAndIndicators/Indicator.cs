using System;
using System.Collections.Generic;
using System.Text;

namespace StrategiesAndIndicators
{
    public partial class Indicator : IndicatorBase
    {
        private SMA[] cacheSMA = null;

        private static SMA checkSMA = new SMA();

        /// <summary>
        /// The SMA (Simple Moving Average) is an indicator that shows the average value of a security's price over a period of time.
        /// </summary>
        /// <returns></returns>
        public SMA SMA(int period)
        {
            return SMA(Input, period);
        }

        /// <summary>
        /// The SMA (Simple Moving Average) is an indicator that shows the average value of a security's price over a period of time.
        /// </summary>
        /// <returns></returns>
        public SMA SMA(Data.IDataSeries input, int period)
        {
            if (cacheSMA != null)
                for (int idx = 0; idx < cacheSMA.Length; idx++)
                    if (cacheSMA[idx].Period == period && cacheSMA[idx].EqualsInput(input))
                        return cacheSMA[idx];

            lock (checkSMA)
            {
                checkSMA.Period = period;
                period = checkSMA.Period;

                if (cacheSMA != null)
                    for (int idx = 0; idx < cacheSMA.Length; idx++)
                        if (cacheSMA[idx].Period == period && cacheSMA[idx].EqualsInput(input))
                            return cacheSMA[idx];

                SMA indicator = new SMA();
                indicator.BarsRequired = BarsRequired;
                indicator.CalculateOnBarClose = CalculateOnBarClose;
#if NT7
                indicator.ForceMaximumBarsLookBack256 = ForceMaximumBarsLookBack256;
                indicator.MaximumBarsLookBack = MaximumBarsLookBack;
#endif
                indicator.Input = input;
                indicator.Period = period;
                Indicators.Add(indicator);
                //indicator.SetUp();

                SMA[] tmp = new SMA[cacheSMA == null ? 1 : cacheSMA.Length + 1];
                if (cacheSMA != null)
                    cacheSMA.CopyTo(tmp, 0);
                tmp[tmp.Length - 1] = indicator;
                cacheSMA = tmp;
                return indicator;
            }
        }
    }
}
