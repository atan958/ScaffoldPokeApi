using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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

            // First Wave
            //await pokeApiHelper.TransferPokemonSpeciesData();
            //await pokeApiHelper.TransferPokemonTypesData();
            //await pokeApiHelper.TransferPokemonMovesData();
            await pokeApiHelper.TransferPokemonAbilitiesData();

            // Second Wave
            //await pokeApiHelper.TransferPokemonDetailsPokemonMovesJoinData();
            //await pokeApiHelper.TransferPokemonDetailsPokemonTypesJoinData();
        }
    }
}