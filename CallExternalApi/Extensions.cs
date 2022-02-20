using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HanidexDbLibrary.Utilities;
using PokeApiLibrary.Api;

namespace CallExternalApi
{
    public static class Extensions
    {
        public static async Task TransferPokemonSpeciesData(this PokeApiHelper pokeApiHelper)
        {
            var hanidexDbHelper = new HanidexDbHelper();

            var pokemonSpeciesInfoList = await pokeApiHelper.RetrievePokemonSpeciesInfoList();

            pokemonSpeciesInfoList.ToList().ForEach(pokemonSpeciesInfo =>
            {
                hanidexDbHelper.InsertPokemonInfo(pokemonSpeciesInfo);
            });
        }

        public static async Task TransferPokemonMovesData(this PokeApiHelper pokeApiHelper)
        {
            var hanidexDbHelper = new HanidexDbHelper();

            var pokemonMovesList = await pokeApiHelper.RetrievePokemonMovesList();

            var pokemonMoveInfoTasks = pokemonMovesList.Select(move =>
            {
                var moveInfoTask = pokeApiHelper.RetrievePokemonMoveInfo(move);
                return moveInfoTask;
            });
            var pokemonMoveInfoList = await Task.WhenAll(pokemonMoveInfoTasks);
            pokemonMoveInfoList.ToList().ForEach(moveInfo =>
            {
                hanidexDbHelper.InsertMoveInfo(moveInfo);
            });
        }

        public static async Task TransferPokemonTypesData(this PokeApiHelper pokeApiHelper)
        {
            var hanidexDbHelper = new HanidexDbHelper();

            var pokemonTypesList = await pokeApiHelper.RetrievePokemonTypesList();

            pokemonTypesList.ToList().ForEach((type) =>
            {
                Console.WriteLine(type.Name);
                hanidexDbHelper.InsertTypeInfo(type);
            });
        }
    }
}
