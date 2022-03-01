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

        /*
         *
         */
        public async Task TransferPokemonSpeciesDataAsync()
        {
            var pokemonSpeciesInfoList = new List<PokemonSpeciesInfo>();

            try
            {
                pokemonSpeciesInfoList = await ApiHelper.SpeciesProcessor.RetrievePokemonSpeciesInfoListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in PokemonSpecies: {e.Message}");

                pokemonSpeciesInfoList = await ApiHelper.SpeciesProcessor.RetrievePokemonSpeciesInfoListManualAsync();
            }

            pokemonSpeciesInfoList.ToList().ForEach(pokemonSpeciesInfo =>
            {
                DbHelper.InsertPokemonInfo(pokemonSpeciesInfo);
            });

            Console.WriteLine("==================================================================");
            Console.WriteLine($"======================== Pokemon Species ========================");
            Console.WriteLine("==================================================================");
        }

        /*
         *
         */
        public async Task TransferPokemonMovesDataAsync()
        {
            var pokemonMoveInfoList = await ApiHelper.MovesProcessor.RetrievePokemonMoveInfoListAsync();

            pokemonMoveInfoList.ToList().ForEach(moveInfo =>
            {
                moveInfo.Type.Id = ApiHelper.TypesProcessor.GetTypeIdByTypeUrl(moveInfo.Type.Url);

                DbHelper.InsertMoveInfo(moveInfo);
            });

            Console.WriteLine("==================================================================");
            Console.WriteLine($"========================= Pokemon Moves =========================");
            Console.WriteLine("==================================================================");
        }

        /*
         *
         */
        public async Task TransferPokemonTypesDataAsync()
        {
            var pokemonTypesList = await ApiHelper.TypesProcessor.RetrievePokemonTypeInfoListAsync();

            pokemonTypesList.ToList().ForEach(typeInfo =>
            {
                DbHelper.InsertTypeInfo(typeInfo);
            });

            Console.WriteLine("==================================================================");
            Console.WriteLine($"========================= Pokemon Types =========================");
            Console.WriteLine("==================================================================");
        }

        /*
         *
         */
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
                ApiHelper.DetailsProcessor.ResetClient();
                pokemonDetailsInfoList = await ApiHelper.DetailsProcessor.RetrievePokemonDetailsInfoListFromIdListManualAsync(pokemonIdList);
            }

            pokemonDetailsInfoList.ForEach(detailsInfo =>
            {
                detailsInfo.Moves.ForEach(move =>
                {
                    DbHelper.InsertPokemonMoveJoin(detailsInfo, move.Move);
                });
            });

            Console.WriteLine("==================================================================");
            Console.WriteLine($"========================= Details Moves =========================");
            Console.WriteLine("==================================================================");
        }

        /*
         *
         */
        public async Task TransferPokemonDetailsPokemonTypesJoinDataAsync()
        {
            var pokemonIdList = DbHelper.GetPokemonIdList();

            var pokemonDetailsInfoList = await ApiHelper.DetailsProcessor.RetrievePokemonDetailsInfoListFromIdListManualAsync(pokemonIdList);

            pokemonDetailsInfoList.ForEach(detailsInfo =>
            {
                detailsInfo.Types.ForEach(type =>
                {
                    DbHelper.InsertPokemonTypeJoin(detailsInfo, type.Type);
                });

            });

            Console.WriteLine("==================================================================");
            Console.WriteLine($"========================= Details Types =========================");
            Console.WriteLine("==================================================================");
        }

        /*
         *
         */
        public async Task TransferPokemonAbilitiesDataAsync()
        {
            var pokemonAbilityInfoList = await ApiHelper.AbilitiesProcessor.RetrievePokemonAbilityInfoListAsync();

            pokemonAbilityInfoList.ForEach(abilityInfo =>
            {
                DbHelper.InsertAbilityInfo(abilityInfo);
            });

            Console.WriteLine("==================================================================");
            Console.WriteLine($"========================= Pokemon Abilities =========================");
            Console.WriteLine("==================================================================");
        }
    }
}
