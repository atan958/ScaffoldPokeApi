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
        public PokeApiSpeciesProcessor SpeciesProcessor { get; set; }
        public PokeApiMovesProcessor MovesProcessor { get; set; }
        public PokeApiTypesProcessor TypesProcessor { get; set; }
        public PokeApiDetailsProcessor DetailsProcessor { get; set; }
        public PokeApiAbilitiesProcessor AbilitiesProcessor { get; set; }

        public PokeApiHelper() 
        {
            SpeciesProcessor = new PokeApiSpeciesProcessor();
            MovesProcessor = new PokeApiMovesProcessor();
            TypesProcessor = new PokeApiTypesProcessor();
            DetailsProcessor = new PokeApiDetailsProcessor();
            AbilitiesProcessor = new PokeApiAbilitiesProcessor();
        }
    }
}

//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
