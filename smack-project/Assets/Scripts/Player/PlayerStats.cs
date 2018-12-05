using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatMod = Player.StatDictionary.StatMod;
using StatCost = Player.StatDictionary.StatCost;
namespace Player
{
    public class PlayerStats : IStatSource
    {

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

        public int MaxHP, CurrHP, MaxMP, CurrMP, MaxSP, CurrSP;

        public StatBundle BasicStats;

        public float Speed;
        public List<StatBundle> statBundles; // skills, equipment, etc.
        public Dictionary<StatNames, int> StatCache;

        // todo: load defaults from file
        public PlayerStats() {
            BasicStats = JsonConvert.DeserializeObject<StatBundle>(
                File.ReadAllText("Assets/Scripts/Data/BasicPlayerStats.json"));
            BasicStats.Source = this;
        }

        public StatBundle GetBundle()
        {
            return BasicStats;
        }
    }
}