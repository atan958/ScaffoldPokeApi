using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PokeApiLibrary.Models.Details
{
    public class PokemonDetailsInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DetailsAbility> Abilities { get; set; }
        public List<DetailsStat> Stats { get; set; }
        public List<DetailsType> Types { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
    }

    public class DetailsAbility
    {
        public AbilityAbility Ability { get; set; }
    }

    public class AbilityAbility
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class DetailsStat
    {
        public int Base_Stat { get; set; }
        public int Effort { get; set; }
        public StatStat Stat { get; set; }
    }

    public class StatStat
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class DetailsType
    {
        public TypeType Type { get; set; }
    }

    public class TypeType 
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
