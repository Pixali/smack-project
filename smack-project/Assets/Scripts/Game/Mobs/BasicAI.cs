using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAI : MonoBehaviour {

    // todo make guys network

    public int hp;
    public float speed;
    public int dmg;

    private bool inrange = false;

	// Use this for initialization
	void Start () {
		
	}
	
    void takeDmg(int dmg) {
        hp -= dmg;
        if (hp <= 0) {
            Destroy(gameObject);
        }
    }

	// Update is called once per frame
	void Update () {
        if (!inrange) {
            // search for closest player to fight with
            GameObject target = Utility.findClosestGameObject(gameObject, "Player");
            if (target != null) {
                transform.rotation = Quaternion.LookRotation(Vector3.forward, target.transform.position - transform.position);
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            }

        }
    }

    IEnumerator attack(GameObject target) {
        while (inrange) {
            Debug.Log("Attacking...");
            target.GetComponent<Player.Player>().takeDmg(dmg);
            yield return new WaitForSeconds(0.5f);
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            // in melee range, attack
            inrange = true;
            StartCoroutine(attack(other.gameObject));
        }
        else if (other.gameObject.tag == "Projectile") {
            takeDmg(other.gameObject.GetComponent<Projectile>().dmg);
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            inrange = false;
        }
    }
}
