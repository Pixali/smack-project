using System;
using System.Collections.Generic;
using System.Linq;

namespace Player
{
    public static class StatDictionary
    {
        public struct StatReq
        {
            public StatNames stat;
            public int min, max;
        }
        public struct StatMod
        {
            public StatNames stat;
            public int mod, add;
            public bool isMalus; // stat = adds * mods - addsMalus * modsMalus

            public StatMod(StatNames stat, int mod = 0, int add = 0, bool isMalus = false) {
                this.stat = stat;
                this.mod = mod;
                this.add = add;
                this.isMalus = isMalus;
            }
        }
        public struct StatCost
        {
            public StatNames stat;
            public int cost, time;
            // effectively a simple stat change (optionally over time or temporary)
            // cost is applied each tick while time >= 0
            // set time to -1 to make it a debuff (while active)
            // set time to 0 to make it an instant effect, like basic damage
            // time is reduced by 1 every tick
        }
        public struct StatCap
        {
            public int min, max;
            public Func<PlayerStats, int> minFunc, maxFunc;
            public StatCap(int min, int max, Func<PlayerStats, int> minFunc, Func<PlayerStats, int> maxFunc) {
                this.min = min;
                this.max = max;
                this.minFunc = minFunc;
                this.maxFunc = maxFunc;
            }
            public StatCap(int min, int max) : this(min, max, null, null) {
            }
        }
        public static Dictionary<StatNames, StatCap> StatCaps = new Dictionary<StatNames, StatCap>
        {
            {StatNames.MaxHP, new StatCap(0, int.MaxValue)},
            {StatNames.MaxMP, new StatCap(0, int.MaxValue)},
            {StatNames.MaxSP, new StatCap(0, int.MaxValue)},
            {StatNames.CurrHP, new StatCap(0, int.MaxValue, null, p => GetOrCalcStat(p, StatNames.MaxHP))},
            {StatNames.CurrMP, new StatCap(0, int.MaxValue, null, p => GetOrCalcStat(p, StatNames.MaxMP))},
            {StatNames.CurrSP, new StatCap(0, int.MaxValue, null, p => GetOrCalcStat(p, StatNames.MaxSP))},
            {StatNames.MoveSpeed, new StatCap(50, 190)}
        };

        public static int GetOrCalcStat(PlayerStats player, StatNames stat) {
            int i;
            return player.StatCache.TryGetValue(stat, out i) ? i : CalcStat(player, stat);
        }
        public static int CalcStat(PlayerStats player, StatNames stat, bool updateCache = true) {
            int mod = 1, add = 0;
            int modMalus = 1, addMalus = 0;
            foreach (var equip in player.statBundles)
            {
                StatMod sMod;
                if (equip.Mods.TryGetValue(stat, out sMod)) {
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
            int total = add * mod;
            if (StatCaps.ContainsKey(stat))
            {
                var cap = StatCaps[stat];
                var min = cap.minFunc != null ? cap.minFunc(player) : cap.min;
                var max = cap.maxFunc != null ? cap.maxFunc(player) : cap.max;
                total = total > max ? max : (total < min ? min : total);
            }
            if (updateCache) player.StatCache[stat] = total; // epic CPU cycle-saving memes
            return total;
        }
        public static bool CheckReq(PlayerStats player, StatReq req) {
            int total = CalcStat(player, req.stat);
            return total >= req.min && total <= req.max;
        }
    }
    public enum StatNames : ushort
    {
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

    }
}
