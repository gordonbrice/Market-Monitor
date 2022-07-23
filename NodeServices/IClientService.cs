using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NodeServices
{
    public interface IClientService
    {
        string Name { get; }

        Task<string> GetClientVersion();
    }
}
