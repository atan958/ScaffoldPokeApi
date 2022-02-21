using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApiLibrary.Models.Details
{
    class PokemonDetailsList
    {
        public int Count { get; set; }
        public List<PokemonDetails> Results { get; set; }
    }
}
