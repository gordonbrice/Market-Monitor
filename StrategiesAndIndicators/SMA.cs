using System;
using System.ComponentModel;

namespace StrategiesAndIndicators
{
    public class SMA : Indicator
    {
        #region Variables
        private int period = 14;
        #endregion

        /// <summary>
        /// This method is used to configure the indicator and is called once before any bar data is loaded.
        /// </summary>
        //protected override void Initialize()
        //{
        //    Add(new Plot(Color.Orange, "SMA"));

        //    Overlay = true;
        //}

        ///// <summary>
        ///// Called on each bar update event (incoming tick).
        ///// </summary>
        //protected override void OnBarUpdate()
        //{
        //    if (CurrentBar == 0)
        //        Value.Set(Input[0]);
        //    else
        //    {
        //        double last = Value[1] * Math.Min(CurrentBar, Period);

        //        if (CurrentBar >= Period)
        //            Value.Set((last + Input[0] - Input[Period]) / Math.Min(CurrentBar, Period));
        //        else
        //            Value.Set((last + Input[0]) / (Math.Min(CurrentBar, Period) + 1));
        //    }
        //}

        #region Properties
        /// <summary>
        /// </summary>
        [Description("Numbers of bars used for calculations")]
        public int Period
        {
            get { return period; }
            set { period = Math.Max(1, value); }
        }
        #endregion
    }
}
