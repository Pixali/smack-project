using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player {
    public class Player : MonoBehaviour
    {

        // stats
        public int maxHp;
        private int currentHp;

        public int maxMana;
        private int currentMana;
        public float manaRestoreRate;

        public float speed;

        // physics, etc
        private Rigidbody2D rb;
        private GameController gameController;

        // Use this for initialization
        void Start()
        {

            gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            rb = GetComponent<Rigidbody2D>();

        }

        void FixedUpdate()
        {

            if (!gameController.paused)
            {

                float horiz = Input.GetAxisRaw("Horizontal");
                float verti = Input.GetAxisRaw("Vertical");

                rb.velocity = new Vector2(horiz, verti).normalized * speed;
            }

        }

        void OnTriggerEnter2D(Collider2D other)
        {

            if (!gameController.paused)
            {
                Debug.Log("collided with " + other.gameObject);
            }
        }
    }
}
