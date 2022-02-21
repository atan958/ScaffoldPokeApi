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
using PokeApiLibrary.Models.Details;
using PokeApiLibrary.Models.Moves;
using PokeApiLibrary.Models.Species;
using PokeApiLibrary.Models.Types;

namespace PokeApiLibrary.Api
{
    public class PokeApiHelper
    {
        const string _pokemonSpeciesUrl = "https://pokeapi.co/api/v2/pokemon-species?limit=900";
        const string _pokemonMovesUrl = "https://pokeapi.co/api/v2/move?limit=900";
        const string _pokemonTypesUrl = "https://pokeapi.co/api/v2/type";
        const string _pokemonDetailsUrl = "https://pokeapi.co/api/v2/pokemon";

        public HttpClient ApiClient { get; set; }

        public PokeApiHelper() 
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            ApiClient = new HttpClient(clientHandler);
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
                move.Type.Id = GetTypeIdByTypeUrl(move.Type.Url);
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
            var isValidTypeId = int.TryParse(tryTypeId, out var typeId);

            return (isValidTypeId) ? typeId : null;
        }

        /*
         *  Another method which does a similar thing
         */
        public async Task<List<PokemonType>> RetrievePokemonTypesList()
        {
            using var response = await ApiClient.GetAsync(_pokemonTypesUrl);

            var content = await response.Content.ReadAsAsync<PokemonTypesList>();

            content.Results.ForEach(type =>
            {
                type.Id = GetTypeIdByTypeUrl(type.Url);
            });

            return content.Results;
        }

        /*
         *  Selected Pokemon Ids
         */
        public async Task<List<PokemonDetailsInfo>> RetrievePokemonDetailsInfoListSelected(List<int> pokemonSpeciesIdList)
        {
            Console.WriteLine("Retrieving List of Pokemon Details Information");

            var listList = new List<List<PokemonDetailsInfo>>();
            for (var i = 0; i < 60; i++)
            {
                var startIndex = i * 20;
                var endIndex = (((i + 1) * 20) > pokemonSpeciesIdList.Count()) ? pokemonSpeciesIdList.Count() : ((i + 1) * 20);
                var pokemonDetailsInfoTasks = new List<Task<PokemonDetailsInfo>>();

                for (var j = startIndex; j < endIndex; j++)
                {
                    var detailsInfoTask = RetrievePokemonDetailsInfo(pokemonSpeciesIdList[j]);
                    pokemonDetailsInfoTasks.Add(detailsInfoTask);
                }

                var pokemonDetailsInfoList = (await Task.WhenAll(pokemonDetailsInfoTasks)).ToList();
                listList.Add(pokemonDetailsInfoList);
                Console.WriteLine($"Completed Loops {i}");
            }

            var output = listList.SelectMany(list => list).ToList();

            return output;
        }

        private async Task<PokemonDetailsInfo> RetrievePokemonDetailsInfo(int speciesId)
        {
            Console.WriteLine("Started " + startCount++);

            using var response = await ApiClient.GetAsync($"{_pokemonDetailsUrl}/{speciesId}");

            var content = await response.Content.ReadAsAsync<PokemonDetailsInfo>();
            Console.WriteLine("Completed " + endCount++);
            return content;
        }


        /*
         *
         */
        public async Task<List<PokemonDetailsInfo>> _RetrievePokemonDetailsInfoList()
        {
            var pokemonDetailsList = await RetrievePokemonDetailsList();

            var pokemonDetailsInfoTasks = pokemonDetailsList.Select(pokemonDetails =>
            {
                var detailsInfoTask = RetrievePokemonDetailsInfo(pokemonDetails);
                return detailsInfoTask;
            }).ToList();

            var pokemonDetailsInfoList = new List<PokemonDetailsInfo>();

            int y = 1;
            for (int i = 0; i < pokemonDetailsInfoTasks.Count(); i++)
            {
                var detailsInfo = await pokemonDetailsInfoTasks[i];
                Console.WriteLine($"i is {i} and y * 100 is {y * 100}");
                if (i == (y * 100))
                {
                    Console.WriteLine("WAITING"); 
                    await Task.Delay(5000); y++; 
                    Console.WriteLine("DONE WAITING");
                }
                
                pokemonDetailsInfoList.Add(detailsInfo);
            }

            return pokemonDetailsInfoList;
        }

        private async Task<List<PokemonDetails>> RetrievePokemonDetailsList()
        {
            using var response = await ApiClient.GetAsync(_pokemonDetailsUrl);

            var content = await response.Content.ReadAsAsync<PokemonDetailsList>();

            return content.Results;
        }

        private int startCount = 0;
        private int endCount = 0;
        private int x = 1;

        private async Task<PokemonDetailsInfo> RetrievePokemonDetailsInfo(PokemonDetails pokemonDetails)
        {
            //if (startCount>(x*200))
            //{
            //    while (endCount< (x * 200 -10))
            //    {
            //        Task.Delay(5000);
            //    }

            //    x++;
            //}

            Console.WriteLine($"Started {startCount++}");
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls13;
            using var response = await ApiClient.GetAsync(pokemonDetails.Url);

            var content = await response.Content.ReadAsAsync<PokemonDetailsInfo>();
            Console.WriteLine($"Completed {endCount++}");

            return content;
        }
    }
}

//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

