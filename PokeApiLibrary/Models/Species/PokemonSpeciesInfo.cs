using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApiLibrary.Models.Species
{
    public class PokemonSpeciesInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PokemonGeneration Generation { get; set; }
    }

    public class PokemonGeneration
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
