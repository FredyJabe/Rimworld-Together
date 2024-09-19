﻿using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace GameClient
{
    public static class RewardManager
    {
        public static void ParseRewardPacket(Packet packet)
        {
            RewardData siteData = Serializer.ConvertBytesToObject<RewardData>(packet.contents);
            ReceiveRewards(siteData);
        }

        private static void ReceiveRewards(RewardData siteData)
        {
            foreach (RewardFile reward in siteData._rewardData)
            {
                for (int i = 0; reward.RewardDefs.Length > i; i++)
                {
                    ThingDataFile thingData = new ThingDataFile();
                    thingData.DefName = reward.RewardDefs[i];
                    thingData.Quantity = reward.RewardAmount[i];
                    thingData.Quality = 0;
                    thingData.Hitpoints = DefDatabase<ThingDef>.GetNamed(thingData.DefName).BaseMaxHitPoints;
                    ThingScribeManager.StringToItem(thingData);
                }
            }
        }
    }
}
