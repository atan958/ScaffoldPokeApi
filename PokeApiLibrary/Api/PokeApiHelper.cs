using PokeApiLibrary.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PokeApiLibrary.Models.Abilities;
using PokeApiLibrary.Models.Details;
using PokeApiLibrary.Models.Moves;
using PokeApiLibrary.Models.Species;
using PokeApiLibrary.Models.Types;

namespace PokeApiLibrary.Api
{
    public class PokeApiHelper
    {
        public HttpClient ApiClient { get; set; }

        public PokeApiSpeciesProcessor SpeciesProcessor { get; set; }
        public PokeApiMovesProcessor MovesProcessor { get; set; }
        public PokeApiTypesProcessor TypesProcessor { get; set; }
        public PokeApiDetailsProcessor DetailsProcessor { get; set; }
        public PokeApiAbilitiesProcessor AbilitiesProcessor { get; set; }

        public PokeApiHelper() 
        {
            HttpClientHandler clientHandler = new();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            ApiClient = new HttpClient(clientHandler);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            SpeciesProcessor = new PokeApiSpeciesProcessor(ApiClient);
            MovesProcessor = new PokeApiMovesProcessor(ApiClient);
            TypesProcessor = new PokeApiTypesProcessor(ApiClient);
            DetailsProcessor = new PokeApiDetailsProcessor(ApiClient);
            AbilitiesProcessor = new PokeApiAbilitiesProcessor(ApiClient);
        }
    }
}

//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
