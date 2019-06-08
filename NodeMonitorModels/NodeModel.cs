using System;
using Nethereum.Hex;

namespace NodeMonitorModels
{
    public class NodeModel : ViewModelBase
    {
        string status;
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        string protocolVersion;
        public string ProtocolVersion
        {
            get { return protocolVersion; }
            set
            {
                protocolVersion = value;
                OnPropertyChanged("ProtocolVersion");
            }
        }

        HexBigInteger highestBlock;
        public HexBigInteger HighestBlock
        {
            get { return highestBlock; }
            set
            {
                highestBlock = value;
                OnPropertyChanged("HighestBlock");
            }
        }
    }
}
