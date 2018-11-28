using System;
using System.Collections.Generic;

namespace Player
{
    public class EquipmentStats
    {
        public List<StatDictionary.StatReq> Reqs;
        public List<StatDictionary.StatMod> Mods;
        public List<StatDictionary.StatCost> Costs;

        public EquipmentStats(List<StatDictionary.StatReq> reqs, List<StatDictionary.StatMod> mods, List<StatDictionary.StatCost> costs)
        {
            this.Reqs = reqs;
            this.Mods = mods;
            this.Costs = costs;
        }
    }
}
