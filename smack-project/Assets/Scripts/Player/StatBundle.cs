using System;
using System.Collections.Generic;
using StatReq = Player.StatDictionary.StatReq;
using StatMod = Player.StatDictionary.StatMod;
using StatCost = Player.StatDictionary.StatCost;

namespace Player
{
    public class StatBundle
    {
        public Dictionary<StatNames, StatReq> Reqs;
        public Dictionary<StatNames, StatMod> Mods;
        public Dictionary<StatNames, StatCost> Costs;

        public StatBundle
            (Dictionary<StatNames, StatReq> reqs,
             Dictionary<StatNames, StatMod> mods,
             Dictionary<StatNames, StatCost> costs) {
            this.Reqs = reqs ?? new Dictionary<StatNames, StatReq>();
            this.Mods = mods ?? new Dictionary<StatNames, StatMod>();
            this.Costs = costs ?? new Dictionary<StatNames, StatCost>();
        }
        public StatBundle() : this(null, null, null) {
        }
    }
}
