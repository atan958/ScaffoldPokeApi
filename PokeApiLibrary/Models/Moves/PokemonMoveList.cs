using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApiLibrary.Models.Moves
{
    public class PokemonMoveList
    {
        public int Count { get; set; }
        public List<PokemonMove> Results { get; set; }
    }
}