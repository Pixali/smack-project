using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float speed;
    private Player.Player player;
    private Transform playerPos;
    private GameObject playerObj;

	void Update () {
        if (playerObj == null) {
            playerObj = GameObject.FindGameObjectWithTag("MyPlayer");
            if (playerObj != null) {
                playerPos = playerObj.transform;
                player = playerObj.GetComponent<Player.Player>();
            }
        }
        else {
            transform.position = new Vector3(playerPos.position.x, playerPos.position.y, -10); 
            //transform.position = Vector3.Lerp(transform.position, new Vector3(playerPos.position.x, playerPos.position.y, -10), speed * Time.deltaTime);
        }
    }
}
