using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class Weapon : MonoBehaviour
    {
        private Player owner;
        private Rigidbody2D rb;
        private BoxCollider2D collider;
        private float distance_offset = 1f;
        private GameController controller;

        public WeaponStats stats;

        

        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            collider = GetComponent<BoxCollider2D>();
            controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }

        void FixedUpdate()
        {
            if (controller.paused)
                return;

            var player_obj = GameObject.FindGameObjectWithTag("Player");

            if(player_obj)
            {
                owner = player_obj.GetComponent<Player>();
            }

            if (owner == null)
                return;

            transform.SetParent(owner.transform);

            rb.transform.rotation = Quaternion.LookRotation(Vector3.forward , Camera.main.ScreenToWorldPoint(Input.mousePosition) - rb.transform.position);


            transform.position = owner.transform.position + (owner.transform.up * distance_offset);
        }
    }

}
