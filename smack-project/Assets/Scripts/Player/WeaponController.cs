using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponController : NetworkBehaviour {

    private GameController gameController;

    private float canFire;
    public float fireRate;

	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();    
    }
	
	// Update is called once per frame
	void Update () {
	    if (!gameController.paused && isLocalPlayer) {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        }    
	}
}
