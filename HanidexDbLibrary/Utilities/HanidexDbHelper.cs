using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeApiLibrary.Models;
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
            _connectionString = @"Data Source=ANGELO\SQLEXPRESS;Initial Catalog=TrialHanidex;Integrated Security=True";
        }

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
                Console.WriteLine(ex.Message);
            }
        }

        private int GetGenerationNumber(string pokemonGen)
        {
            switch (pokemonGen)
            {
                case "generation-i":
                    return 1;
                case "generation-ii":
                    return 2;
                case "generation-iii":
                    return 3;
                case "generation-iv":
                    return 4;
                case "generation-v":
                    return 5;
                case "generation-vi":
                    return 6;
                case "generation-vii":
                    return 7;
                case "generation-viii":
                    return 8;
                default:
                    return 0;
            }
        }

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

        public void InsertTypeInfo(PokemonType typeInfo)
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
         *
         */
        public void InsertPokemonMoveJoin1(PokemonSpecies species, PokemonMoveInfo moveInfo)
        {
            var pokemonId = GetPokemonIdByPokemonName(species.Name);

            var queryString = "INSERT INTO PokemonMoves (Pokemon_Id, Move_Id)\n" +
                              $"VALUES ({pokemonId}, {moveInfo.Id})";
            try
            {
                using SqlConnection con = new(_connectionString);
                SqlCommand cmd = new(queryString, con);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PokemonMove: {species.Name} {moveInfo.Name} in InsertPokemonMoveJoin: {ex.Message}");
            }
        }

        /*
         *  Helper Method: 
         */
        private string GetPokemonIdByPokemonName(string pokemonName)
        {
            var typeId = "0";

            var queryString = "SELECT Id FROM Pokemon\n" + 
                              $"WHERE Name = '{pokemonName}'";
            try
            {
                using SqlConnection con = new(_connectionString);
                SqlCommand cmd = new(queryString, con);

                con.Open();
                var rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    Console.WriteLine(rdr[0]);
                    typeId = Convert.ToString(rdr[0]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Pokemon: {pokemonName} in GetPokemonIdByPokemonName: {ex.Message}");
            }

            return typeId;
        }

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

        public int? GetMoveIdByMoveName(string moveName)
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
         *  Acquires the Ids of all the Pokemon species stored in the Database
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
    }
}
