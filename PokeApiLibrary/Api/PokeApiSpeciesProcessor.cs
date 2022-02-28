using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PokeApiLibrary.Models.Species;

namespace PokeApiLibrary.Api
{
    public class PokeApiSpeciesProcessor
    {
        private HttpClient ApiClient { get; }

        private readonly string _pokemonSpeciesUrl;

        public PokeApiSpeciesProcessor(HttpClient client)
        {
            ApiClient = client;

            _pokemonSpeciesUrl = "https://pokeapi.co/api/v2/pokemon-species?limit=900";
        }

        /*
         *  Retrieves a list of Pokemon-specific data
         */
        public async Task<List<PokemonSpeciesInfo>> RetrievePokemonSpeciesInfoListAsync()
        {
            var pokemonSpeciesList = await RetrievePokemonSpeciesListAsync();

            var count = 0;
            var pokemonSpeciesInfoTasks = pokemonSpeciesList.Results.Select((pokemonSpecies) =>
            {
                Console.WriteLine($"Api Call #{count++}");
                var speciesInfoTask = RetrievePokemonSpeciesInfoAsync(pokemonSpecies);
                return speciesInfoTask;
            });

            var pokemonSpeciesInfoList = (await Task.WhenAll(pokemonSpeciesInfoTasks)).ToList();

            return pokemonSpeciesInfoList;
        }

        public async Task<List<PokemonSpeciesInfo>> RetrievePokemonSpeciesInfoListManualAsync()
        {
            Console.WriteLine("Retrieving List of Pokemon Details Information");

            const int numberOfCallsPerRun = 20;

            var pokemonSpeciesList = await RetrievePokemonSpeciesListAsync();

            var totalNumberOfCalls = pokemonSpeciesList.Count;

            var totalNumberOfRuns = Math.Ceiling(Convert.ToDecimal(totalNumberOfCalls) / numberOfCallsPerRun);

            var runsOutputsList = new List<List<PokemonSpeciesInfo>>();

            for (var i = 0; i < totalNumberOfRuns; i++)
            {
                var startIndex = i * numberOfCallsPerRun;
                var endIndex = (((i + 1) * numberOfCallsPerRun) > pokemonSpeciesList.Count) ? pokemonSpeciesList.Count : ((i + 1) * numberOfCallsPerRun);
                var pokemonDetailsInfoTasks = new List<Task<PokemonSpeciesInfo>>();

                for (var j = startIndex; j < endIndex; j++)
                {
                    var detailsInfoTask = RetrievePokemonSpeciesInfoAsync(pokemonSpeciesList.Results[j]);
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
         *
         */
        private async Task<PokemonSpeciesList> RetrievePokemonSpeciesListAsync()
        {
            using var response = await ApiClient.GetAsync(_pokemonSpeciesUrl);

            var content = await response.Content.ReadAsAsync<PokemonSpeciesList>();

            return content;
        }

        /*
         *
         */
        private int callCount = 0;
        private async Task<PokemonSpeciesInfo> RetrievePokemonSpeciesInfoAsync(PokemonSpecies pokemonSpecies)
        {
            Console.WriteLine(callCount++);
            using var response = await ApiClient.GetAsync(pokemonSpecies.Url);

            var content = await response.Content.ReadAsAsync<PokemonSpeciesInfo>();

            return content;
        }

    }
}
