using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeApiLibrary.Models.Species;

namespace PokeApiLibrary.Models.Moves
{
    public class PokemonMoveInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Accuracy { get; set; }
        public MoveDamageClass Damage_Class { get; set;  }
        public List<PokemonSpecies> Learned_By_Pokemon { get; set; }
        public MoveMeta Meta { get; set; }
        public int? Power { get; set; }
        public int? Pp { get; set; }
        public MoveType Type { get; set; }
    }

    public class MoveDamageClass
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class MoveMeta
    {
        public MetaAilment Ailment { get; set; }
        public int? Ailment_Chance { get; set; }
        public int? Crit_Rate { get; set; }
        public int? Drain { get; set; }
        public int? Flinch_Chance { get; set; }
        public int? Healing { get; set; }
        public int? Max_Hits { get; set; }
        public int? Max_Turns { get; set; }
        public int? Min_Hits { get; set; }
        public int? Min_Turns { get; set; }
        public int? Stat_Chance { get; set; }
    }

    public class MetaAilment
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class MoveType
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
