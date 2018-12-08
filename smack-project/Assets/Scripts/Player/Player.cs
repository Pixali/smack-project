using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Player {
    public class Player : NetworkBehaviour
    {
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

        // set sprite to red for "our" player
        public override void OnStartLocalPlayer() {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Player/myplayer");
        }

        public void useMana(int amount) {
            playerStats.CurrMP -= amount;
            if (playerStats.CurrMP <= 0)
            {
                playerStats.CurrMP = 0;
                Debug.Log("You have no mana");
            }
            manaText.text = "mana: " + playerStats.CurrMP + "/" + playerStats.MaxMP;
        }

        public void takeDmg(int dmg) {
            playerStats.CurrHP -= dmg;
            if (playerStats.CurrHP <= 0)
            {
                playerStats.CurrHP = 0;
                Debug.Log("You have died!");
                Destroy(gameObject);
            }
            hpText.text = "hp: " + playerStats.CurrHP + "/" + playerStats.MaxHP;
        }

        private void shoot() {
            if (playerStats.CurrMP >= 10) {
                useMana(10);
                Instantiate(Resources.Load<GameObject>("Prefabs/Projectiles/projectile"), transform.position, transform.rotation);
            }
        }

        // Use this for initialization
        void Start() {
            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            rb = GetComponent<Rigidbody2D>();

            hpText = GameObject.Find("UI/Stats/hpText").GetComponent<Text>();
            manaText = GameObject.Find("UI/Stats/manaText").GetComponent<Text>();

            playerStats = new PlayerStats
            {
                MaxHP = 100,
                MaxMP = 100,
                Speed = 3.5f
            };

            playerStats.CurrHP = playerStats.MaxHP;
            playerStats.CurrMP = playerStats.MaxMP;

            hpText.text = "hp: " + playerStats.CurrHP + "/" + playerStats.MaxHP;
            manaText.text = "mana: " + playerStats.CurrMP + "/" + playerStats.MaxMP;

            equipped_weapons = new List<Weapon>();
        }



        void FixedUpdate() {
            if (!gameController.paused) {
                rb.transform.rotation = Quaternion.LookRotation(Vector3.forward, Camera.main.ScreenToWorldPoint(Input.mousePosition) - rb.transform.position);

                if (Input.GetKeyDown("space")) {
                    shoot();
                }

                float horiz = Input.GetAxisRaw("Horizontal");
                float verti = Input.GetAxisRaw("Vertical");

                rb.velocity = new Vector2(horiz, verti).normalized * playerStats.Speed;
            }

        }

        void OnTriggerEnter2D(Collider2D other) {
            if (!gameController.paused) {
                Debug.Log("collided with " + other.gameObject);
            }
        }
    }
}
