using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatMod = Player.StatDictionary.StatMod;
using StatCost = Player.StatDictionary.StatCost;
using System;

namespace Player {
    public class PlayerStats : IStatSource {

        // XP calculations
        // level is based on a recursive function:
        // f(1) = 0, f(2) = 10
        // f(n+1) = f(n) + (f(n) - f(n-1)) * 1.5
        // so the XP required to get from 1 to 2 is 10,
        // then every level after takes 50% more xp than the last

        public int XP;
        public int Level {
            get {
                return Mathf.FloorToInt(-Mathf.Log(40 / (3 * (this.XP + 20))) / (Mathf.Log(3) - Mathf.Log(2)));
            }
            set {
                this.XP = Mathf.FloorToInt(5 * Mathf.Pow(2, (3 - value)) * Mathf.Pow(3, (value - 1)) - 20);
            }
        }
        public int XPtoNext {
            get {
                int lv = this.Level + 1;
                return Mathf.FloorToInt(5 * Mathf.Pow(2, (3 - lv)) * Mathf.Pow(3, (lv - 1)) - 20) - this.XP;
            }
        }

        // we using stats now bois
        public float MaxHP {
            get { return StatDictionary.GetOrCalcStat(this, StatNames.MaxHP); }
            set { BasicStats.Mods[StatNames.MaxHP] = new StatMod(0, value); }
        }
        public float CurrHP {
            get { return StatDictionary.GetOrCalcStat(this, StatNames.CurrHP) - CostTotals.GetOrDefault(StatNames.CurrHP, 0); }
            set { CostTotals[StatNames.CurrHP] = value - StatDictionary.GetOrCalcStat(this, StatNames.CurrHP); }
        }
        public float MaxMP {
            get { return StatDictionary.GetOrCalcStat(this, StatNames.MaxMP); }
            set { BasicStats.Mods[StatNames.MaxMP] = new StatMod(0, value); }
        }
        public float CurrMP {
            get { return StatDictionary.GetOrCalcStat(this, StatNames.CurrMP) - CostTotals.GetOrDefault(StatNames.CurrMP, 0); }
            set { CostTotals[StatNames.CurrMP] = value - StatDictionary.GetOrCalcStat(this, StatNames.CurrMP); }
        }
        public float MaxSP {
            get { return StatDictionary.GetOrCalcStat(this, StatNames.MaxSP); }
            set { BasicStats.Mods[StatNames.MaxSP] = new StatMod(0, value); }
        }
        public float CurrSP {
            get { return StatDictionary.GetOrCalcStat(this, StatNames.CurrSP) - CostTotals.GetOrDefault(StatNames.CurrSP, 0); }
            set { CostTotals[StatNames.CurrSP] = value - StatDictionary.GetOrCalcStat(this, StatNames.CurrSP); }
        }

        public float Speed {
            get { return StatDictionary.GetOrCalcStat(this, StatNames.MoveSpeed); }
            set { BasicStats.Mods[StatNames.MoveSpeed] = new StatMod(0, value); }
        }

        public StatBundle BasicStats;


        public List<StatBundle> statBundles; // skills, equipment, etc.
        public List<StatCost> activeCosts;
        public Dictionary<StatNames, float> CostTotals; // reset on ex. death
        public Dictionary<StatNames, float> StatCache;

        public PlayerStats() {
            BasicStats = JsonConvert.DeserializeObject<StatBundle>(
                File.ReadAllText("Assets/Scripts/Data/BasicPlayerStats.json"));
            BasicStats.Source = this;
            statBundles = new List<StatBundle> { BasicStats };
            activeCosts = new List<StatCost>();
            CostTotals = new Dictionary<StatNames, float>();
            StatCache = new Dictionary<StatNames, float>();
        }

        public StatBundle GetBundle() {
            return BasicStats;
        }
    }
}