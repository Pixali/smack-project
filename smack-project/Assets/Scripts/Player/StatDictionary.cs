using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Player {
    public static class StatDictionary {
        public struct StatReq {
            public float min, max;
        }
        public struct StatMod {
            public float mod, add;
            public bool isMalus; // stat = adds * mods - addsMalus * modsMalus

            public StatMod(float mod = 0, float add = 0, bool isMalus = false) {
                this.mod = mod;
                this.add = add;
                this.isMalus = isMalus;
            }
        }
        public struct StatCost {
            public StatNames stat;
            public float cost;
            public int time, tick, charges;
            // effectively a simple stat change (optionally over time or temporary)
            // tick increased by 1 each tick
            // when tick reaches time, reset to 0, subtract 1 from charges, apply cost
            // set charges to -1 to make the cost apply forever
            // set time to 0 to make it apply every tick
        }
        public struct StatCap {
            public float min, max;
            public Func<PlayerStats, float> minFunc, maxFunc;
            public StatCap(float min, float max, Func<PlayerStats, float> minFunc, Func<PlayerStats, float> maxFunc) {
                this.min = min;
                this.max = max;
                this.minFunc = minFunc;
                this.maxFunc = maxFunc;
            }
            public StatCap(float min, float max) : this(min, max, null, null) {
            }
        }
        public static Dictionary<StatNames, StatCap> StatCaps = new Dictionary<StatNames, StatCap>
        {
            {StatNames.MaxHP, new StatCap(0, float.PositiveInfinity)},
            {StatNames.MaxMP, new StatCap(0, float.PositiveInfinity)},
            {StatNames.MaxSP, new StatCap(0, float.PositiveInfinity)},
            {StatNames.CurrHP, new StatCap(0, float.PositiveInfinity, null, p => GetOrCalcStat(p, StatNames.MaxHP))},
            {StatNames.CurrMP, new StatCap(0, float.PositiveInfinity, null, p => GetOrCalcStat(p, StatNames.MaxMP))},
            {StatNames.CurrSP, new StatCap(0, float.PositiveInfinity, null, p => GetOrCalcStat(p, StatNames.MaxSP))},
            {StatNames.MoveSpeed, new StatCap(50, 190)}
        };

        public static float GetOrCalcStat(PlayerStats player, StatNames stat) {
            float i;
            return player.StatCache.TryGetValue(stat, out i) ? i : CalcStat(player, stat);
        }
        public static float CalcStat(PlayerStats player, StatNames stat, bool updateCache = true) {
            float mod = 1, add = 0;
            float modMalus = 1, addMalus = 0;
            foreach (var bundle in player.statBundles) {
                StatMod sMod;
                if (bundle.Mods.TryGetValue(stat, out sMod)) {
                    if (sMod.isMalus) {
                        modMalus += sMod.mod;
                        addMalus += sMod.add;
                    }
                    else {
                        mod += sMod.mod;
                        add += sMod.add;
                    }
                }
            }
            float total = add * mod - addMalus * modMalus;
            if (StatCaps.ContainsKey(stat)) {
                var cap = StatCaps[stat];
                var min = cap.minFunc != null ? cap.minFunc(player) : cap.min;
                var max = cap.maxFunc != null ? cap.maxFunc(player) : cap.max;
                total = total > max ? max : (total < min ? min : total);
            }
            if (updateCache) player.StatCache[stat] = total; // epic CPU cycle-saving memes
            return total;
        }
        public static bool CheckReq(PlayerStats player, StatReq req, StatNames stat) {
            float total = CalcStat(player, stat);
            return total >= req.min && total <= req.max;
        }
        public static void ApplyChange(PlayerStats player, StatCost cost) {
            player.activeCosts.Add(cost);
        }
        public static void StatTick(PlayerStats player, int ticks) {
            for (int i = player.activeCosts.Count - 1; i >= 0; i--) {
                var cost = player.activeCosts[i];
                if (cost.tick >= cost.time) {
                    cost.tick = 0;
                    if (cost.charges > 0) {
                        cost.charges--;
                    }
                    player.CostTotals[cost.stat] -= cost.cost;
                }
                else cost.tick += ticks;

                if (cost.charges == 0) player.activeCosts.RemoveAt(i);
                else player.activeCosts[i] = cost;
            }
        }

    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatNames : ushort {
        MaxHP, CurrHP,
        MaxMP, CurrMP,
        MaxSP, CurrSP,
        Vitality,
        Cunning,
        Wisdom,
        PhysDef, PhysIncomingRes,
        MagDef, MagIncomingRes,
        PhysStatusRes,
        MagStatusRes,
        PoisonDef, PoisonRes,
        MoveSpeed, AttackSpeed,
        ItemUseTime,
        SkillUseTime,

        PhysAttackDmg, MagAttackDmg,
        CQAttackRange, ProjAttackRange,

    }
}
