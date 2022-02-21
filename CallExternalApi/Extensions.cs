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

            var pokemonMoveInfoList = await pokeApiHelper.RetrievePokemonMoveInfoList();

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
                hanidexDbHelper.InsertTypeInfo(type);
            });
        }

        public static async Task _TransferPokemonSpeciesPokemonMovesJoinData(this PokeApiHelper pokeApiHelper)
        {
            var hanidexDbHelper = new HanidexDbHelper();

            var pokemonMoveInfoList = await pokeApiHelper.RetrievePokemonMoveInfoList();

            pokemonMoveInfoList.ForEach(move =>
            {
                Console.WriteLine("========================================");
                Console.WriteLine($"============={move.Name}=============");
                Console.WriteLine("========================================");
                move.Learned_By_Pokemon.ForEach(pokemon =>
                {
                    //hanidexDbHelper.InsertPokemonMoveJoin(pokemon, move);
                });
            });
        }
        public static async Task TransferPokemonDetailsPokemonMovesJoinData(this PokeApiHelper pokeApiHelper)
        {
            var hanidexDbHelper = new HanidexDbHelper();

            var pokemonIdList = hanidexDbHelper.GetPokemonIdList();

            var pokemonDetailsInfoList = await pokeApiHelper.RetrievePokemonDetailsInfoListSelected(pokemonIdList);

            pokemonDetailsInfoList.ForEach(detailsInfo =>
            {
                Console.WriteLine($"Pokemon: {detailsInfo.Name}, #Moves: {detailsInfo.Moves.Count()}");
                detailsInfo.Moves.ForEach(move =>
                {
                    hanidexDbHelper.InsertPokemonMoveJoin(detailsInfo, move.Move );
                });
                //Console.WriteLine();
                //Console.WriteLine(detailsInfo.Name);
                //Console.WriteLine($"\t{String.Join(", ", moves)}");
                //Console.WriteLine();
            });
        }

        public static async Task TransferPokemonDetailsData(this PokeApiHelper pokeApiHelper)
        {
            Console.WriteLine("Starting Transfer of Pokemon Details Data");

            var hanidexDbHelper = new HanidexDbHelper();

            var pokemonIdList = hanidexDbHelper.GetPokemonIdList();

            var pokemonDetailsInfoList = await pokeApiHelper.RetrievePokemonDetailsInfoListSelected(pokemonIdList);

            pokemonDetailsInfoList.ForEach(detailsInfo =>
            {
                var moves = detailsInfo.Moves.Select(move =>
                {
                    return move.Move.Name;
                });
                Console.WriteLine();
                Console.WriteLine(detailsInfo.Name);
                Console.WriteLine($"\t{String.Join(", ", moves)}");
                Console.WriteLine();
            });
        }


    }
}
