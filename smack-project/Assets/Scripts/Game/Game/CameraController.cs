using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class CameraController : MonoBehaviour {

    public float speed;
    private Player.Player player;
    private Transform playerPos;
    private GameObject playerObj;
    //private PlayerStats playerStats;
    private static float maxLen;
    private static float driftLen = 15f;
    private Vector3 driftPos = Vector3.zero;

    static CameraController() {
        var smallerLen = Screen.width > Screen.height ? Screen.height : Screen.width;
        maxLen = new Vector3(smallerLen / 2f, smallerLen / 2f).magnitude * 0.5f;
        maxLen *= maxLen;
    }

	void Update () {
        if (playerObj == null) {
            playerObj = GameObject.FindGameObjectWithTag("MyPlayer");
            if (playerObj != null) {
                playerPos = playerObj.transform;
                player = playerObj.GetComponent<Player.Player>();
            }
        }
        else {

            var xyz = Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f);
            var xyzScreenSpace = new Vector3(xyz.x / Screen.width / 2, xyz.y / Screen.height / 2);
            driftPos = xyz.sqrMagnitude > maxLen
                ? Vector3.Lerp(driftPos, xyzScreenSpace * driftLen, speed * Time.deltaTime)
                : Vector3.Lerp(driftPos, Vector3.zero, speed * Time.deltaTime);
            driftPos.z = -10;
            transform.position = playerPos.position + driftPos;
        }
    }
}
