using MVVMSupport;
using Nethereum.Web3;
using NodeServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeModels
{
    public class AccountModel : ViewModelBase
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
        string address;
        public string Address
        {
            get
            {
                return this.address;
            }
            set
            {
                this.address = value;
                OnPropertyChanged("Address");
            }
        }

        decimal eth;
        public decimal Eth
        {
            get
            {
                return this.eth;
            }
            set
            {
                this.eth = value;
                OnPropertyChanged("Eth");
            }
        }

        decimal usd;
        public decimal Usd
        {
            get
            {
                return this.usd;
            }
            set
            {
                this.usd = value;
                OnPropertyChanged("Usd");
            }
        }

        INodeService node;
        public AccountModel(INodeService node, string name, string address)
        {
            this.node = node;
            Name = name;
            Address = address;
        }

        public void GetBalance()
        {
            var balanceAwaiter = this.node.GetBalance(Address).GetAwaiter();

            balanceAwaiter.OnCompleted(() =>
            {
                Eth = Web3.Convert.FromWei(balanceAwaiter.GetResult());
            });

        }
    }
}
