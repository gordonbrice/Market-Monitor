﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NodeModels
{
    public class PriceModel : ViewModelBase
    {
        string name;
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
                OnPropertyChanged("Name");
            }
        }

        string market;
        public string Market
        {
            get
            {
                return this.market;
            }
            set
            {
                this.market = value;
                OnPropertyChanged("Market");
            }
        }

        UInt64 open;
        public UInt64 Open
        {
            get
            {
                return this.open;
            }
            set
            {
                this.open = value;
                OnPropertyChanged("Open");
            }
        }

        UInt64 high;
        public UInt64 High
        {
            get
            {
                return this.high;
            }
            set
            {
                this.high = value;
                OnPropertyChanged("High");
            }
        }

        UInt64 low;
        public UInt64 Low
        {
            get
            {
                return this.low;
            }
            set
            {
                this.low = value;
                OnPropertyChanged("Low");
            }
        }

        UInt64 close;
        public UInt64 Close
        {
            get
            {
                return this.close;
            }
            set
            {
                this.close = value;
                OnPropertyChanged("Close");
            }
        }

        DateTime time;
        public DateTime Time
        {
            get
            {
                return this.time;
            }
            set
            {
                this.time = value;
                OnPropertyChanged("Time");
            }
        }

    }
}
