using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeApiLibrary.Models;
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

            var queryString = "INSERT INTO Moves (Id, Name, Accuracy, Power, PP)\n" +
                              $"VALUES ({ moveInfo.Id }, N\'{ moveInfo.Name }\', { accuracy}, { power }, { moveInfo.Pp })";
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

        public string GetTypeIdByTypeName(string typeName)
        {
            var typeId = "";

            var queryString = "SELECT Id FROM Types\n" + 
                              $"WHERE Name = '{typeName}'";
            try
            {
                using SqlConnection con = new(_connectionString);
                SqlCommand cmd = new(queryString, con);

                con.Open();
                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine(rdr[0]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return typeId;
        }

        public void InsertTypeInfo(PokemonType typeInfo)
        {
            var queryString = "INSERT INTO Types (Name)\n" +
                              $"VALUES (N\'{ typeInfo.Name }\')";
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
    }
}
