using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApiLibrary.Models.Abilities
{
    public class PokemonAbilityWithId
    {
        public PokemonAbility Ability { get; set; }
        public int? Id { get; set; }
    }
}
