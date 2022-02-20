using PokeApiLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PokeApiLibrary.Models.Moves;
using PokeApiLibrary.Models.Species;
using PokeApiLibrary.Models.Types;

namespace PokeApiLibrary.Api
{
    public class PokeApiHelper
    {
        const string _pokemonSpeciesUrl = "https://pokeapi.co/api/v2/pokemon-species";
        const string _pokemonMovesUrl = "https://pokeapi.co/api/v2/move";
        const string _pokemonTypesUrl = "https://pokeapi.co/api/v2/type";

        public HttpClient ApiClient { get; set; }

        public PokeApiHelper() 
        {
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /*
         *  Retrieves a list of Pokemon-specific data
         */
        public async Task<List<PokemonSpeciesInfo>> RetrievePokemonSpeciesInfoList()
        {
            var pokemonSpeciesList = await RetrievePokemonSpeciesList();
            
            var pokemonSpeciesInfoTasks = pokemonSpeciesList.Select((pokemonSpecies) =>
            {
                return RetrievePokemonSpeciesInfo(pokemonSpecies);
            });

            var pokemonSpeciesInfoList = await Task.WhenAll(pokemonSpeciesInfoTasks);

            return pokemonSpeciesInfoList.ToList();
        }

        private async Task<List<PokemonSpecies>> RetrievePokemonSpeciesList()
        {
            using var response = await ApiClient.GetAsync(_pokemonSpeciesUrl);

            var content = await response.Content.ReadAsAsync<PokemonSpeciesList>();

            return content.Results;
        }

        private async Task<PokemonSpeciesInfo> RetrievePokemonSpeciesInfo(PokemonSpecies pokemonSpecies)
        {
            using var response = await ApiClient.GetAsync(pokemonSpecies.Url);

            return await response.Content.ReadAsAsync<PokemonSpeciesInfo>();
        }

        /*
         * Another method which does a similar thing
         */
        public async Task<List<PokemonMove>> RetrievePokemonMovesList()
        {
            using var response = await ApiClient.GetAsync(_pokemonMovesUrl);

            var content = await response.Content.ReadAsAsync<PokemonMovesList>();

            return content.Results;
        }

        public async Task<PokemonMoveInfo> RetrievePokemonMoveInfo(PokemonMove pokemonMove)
        {
            using var response = await ApiClient.GetAsync(pokemonMove.Url);

            var content = await response.Content.ReadAsAsync<PokemonMoveInfo>();

            return content;
        }

        /*
         *  Another method which does a similar thing
         */
        public async Task<List<PokemonType>> RetrievePokemonTypesList()
        {
            using var response = await ApiClient.GetAsync(_pokemonTypesUrl);

            var content = await response.Content.ReadAsAsync<PokemonTypesList>();


            return content.Results;
        }
    }
}
