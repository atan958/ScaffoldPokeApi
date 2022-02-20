using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApiLibrary.Models.Species
{
    public class PokemonSpeciesList
    {
        public int Count { get; set; }
        public List<PokemonSpecies> Results { get; set; }
    }

}