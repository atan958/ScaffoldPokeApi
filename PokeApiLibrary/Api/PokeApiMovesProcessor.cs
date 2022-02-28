using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PokeApiLibrary.Models.Moves;

namespace PokeApiLibrary.Api
{
    public class PokeApiMovesProcessor : PokeApiProcessor
    {
        private readonly string _pokemonMovesUrl;

        public PokeApiMovesProcessor(HttpClient client)
        {
            _pokemonMovesUrl = "https://pokeapi.co/api/v2/move?limit=900";
        }

        /*
         * Method: Another method which does a similar thing
         */
        public async Task<List<PokemonMoveInfo>> RetrievePokemonMoveInfoListAsync()
        {
            var pokemonMovesList = await RetrievePokemonMovesListAsync();

            var otherCount = 0;
            var pokemonMoveInfoTasks = pokemonMovesList.Results.Select(move =>
            {
                Console.WriteLine($"Pokemon Move Call ${otherCount++}");
                var moveInfoTask = RetrievePokemonMoveInfoAsync(move);
                return moveInfoTask;
            });

            var pokemonMoveInfoList = await Task.WhenAll(pokemonMoveInfoTasks);

            return pokemonMoveInfoList.ToList();
        }

        /*
         *
         */
        private async Task<PokemonMovesList> RetrievePokemonMovesListAsync()
        {
            using var response = await ApiClient.GetAsync(_pokemonMovesUrl);

            var content = await response.Content.ReadAsAsync<PokemonMovesList>();

            return content;
        }

        /*
         *  Helper: 
         */
        private async Task<PokemonMoveInfo> RetrievePokemonMoveInfoAsync(PokemonMove pokemonMove)
        {
            using var response = await ApiClient.GetAsync(pokemonMove.Url);

            var content = await response.Content.ReadAsAsync<PokemonMoveInfo>();

            return content;
        }
    }
}
