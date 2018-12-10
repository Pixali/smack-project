using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class WeaponStats : IStatSource {
        public string Name { get; set; }
        public PlayerStats Player { get; set; }
        public float BaseDamage {
            get {
                return StatDictionary.CalcStat(Player, StatNames.PhysAttackDmg);
            }
            set {
                bundle.Mods[StatNames.PhysAttackDmg] = new StatDictionary.StatMod(0, (int)value);
            }
        }
        public float Range {
            get {
                return StatDictionary.CalcStat(Player, StatNames.CQAttackRange);
            }
            set {
                bundle.Mods[StatNames.CQAttackRange] = new StatDictionary.StatMod(0, (int)value);
            }
        }

        public StatBundle bundle;

        public WeaponStats() {
            bundle = new StatBundle {
                Source = this
            };
        }

        public StatBundle GetBundle() {
            return bundle;
        }
    }

}
