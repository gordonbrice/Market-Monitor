using System;
using System.Threading.Tasks;

namespace RESTApi
{
    public interface IConnection
    {
        Task<string> ApiGet(string function, string key);
    }
}
