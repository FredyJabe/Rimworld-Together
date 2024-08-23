using System;
using System.Collections.Generic;

namespace Shared
{
    [Serializable]
    public class SiteValuesFile
    {
        public int PersonalFarmlandCost = 1000;
        public int FactionFarmlandCost = 2000;
        public int FarmlandRewardCount = 50;

        public int PersonalQuarryCost = 1000;
        public int FactionQuarryCost = 2000;
        public int QuarryRewardCount = 50;

        public int PersonalSawmillCost = 1000;
        public int FactionSawmillCost = 2000;
        public int SawmillRewardCount = 50;

        public int PersonalBankCost = 1000;
        public int FactionBankCost = 2000;
        public int BankRewardCount = 50;

        public int PersonalLaboratoryCost = 1000;
        public int FactionLaboratoryCost = 2000;
        public int LaboratoryRewardCount = 50;

        public int PersonalRefineryCost = 1000;
        public int FactionRefineryCost = 2000;
        public int RefineryRewardCount = 50;

        public int PersonalHerbalWorkshopCost = 1000;
        public int FactionHerbalWorkshopCost = 2000;
        public int HerbalWorkshopRewardCount = 50;

        public int PersonalTextileFactoryCost = 1000;
        public int FactionTextileFactoryCost = 2000;
        public int TextileFactoryRewardCount = 50;

        public int PersonalFoodProcessorCost = 1000;
        public int FactionFoodProcessorCost = 2000;
        public int FoodProcessorRewardCount = 50;


        public float PersonalCostMultiplier = 1.0f;
        public float FactionCostMultipliser = 5.0f;
        public List<PlayerSite> Sites = new()
        {
            new("Farmland", new(){{"Silver",1000}}, new(){{"Potato",50}}),
            new("Quarry", new(){{"Silver",1000}}, new(){{"Steel",50}}),
            new("Sawmill", new(){{"Silver",1000}}, new(){{"WoodLog",50}}),
            new("Bank", new(){{"Silver",1000}}, new(){{"Silver",50}}),
            new("Laboratory", new(){{"Silver",1000}}, new(){{"ComponentIndustrial",50}}),
            new("Refinery", new(){{"Silver",1000}}, new(){{"Chemfuel",50}}),
            new("Heral Workshop", new(){{"Silver",1000}}, new(){{"Potato",50}}),
            new("Textile Factory", new(){{"Silver",1000}}, new(){{"Potato",50}}),
            new("Food Processor", new(){{"Silver",1000}}, new(){{"Potato",50}})
        };
    }

    public class PlayerSite
    {
        public PlayerSite(string name, Dictionary<string, int> cost, Dictionary<string, int> reward)
        {
            Name = name;
            Cost = cost;
            Reward = reward;
        }
        public string Name = "Site";
        public Dictionary<string, int> Cost = new()
        {
            { "Silver", 100},
            { "Steel", 100}
        };
        public Dictionary<string,int> Reward = new()
        {
            { "Silver", 5 }
        };
    }
}
