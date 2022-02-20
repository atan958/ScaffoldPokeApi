using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApiLibrary.Models.Types
{
    public class PokemonTypesList
    {
        public int Count { get; set; }
        public List<PokemonType> Results { get; set; }
    }
}
