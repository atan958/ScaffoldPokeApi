using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PokeApiLibrary.Models.Details;

namespace PokeApiLibrary.Api
{
    public class PokeApiDetailsProcessor : PokeApiProcessor
    {
        private readonly string _pokemonDetailsUrl;

        /*
         *  ...To be removed...
         */
        private int startCount = 0;
        private int endCount = 0;

        public PokeApiDetailsProcessor()
        {
            _pokemonDetailsUrl = "https://pokeapi.co/api/v2/pokemon";
        }

        private HttpClient GetClient()
        {
            HttpClientHandler clientHandler = new();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            var client= new HttpClient(clientHandler);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            return client;
        }

        public async Task<List<PokemonDetailsInfo>> RetrievePokemonDetailsInfoListFromIdListAsync(List<int> pokemonSpeciesIdList)
        {
            var pokemonDetailsInfoTasks = pokemonSpeciesIdList.Select(speciesId =>
            {
                var detailsInfoTask = RetrievePokemonDetailsInfoAsync(speciesId);
                return detailsInfoTask;
            });

            var pokemonDetailsInfoList = (await Task.WhenAll(pokemonDetailsInfoTasks)).ToList();

            return pokemonDetailsInfoList;
        }

        /*
         *  Method: Acquires Details data from PokeApi for the given Pokemon Ids
         */
        public async Task<List<PokemonDetailsInfo>> RetrievePokemonDetailsInfoListFromIdListManualAsync(List<int> pokemonSpeciesIdList)
        {
            Console.WriteLine("Retrieving List of Pokemon Details Information");

            const int numberOfCallsPerRun = 20;

            var totalNumberOfRuns = Math.Ceiling(Convert.ToDecimal(pokemonSpeciesIdList.Count) / numberOfCallsPerRun);

            var runsOutputsList = new List<List<PokemonDetailsInfo>>();

            for (var i = 0; i < totalNumberOfRuns; i++)
            {
                var startIndex = i * numberOfCallsPerRun;
                var endIndex = (((i + 1) * numberOfCallsPerRun) > pokemonSpeciesIdList.Count) ? pokemonSpeciesIdList.Count : ((i + 1) * numberOfCallsPerRun);
                var pokemonDetailsInfoTasks = new List<Task<PokemonDetailsInfo>>();

                for (var j = startIndex; j < endIndex; j++)
                {
                    var detailsInfoTask = RetrievePokemonDetailsInfoAsync(pokemonSpeciesIdList[j]);
                    pokemonDetailsInfoTasks.Add(detailsInfoTask);
                }

                var pokemonDetailsInfoList = (await Task.WhenAll(pokemonDetailsInfoTasks)).ToList();
                runsOutputsList.Add(pokemonDetailsInfoList);
                Console.WriteLine($"Completed Loops {i}");
            }

            var outputDetailsInfoList = runsOutputsList.SelectMany(list => list).ToList();

            return outputDetailsInfoList;
        }

        /*
         *  Helper:
         */
        private async Task<PokemonDetailsInfo> RetrievePokemonDetailsInfoAsync(int speciesId)
        {
            Console.WriteLine("Started " + startCount++);

            using var response = await ApiClient.GetAsync($"{_pokemonDetailsUrl}/{speciesId}");

            var content = await response.Content.ReadAsAsync<PokemonDetailsInfo>();

            Console.WriteLine("Completed " + endCount++);

            return content;
        }
    }
}
