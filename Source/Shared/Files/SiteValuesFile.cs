using System;
using System.Collections.Generic;

namespace Shared
{
    [Serializable]
    public class SiteValuesFile
    {
        public float PersonalCostMultiplier = 1.0f;
        public float FactionCostMultipliser = 5.0f;
        public List<PlayerSite> Sites = new()
        {
            new("Farmland", new(){{"Silver",1000}}, new(){{"RawPotatoes",50}}),
            new("Quarry", new(){{"Silver",1000}}, new(){{"Steel",50}}),
            new("Sawmill", new(){{"Silver",1000}}, new(){{"WoodLog",50}}),
            new("Bank", new(){{"Silver",1000}}, new(){{"Silver",50}}),
            new("Laboratory", new(){{"Silver",1000}}, new(){{"ComponentIndustrial",50}}),
            new("Refinery", new(){{"Silver",1000}}, new(){{"Chemfuel",50}}),
            new("Herbal Workshop", new(){{"Silver",1000}}, new(){{"MedicineHerbal",50}}),
            new("Textile Factory", new(){{"Silver",1000}}, new(){{"Cloth",50}}),
            new("Food Processor", new(){{"Silver",1000}}, new(){{"MealSimple",50}})
        };
    }

    public class PlayerSite
    {
        public PlayerSite(string name, Dictionary<string, int> cost, Dictionary<string, int> reward, int limit = 0)
        {
            Name = name;
            Cost = cost;
            Reward = reward;
            Limit = limit;
        }
        public string Name;
        public int Limit;
        public Dictionary<string, int> Cost;
        public Dictionary<string,int> Reward;
    }
}
