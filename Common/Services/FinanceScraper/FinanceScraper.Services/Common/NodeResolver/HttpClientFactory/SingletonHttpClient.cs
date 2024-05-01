using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceScraper.Common.NodeResolver.HttpClientFactory
{
    public class SingletonHttpClient : IDisposable
    {
        private static readonly HttpClient Instance = new HttpClient();
        private static bool _disposed = false;

        private SingletonHttpClient() { }

        public static HttpClient GetInstance()
        {
            return Instance;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }

        ~SingletonHttpClient()
        {
            Dispose(false);
        }
    }
}
