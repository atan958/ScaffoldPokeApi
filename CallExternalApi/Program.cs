using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DemoLibrary;
using HanidexDbLibrary.Utilities;
using Newtonsoft.Json;
using PokeApiLibrary.Api;
using PokeApiLibrary.Models;

namespace CallExternalApi
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var pokeApiHelper = new PokeApiHelper();
            var tasks = new List<Task>();
            //tasks.Add(pokeApiHelper.TransferPokemonSpeciesData());
            tasks.Add(pokeApiHelper.TransferPokemonMovesData());
            //tasks.Add(pokeApiHelper.TransferPokemonTypesData());
            await Task.WhenAll(tasks);

            new HanidexDbHelper().GetTypeIdByTypeName("psychic");
        }
    }
}