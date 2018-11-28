using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    // todo networking

    public float lifetime;
    public float speed;
    public int dmg;

    private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
	}
	
	// Update is called once per frame
	void Update () {
        rb.transform.position += rb.transform.up * Time.deltaTime * speed;
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
