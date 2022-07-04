
using CommunityToolkit.Mvvm.ComponentModel;

namespace NodeModels2
{
    public partial class PriceModel : ObservableObject, IEquatable<PriceModel>
    {
        [ObservableProperty]
        string name;

        [ObservableProperty]
        Erc20TokenModel token;

        [ObservableProperty]
        string market;

        [ObservableProperty]
        UInt64 open;

        [ObservableProperty]
        UInt64 high;

        [ObservableProperty]
        UInt64 low;

        [ObservableProperty]
        UInt64 close;

        [ObservableProperty]
        DateTime time;

        public bool Equals(PriceModel other)
        {
            return Name == other.Name && Market == other.Market;
        }
    }
}
