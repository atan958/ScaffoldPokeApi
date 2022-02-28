using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HanidexDbLibrary.Utilities;
using PokeApiLibrary.Api;
using PokeApiLibrary.Models.Details;
using PokeApiLibrary.Models.Species;

namespace CallExternalApi
{
    public class PokemonDataTransferHelper
    {
        private PokeApiHelper ApiHelper { get; }
        private HanidexDbHelper DbHelper { get; }

        public PokemonDataTransferHelper()
        {
            ApiHelper = new PokeApiHelper();

            DbHelper = new HanidexDbHelper();
        }

        public async Task TransferPokemonSpeciesDataAsync()
        {
            var pokemonSpeciesInfoList = new List<PokemonSpeciesInfo>();

            try
            {
                pokemonSpeciesInfoList = await ApiHelper.SpeciesProcessor.RetrievePokemonSpeciesInfoListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                pokemonSpeciesInfoList = await ApiHelper.SpeciesProcessor.RetrievePokemonSpeciesInfoListManualAsync();
            }

            var count = 0;
            pokemonSpeciesInfoList.ToList().ForEach(pokemonSpeciesInfo =>
            {
                Console.WriteLine($"Db Insertion #{count++}");
                DbHelper.InsertPokemonInfo(pokemonSpeciesInfo);
            });
        }

        public async Task TransferPokemonMovesDataAsync()
        {
            var pokemonMoveInfoList = await ApiHelper.MovesProcessor.RetrievePokemonMoveInfoListAsync();

            var count = 0;
            pokemonMoveInfoList.ToList().ForEach(moveInfo =>
            {
                Console.WriteLine(count++);

                moveInfo.Type.Id = ApiHelper.TypesProcessor.GetTypeIdByTypeUrl(moveInfo.Type.Url);

                DbHelper.InsertMoveInfo(moveInfo);
            });
        }

        public async Task TransferPokemonTypesDataAsync()
        {
            var pokemonTypesList = await ApiHelper.TypesProcessor.RetrievePokemonTypeInfoListAsync();

            pokemonTypesList.ToList().ForEach(typeInfo =>
            {
                DbHelper.InsertTypeInfo(typeInfo);
            });
        }

        public async Task _TransferPokemonSpeciesPokemonMovesJoinDataAsync()
        {
            var pokemonMoveInfoList = await ApiHelper.MovesProcessor.RetrievePokemonMoveInfoListAsync();

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

        public async Task TransferPokemonDetailsPokemonMovesJoinDataAsync()
        {
            var pokemonIdList = DbHelper.GetPokemonIdList();

            var pokemonDetailsInfoList = new List<PokemonDetailsInfo>();

            try
            {
                pokemonDetailsInfoList = await ApiHelper.DetailsProcessor.RetrievePokemonDetailsInfoListFromIdListAsync(pokemonIdList);
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR in JoinDataAsync: {e.Message}");
                ApiHelper.DetailsProcessor.ResetClient();
                pokemonDetailsInfoList = await ApiHelper.DetailsProcessor.RetrievePokemonDetailsInfoListFromIdListManualAsync(pokemonIdList);
            }

            pokemonDetailsInfoList.ForEach(detailsInfo =>
            {
                Console.WriteLine($"Pokemon: {detailsInfo.Name}, #Moves: {detailsInfo.Moves.Count}");
                detailsInfo.Moves.ForEach(move =>
                {
                    DbHelper.InsertPokemonMoveJoin(detailsInfo, move.Move);
                });
            });
        }

        public async Task _TransferPokemonDetailsDataAsync()
        {
            Console.WriteLine("Starting Transfer of Pokemon Details Data");

            var pokemonIdList = DbHelper.GetPokemonIdList();

            var pokemonDetailsInfoList = await ApiHelper.DetailsProcessor.RetrievePokemonDetailsInfoListFromIdListManualAsync(pokemonIdList);

            pokemonDetailsInfoList.ForEach(detailsInfo =>
            {
                var moves = detailsInfo.Moves.Select(move =>
                {
                    return move.Move.Name;
                });

            });
        }

        /*
         *
         */
        public async Task TransferPokemonDetailsPokemonTypesJoinDataAsync()
        {
            Console.WriteLine("Starting Transfer");

            var pokemonIdList = DbHelper.GetPokemonIdList();

            var pokemonDetailsInfoList = await ApiHelper.DetailsProcessor.RetrievePokemonDetailsInfoListFromIdListManualAsync(pokemonIdList);

            pokemonDetailsInfoList.ForEach(detailsInfo =>
            {
                detailsInfo.Types.ForEach(type =>
                {
                    Console.WriteLine($"Inserting {detailsInfo.Name}, {type.Type.Name}");

                    DbHelper.InsertPokemonTypeJoin(detailsInfo, type.Type);
                });

            });
        }

        public async Task TransferPokemonAbilitiesDataAsync()
        {
            var pokemonAbilityInfoList = await ApiHelper.AbilitiesProcessor.RetrievePokemonAbilityInfoListAsync();

            pokemonAbilityInfoList.ForEach(abilityInfo =>
            {
                Console.WriteLine($"({abilityInfo.Id}) {abilityInfo.Name}");
                DbHelper.InsertAbilityInfo(abilityInfo);
            });
            Console.WriteLine($"\nTotal #Abilities: {pokemonAbilityInfoList.Count}");
        }
    }
}
