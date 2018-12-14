using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// todo networking

namespace Player
{
    public class Weapon : NetworkBehaviour
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

            var player_obj = GameObject.FindGameObjectWithTag("MyPlayer");

            if(player_obj)
            {
                owner = player_obj.GetComponent<Player>();
            }

            if (owner == null)
                return;

            if (!hasAuthority) {
                return;
            }

            transform.SetParent(owner.transform);

            rb.transform.rotation = Quaternion.LookRotation(Vector3.forward , Camera.main.ScreenToWorldPoint(Input.mousePosition) - rb.transform.position);


            transform.position = owner.transform.position + (owner.transform.up * distance_offset);
        }
    }

}
