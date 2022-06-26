using CommunityToolkit.Mvvm.ComponentModel;
using Nethereum.Web3;
using NodeServices;

namespace NodeModels2
{
    public partial class AccountModel : ObservableObject
    {
        [ObservableProperty]
        string name;

        [ObservableProperty]
        string address;

        [ObservableProperty]
        decimal eth;

        [ObservableProperty]
        decimal usd;

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
