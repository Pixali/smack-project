using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

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
	void Start () {
		
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        rb = GetComponent<Rigidbody2D>();

	}
	
	void FixedUpdate () {
	    
        if (!gameController.paused) {
                
            float horiz = Input.GetAxisRaw("Horizontal");
            float verti = Input.GetAxisRaw("Vertical");

            if (horiz == 0 && verti == 0) {
                rb.velocity = new Vector2(0, 0); 
            } 
            if (horiz == 1 || horiz == -1) {
                rb.velocity = new Vector2(horiz * speed, 0);
            }
            if (verti == 1 || verti == -1) {
                rb.velocity = new Vector2(0, verti * speed); 
            }
            // diagonal movement
            if (horiz != 0 && verti != 0) {
                rb.velocity = new Vector2(horiz * speed, verti * speed);
            }

            
        }

	}

    void OnTriggerEnter2D(Collider2D other) {
        
        if (!gameController.paused) {
            Debug.Log("collided with "+other.gameObject);
        }
    }
}
