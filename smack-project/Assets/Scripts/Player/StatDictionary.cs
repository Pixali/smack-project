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
            public StatCap(int min, int max)
            {
                this.min = min;
                this.max = max;
            }
        }
        public static Dictionary<StatNames, StatCap> StatCaps = new Dictionary<StatNames, StatCap>
        {
            {StatNames.MoveSpeed, new StatCap(50, 190)}
        };
        public static int CalcStat(PlayerStats player, StatNames stat)
        {
            int mod = 1, add = 0;
            foreach (var equip in player.equipmentStats)
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
                total = total > cap.max ? cap.max : (total < cap.min ? cap.min : total);
            }
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
