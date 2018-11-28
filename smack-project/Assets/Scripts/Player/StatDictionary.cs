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
        }
        public struct StatCost
        {

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
            {StatNames.CurrHP, new StatCap(0, int.MaxValue, null, // I seriously wish this could be condensed better
               p => p.StatCache.ContainsKey(StatNames.MaxHP) ? p.StatCache[StatNames.MaxHP] : CalcStat(p, StatNames.MaxHP))},
            {StatNames.CurrMP, new StatCap(0, int.MaxValue, null,
               p => p.StatCache.ContainsKey(StatNames.MaxMP) ? p.StatCache[StatNames.MaxMP] : CalcStat(p, StatNames.MaxMP))},
            {StatNames.CurrSP, new StatCap(0, int.MaxValue, null,
               p => p.StatCache.ContainsKey(StatNames.MaxSP) ? p.StatCache[StatNames.MaxSP] : CalcStat(p, StatNames.MaxSP))},
            {StatNames.MoveSpeed, new StatCap(50, 190)}
        };
        public static int CalcStat(PlayerStats player, StatNames stat, bool updateCache = true)
        {
            int mod = 1, add = 0;
            foreach (var equip in player.EquipmentStats)
            {
                foreach (var emod in equip.Mods)
                {
                    if (emod.stat != stat) continue;
                    mod += emod.mod;
                    add += emod.add;
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
        public static bool CheckReq(PlayerStats player, StatReq req)
        {
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
