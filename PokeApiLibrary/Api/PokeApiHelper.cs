using PokeApiLibrary.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
                var speciesInfoTask = RetrievePokemonSpeciesInfo(pokemonSpecies);
                return speciesInfoTask;
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

            var content = await response.Content.ReadAsAsync<PokemonSpeciesInfo>();

            return content;
        }


        /*
         * Another method which does a similar thing
         */
        public async Task<List<PokemonMoveInfo>> RetrievePokemonMoveInfoList()
        {
            var pokemonMovesList = await RetrievePokemonMovesList();

            var pokemonMoveInfoTasks = pokemonMovesList.Select(move =>
            {
                var moveInfoTask = RetrievePokemonMoveInfo(move);
                return moveInfoTask;
            });

            var pokemonMoveInfoList = await Task.WhenAll(pokemonMoveInfoTasks);

            pokemonMoveInfoList.ToList().ForEach(move =>
            {
                Console.WriteLine(GetTypeIdByTypeUrl(move.Type.Url));
            });
           
            return pokemonMoveInfoList.ToList();
        }

        private async Task<List<PokemonMove>> RetrievePokemonMovesList()
        {
            using var response = await ApiClient.GetAsync(_pokemonMovesUrl);

            var content = await response.Content.ReadAsAsync<PokemonMovesList>();

            return content.Results;
        }

        private async Task<PokemonMoveInfo> RetrievePokemonMoveInfo(PokemonMove pokemonMove)
        {
            using var response = await ApiClient.GetAsync(pokemonMove.Url);

            var content = await response.Content.ReadAsAsync<PokemonMoveInfo>();

            return content;
        }

        private int? GetTypeIdByTypeUrl(string typeUrl)
        {
            var startIndex = _pokemonTypesUrl.Length + 1;
            var endIndex = (typeUrl.Length - 1) - startIndex;
            var tryTypeId = typeUrl.Substring(startIndex, endIndex);
            var isValidTypeId = Int32.TryParse(tryTypeId, out int typeId);

            return (isValidTypeId) ? typeId : null;
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
