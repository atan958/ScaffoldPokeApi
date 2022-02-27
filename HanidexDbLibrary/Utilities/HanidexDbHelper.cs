using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeApiLibrary.Models;
using PokeApiLibrary.Models.Abilities;
using PokeApiLibrary.Models.Details;
using PokeApiLibrary.Models.Moves;
using PokeApiLibrary.Models.Species;
using PokeApiLibrary.Models.Types;

namespace HanidexDbLibrary.Utilities
{
    public class HanidexDbHelper
    {
        private readonly string _connectionString;

        public HanidexDbHelper()
        {
            //            _connectionString = @"Data Source=ANGELO\SQLEXPRESS;Initial Catalog=TrialHanidex;Integrated Security=True";
            _connectionString = @"Data Source=ANGELO\SQLEXPRESS;Initial Catalog=TryPokemonTypeAbility;Integrated Security=True";
        }

        /*
         *  Method:
         */
        public void InsertPokemonInfo(PokemonSpeciesInfo pokemonSpeciesInfo)
        {
            var generation = GetGenerationNumber(pokemonSpeciesInfo.Generation.Name);

            var queryString = "INSERT INTO Pokemon (Id, Name, Generation)\n" +
                              $"VALUES ({pokemonSpeciesInfo.Id}, N\'{ pokemonSpeciesInfo.Name }', { generation })";
            try
            {
                using SqlConnection con = new(_connectionString);
                SqlCommand cmd = new(queryString, con);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"InserPokemonInfo: {ex.Message}");
            }
        }

        /*
         *  Helper: 
         */
        private static int GetGenerationNumber(string pokemonGen)
        {
            return pokemonGen switch
            {
                "generation-i" => 1,
                "generation-ii" => 2,
                "generation-iii" => 3,
                "generation-iv" => 4,
                "generation-v" => 5,
                "generation-vi" => 6,
                "generation-vii" => 7,
                "generation-viii" => 8,
                _ => 0,
            };
        }

        /*
         *  Method:
         */
        public void InsertMoveInfo(PokemonMoveInfo moveInfo)
        {
            var accuracy = (moveInfo.Accuracy is null) ? "NULL" : moveInfo.Accuracy.ToString();
            var power = (moveInfo.Power is null) ? "NULL" : moveInfo.Power.ToString();
            var pp = (moveInfo.Pp is null) ? "NULL" : moveInfo.Pp.ToString();

            var queryString = "INSERT INTO Moves (Id, Type_Id, Name, Accuracy, Power, PP)\n" +
                              $"VALUES ({ moveInfo.Id }, {moveInfo.Type.Id}, N\'{ moveInfo.Name }\', { accuracy}, { power }, { pp })";
            try
            {
                using SqlConnection con = new(_connectionString);
                SqlCommand cmd = new(queryString, con);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ moveInfo.Name } in InsertMoveInfo: { ex.Message }");
            }
        }

        /*
         *  Method:
         */
        public void InsertTypeInfo(PokemonTypeInfo typeInfo)
        {
            var queryString = "INSERT INTO Types (Id, Name)\n" +
                              $"VALUES ({typeInfo.Id}, N\'{ typeInfo.Name }\')";
            try
            {
                using SqlConnection con = new(_connectionString);
                SqlCommand cmd = new(queryString, con);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /*
         *  Method: Insert an entry to the PokemonMoves Table
         */
        public void InsertPokemonMoveJoin(PokemonDetailsInfo detailsInfo, MoveMove move)
        {
            var moveId = GetMoveIdByMoveName(move.Name);

            var queryString = "INSERT INTO PokemonMoves (Pokemon_Id, Move_Id)\n" +
                              $"VALUES ({detailsInfo.Id}, {moveId})";
            try
            {
                using SqlConnection con = new(_connectionString);
                SqlCommand cmd = new(queryString, con);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error for PokemonDetailsInfo={detailsInfo.Name} & MoveMove={move.Name} in InsertPokemonMoveJoin: {ex.Message}");
            }
        }

        /*
         *  Helper: 
         */
        private int? GetMoveIdByMoveName(string moveName)
        {
            int? moveId = null;

            var queryString = $"SELECT Id FROM Moves WHERE Name = \'{moveName}\'";
            
            try
            {
                using SqlConnection con = new(_connectionString);
                SqlCommand cmd = new(queryString, con);

                con.Open();
                var rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    moveId = Convert.ToInt32(rdr[0]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error for MoveName={moveName} in GetMoveIdByMoveName: {ex.Message}");
            }

            return moveId;
        }

        /*
         *  Method: Acquires the Ids of all the Pokemon species stored in the Database
         */
        public List<int> GetPokemonIdList()
        {
            Console.WriteLine("Getting List of Pokemon Ids");

            var pokemonIdList = new List<int>();

            var queryString = "SELECT Id FROM Pokemon";

            try
            {
                using SqlConnection con = new(_connectionString);
                SqlCommand cmd = new(queryString, con);

                con.Open();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var pokemonId = Convert.ToInt32(rdr[0]);
                    pokemonIdList.Add(pokemonId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return pokemonIdList;
        }

        /*
         *  Method: Insert an entry to the PokemonMoves Table
         */
        public void InsertPokemonTypeJoin(PokemonDetailsInfo detailsInfo, TypeType type)
        {
            var typeId = GetTypeIdByTypeName(type.Name);

            var queryString = "INSERT INTO PokemonTypes (Pokemon_Id, Type_Id)\n" +
                              $"VALUES ({detailsInfo.Id}, {typeId})";
            try
            {
                using SqlConnection con = new(_connectionString);
                SqlCommand cmd = new(queryString, con);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error for Pokemon={detailsInfo.Name} & Type={type.Name} in InsertPokemonTypeJoin: {ex.Message}");
            }
        }

        /*
         *  Helper: 
         */
        private int? GetTypeIdByTypeName(string typeName)
        {
            int? typeId = null;

            var queryString = $"SELECT Id FROM Types WHERE Name = \'{typeName}\'";

            try
            {
                using SqlConnection con = new(_connectionString);
                SqlCommand cmd = new(queryString, con);

                con.Open();
                var rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    typeId = Convert.ToInt32(rdr[0]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error for MoveName={typeName} in GetMoveIdByMoveName: {ex.Message}");
            }

            return typeId;
        }

        public void InsertAbilityInfo(PokemonAbilityInfo abilityInfo)
        {
            var queryString = "INSERT INTO Abilities (Id, Name)\n" +
                              $"VALUES ({abilityInfo.Id}, N\'{ abilityInfo.Name }\')";
            try
            {
                using SqlConnection con = new(_connectionString);
                SqlCommand cmd = new(queryString, con);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in InsertAbilityInfo --> Ability Name: {abilityInfo.Name} --> {ex.Message}");
            }
        }
    }
}
