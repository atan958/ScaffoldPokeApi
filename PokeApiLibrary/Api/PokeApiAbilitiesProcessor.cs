using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PokeApiLibrary.Models.Abilities;

namespace PokeApiLibrary.Api
{
    public class PokeApiAbilitiesProcessor : PokeApiProcessor
    {
        private readonly string _pokemonAbilitiesUrl;

        public PokeApiAbilitiesProcessor()
        {
            _pokemonAbilitiesUrl = "https://pokeapi.co/api/v2/ability";
        }

        /*
         *  Method:
         */
        public async Task<List<PokemonAbilityInfo>> RetrievePokemonAbilityInfoListAsync()
        {
            var pokemonAbilityList = await RetrievePokemonAbilityListAsync();

            var pokemonAbilityInfoTasks = pokemonAbilityList.Results.Select(ability =>
            {
                var pokemonAbilityInfo = RetrievePokemonAbilityInfoAsync(ability);
                return pokemonAbilityInfo;
            }).ToList();

            var pokemonAbilityInfoList = (await Task.WhenAll(pokemonAbilityInfoTasks)).ToList();

            return pokemonAbilityInfoList;
        }

        /*
         *  Helper: 
         */
        private async Task<PokemonAbilitiesList> RetrievePokemonAbilityListAsync()
        {
            using var response = await ApiClient.GetAsync($"{_pokemonAbilitiesUrl}?limit=900");

            var content = await response.Content.ReadAsAsync<PokemonAbilitiesList>();

            return content;
        }

        /*
         *  Helper: 
         */
        private async Task<PokemonAbilityInfo> RetrievePokemonAbilityInfoAsync(PokemonAbility pokemonAbility)
        {
            using var response = await ApiClient.GetAsync(pokemonAbility.Url);

            var content = await response.Content.ReadAsAsync<PokemonAbilityInfo>();

            return content;
        }

        /*
         *
         */
        private int? GetAbilityIdByIdUrl(string abilityUrl)
        {
            var startIndex = _pokemonAbilitiesUrl.Length + 1;
            var endIndex = (abilityUrl.Length - 1) - startIndex;
            var tryAbilityId = abilityUrl.Substring(startIndex, endIndex);
            var isValidAbilityId = int.TryParse(tryAbilityId, out var abilityId);

            return (isValidAbilityId) ? abilityId : null;
        }
    }
}
