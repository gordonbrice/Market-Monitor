using MVVMSupport;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WhaleAlert
{
    public class WhaleAlertModel : ViewModelBase
    {
        WhaleAlertConnection _conn = null;
        string _apiKey = null;

        Status _status;
        public Status Status
        {
            get
            {
                return _status;
            }

            private set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        string _statusResult;
        public string StatusResult
        {
            get
            {
                return _statusResult;
            }

            set
            {
                _statusResult = value;
                OnPropertyChanged("StatusResult");
            }
        }
        public WhaleAlertModel(HttpClient client, string apiKey)
        {
            _conn = new WhaleAlertConnection(client);
            _apiKey = apiKey;

            var statusAwaiter = GetStatus().GetAwaiter();

            statusAwaiter.OnCompleted(() =>
            {
                StatusResult = Status.Result;
            });
        }

        private async Task GetStatus()
        {
            Status = Status.FromJson(await _conn.ApiGet("/v1/status", _apiKey));
        }
    }
}
