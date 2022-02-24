using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApiLibrary.Models.Abilities
{
    public class PokemonAbilitiesList
    {
        public int Count { get; set; }
        public List<PokemonAbility> Results { get; set; }
    }
}

