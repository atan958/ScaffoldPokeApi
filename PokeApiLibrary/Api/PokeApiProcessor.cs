using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PokeApiLibrary.Api
{
    public abstract class PokeApiProcessor
    {
        protected HttpClient ApiClient { get; set; }

        protected PokeApiProcessor()
        {
            ResetClient();
        }

        public void ResetClient()
        {
            HttpClientHandler clientHandler = new();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            ApiClient = new HttpClient(clientHandler);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
