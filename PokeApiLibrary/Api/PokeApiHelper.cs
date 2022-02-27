﻿using PokeApiLibrary.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using PokeApiLibrary.Models.Abilities;
using PokeApiLibrary.Models.Details;
using PokeApiLibrary.Models.Moves;
using PokeApiLibrary.Models.Species;
using PokeApiLibrary.Models.Types;

namespace PokeApiLibrary.Api
{
    public class PokeApiHelper
    {
        private readonly string _pokemonSpeciesUrl;
        private readonly string _pokemonMovesUrl;
        private readonly string _pokemonTypesUrl;
        private readonly string _pokemonDetailsUrl;
        private readonly string _pokemonAbilitiesUrl;

        public HttpClient ApiClient { get; set; }

        public PokeApiHelper() 
        {
            HttpClientHandler clientHandler = new();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            ApiClient = new HttpClient(clientHandler);
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _pokemonSpeciesUrl = "https://pokeapi.co/api/v2/pokemon-species?limit=900";
            _pokemonMovesUrl = "https://pokeapi.co/api/v2/move?limit=900";
            _pokemonTypesUrl = "https://pokeapi.co/api/v2/type";
            _pokemonDetailsUrl = "https://pokeapi.co/api/v2/pokemon";
            _pokemonAbilitiesUrl = "https://pokeapi.co/api/v2/ability";
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
         * Method: Another method which does a similar thing
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

        /*
         *  Helper: Acquires the Id of a Pokemon Type from its Url property
         */
        private int? GetTypeIdByTypeUrl(string typeUrl)
        {
            var startIndex = _pokemonTypesUrl.Length + 1;
            var endIndex = (typeUrl.Length - 1) - startIndex;
            var tryTypeId = typeUrl.Substring(startIndex, endIndex);
            var isValidTypeId = int.TryParse(tryTypeId, out var typeId);

            return (isValidTypeId) ? typeId : null;
        }

        private async Task<List<PokemonMove>> RetrievePokemonMovesList()
        {
            using var response = await ApiClient.GetAsync(_pokemonMovesUrl);

            var content = await response.Content.ReadAsAsync<PokemonMovesList>();

            return content.Results;
        }

        /*
         *  Helper: 
         */
        private async Task<PokemonMoveInfo> RetrievePokemonMoveInfo(PokemonMove pokemonMove)
        {
            using var response = await ApiClient.GetAsync(pokemonMove.Url);

            var content = await response.Content.ReadAsAsync<PokemonMoveInfo>();

            return content;
        }

        /*
         *  Method: Acquires the Pokemon Types from the API
         */
        public async Task<List<PokemonTypeInfo>> RetrievePokemonTypeInfoList()
        {
            var pokemonTypeList = await RetrievePokemonTypeList();

            var pokemonTypeInfoTasks = pokemonTypeList.Select(pokemonType =>
            {
                var typeInfoTask = RetrievePokemonTypeInfo(pokemonType);
                return typeInfoTask;
            });

            var pokemonTypeInfoList = (await Task.WhenAll(pokemonTypeInfoTasks)).ToList();

            return pokemonTypeInfoList;
        }

        private async Task<List<PokemonType>> RetrievePokemonTypeList()
        {
            using var response = await ApiClient.GetAsync(_pokemonTypesUrl);

            var content = await response.Content.ReadAsAsync<PokemonTypesList>();

            return content.Results;
        }

        public async Task<PokemonTypeInfo> RetrievePokemonTypeInfo(PokemonType pokemonType)
        {
            using var response = await ApiClient.GetAsync(pokemonType.Url);

            var content = await response.Content.ReadAsAsync<PokemonTypeInfo>();

            return content;
        }

        /*
         *  Method: Acquires Details data from PokeApi for the given Pokemon Ids
         */
        public async Task<List<PokemonDetailsInfo>> RetrievePokemonDetailsInfoListSelected(List<int> pokemonSpeciesIdList)
        {
            Console.WriteLine("Retrieving List of Pokemon Details Information");

            const int numberOfCallsPerRun = 20;

            var totalNumberOfRuns = Math.Ceiling(Convert.ToDecimal(pokemonSpeciesIdList.Count)/numberOfCallsPerRun);
            
            var runsOutputsList = new List<List<PokemonDetailsInfo>>();

            for (var i = 0; i < totalNumberOfRuns; i++)
            {
                var startIndex = i * numberOfCallsPerRun;
                var endIndex = (((i + 1) * numberOfCallsPerRun) > pokemonSpeciesIdList.Count) ? pokemonSpeciesIdList.Count : ((i + 1) * numberOfCallsPerRun);
                var pokemonDetailsInfoTasks = new List<Task<PokemonDetailsInfo>>();

                for (var j = startIndex; j < endIndex; j++)
                {
                    var detailsInfoTask = RetrievePokemonDetailsInfo(pokemonSpeciesIdList[j]);
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
        private int startCount = 0;
        private int endCount = 0;

        /*
         *
         */
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
        public async Task<List<PokemonAbilityInfo>> RetrievePokemonAbilityInfoList()
        {
            var pokemonAbilityList = await RetrievePokemonAbilityList();

            var pokemonAbilityInfoTasks = pokemonAbilityList.Select(ability =>
            {
                var pokemonAbilityInfo = RetrievePokemonAbilityInfo(ability);
                return pokemonAbilityInfo;
            }).ToList();

            var pokemonAbilityInfoList = (await Task.WhenAll(pokemonAbilityInfoTasks)).ToList();

            return pokemonAbilityInfoList;
        }

        public async Task<List<PokemonAbility>> RetrievePokemonAbilityList()
        {
            using var response = await ApiClient.GetAsync($"{_pokemonAbilitiesUrl}?limit=900");

            var content = await response.Content.ReadAsAsync<PokemonAbilitiesList>();

            return content.Results;
        }

        public async Task<PokemonAbilityInfo> RetrievePokemonAbilityInfo(PokemonAbility pokemonAbility)
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

//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
