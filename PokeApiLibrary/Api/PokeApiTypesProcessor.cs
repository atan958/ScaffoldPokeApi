using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PokeApiLibrary.Models.Types;

namespace PokeApiLibrary.Api
{
    public class PokeApiTypesProcessor
    {
        private HttpClient ApiClient { get; }

        private readonly string _pokemonTypesUrl;

        public PokeApiTypesProcessor(HttpClient client)
        {
            ApiClient = client;

            _pokemonTypesUrl = "https://pokeapi.co/api/v2/type";
        }

        /*
         *  Method: Acquires the Pokemon Types from the API
         */
        public async Task<List<PokemonTypeInfo>> RetrievePokemonTypeInfoListAsync()
        {
            var pokemonTypeList = await RetrievePokemonTypeListAsync();

            var pokemonTypeInfoTasks = pokemonTypeList.Results.Select(pokemonType =>
            {
                var typeInfoTask = RetrievePokemonTypeInfoAsync(pokemonType);
                return typeInfoTask;
            });

            var pokemonTypeInfoList = (await Task.WhenAll(pokemonTypeInfoTasks)).ToList();

            return pokemonTypeInfoList;
        }

        /*
         *  Helper: 
         */
        private async Task<PokemonTypesList> RetrievePokemonTypeListAsync()
        {
            using var response = await ApiClient.GetAsync(_pokemonTypesUrl);

            var content = await response.Content.ReadAsAsync<PokemonTypesList>();

            return content;
        }

        /*
         *  Helper: 
         */
        private async Task<PokemonTypeInfo> RetrievePokemonTypeInfoAsync(PokemonType pokemonType)
        {
            using var response = await ApiClient.GetAsync(pokemonType.Url);

            var content = await response.Content.ReadAsAsync<PokemonTypeInfo>();

            return content;
        }

        /*
         *  Method:
         */
        public int? GetTypeIdByTypeUrl(string typeUrl)
        {
            var startIndex = _pokemonTypesUrl.Length + 1;
            var endIndex = (typeUrl.Length - 1) - startIndex;
            var tryTypeId = typeUrl.Substring(startIndex, endIndex);
            var isValidTypeId = int.TryParse(tryTypeId, out var typeId);

            return (isValidTypeId) ? typeId : null;
        }
    }
}
