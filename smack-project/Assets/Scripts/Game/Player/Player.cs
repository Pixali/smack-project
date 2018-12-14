using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Player {
    public class Player : NetworkBehaviour {
        // stats
        private PlayerStats playerStats;

        // weapon
        private List<Weapon> equipped_weapons;
        private Weapon current_weapon;

        // physics, etc
        private Rigidbody2D rb;
        private GameController gameController;

        // ui
        private Text hpText;
        private Text manaText;


        public bool useMana(int amount) {
            // todo: check remaining mana before casting in here.
            // it'd be silly to allow players to fling meteors around with 1MP.
            if (playerStats.CurrMP < amount) return false;
            StatDictionary.ApplyChange(playerStats, StatNames.CurrMP, amount);
            return true;
        }

        public void takeDmg(int dmg) {
            StatDictionary.ApplyChange(playerStats, StatNames.CurrHP, dmg);
        }

        private void shoot() {
            if (useMana(10)) {
                Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/projectile"), transform.position, transform.rotation);
            }
        }

        public override void OnStartAuthority()
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Player/myplayer");
            gameObject.tag = "MyPlayer";
        }

        // Use this for initialization
        void Start() {

            Debug.Log("player created");
            
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            
            rb = GetComponent<Rigidbody2D>();

            hpText = GameObject.Find("UI/Stats/hpText").GetComponent<Text>();
            manaText = GameObject.Find("UI/Stats/manaText").GetComponent<Text>();

            playerStats = new PlayerStats();

            //playerStats.CurrHP = playerStats.MaxHP;
            //playerStats.CurrMP = playerStats.MaxMP;

            UIManager.updateText(hpText, "hp: " + playerStats.CurrHP + "/" + playerStats.MaxHP);
            UIManager.updateText(manaText, "mana: " + playerStats.CurrMP + "/" + playerStats.MaxMP);

            equipped_weapons = new List<Weapon>();

            StatDictionary.AddStatListener(StatNames.CurrHP, p => {
                if (p == playerStats) {
                    UIManager.updateText(hpText, "hp: " + p.CurrHP + "/" + p.MaxHP);
                    if (p.CurrHP <= 0) {
                        //playerStats.CurrHP = 0;
                        Debug.Log("You have died!");
                        Destroy(gameObject);
                        // todo: respawning. don't forget to clear out playerStats.CostTotals!
                    }
                }
            });
            StatDictionary.AddStatListener(StatNames.CurrMP, p => {
                if (p == playerStats) {
                    UIManager.updateText(manaText, "mana: " + playerStats.CurrMP + "/" + playerStats.MaxMP);
                }
            });
        }



        void FixedUpdate() {

            if (!gameController.paused && hasAuthority) {
                //rb.transform.rotation = Quaternion.LookRotation(Vector3.forward, Camera.main.ScreenToWorldPoint(Input.mousePosition) - rb.transform.position);

                if (Input.GetKeyDown("space")) { // move this to weapon?
                    shoot();
                }
                float horiz = Input.GetAxisRaw("Horizontal");
                float verti = Input.GetAxisRaw("Vertical");
                rb.velocity = new Vector2(horiz, verti).normalized * playerStats.Speed;
            }

        }
        void Update() {
            StatDictionary.StatTick(this.playerStats, Time.deltaTime);
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (!gameController.paused) {
                //Debug.Log("collided with " + other.gameObject);
            }
        }
    }
}
