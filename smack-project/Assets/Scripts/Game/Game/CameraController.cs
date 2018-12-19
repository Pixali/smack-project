using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class CameraController : MonoBehaviour {

    public float speed;
    private Player.Player player;
    private Transform playerPos;
    private GameObject playerObj;
    private PlayerStats playerStats;
    private static float maxLen;
    private static float driftLen = 15f;

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
                playerStats = player.playerStats;
                speed = StatDictionary.GetOrCalcStat(playerStats, StatNames.MoveSpeed) * 1.2f;
            }
        }
        else {

            var xyz = Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f);
            var xyzScreenSpace = new Vector3(xyz.x / Screen.width / 2, xyz.y / Screen.height / 2);
            if (xyz.sqrMagnitude > maxLen) {
                transform.position = Vector3.Lerp(
                    transform.position,
                    new Vector3(playerPos.position.x, playerPos.position.y, -10) + xyzScreenSpace * driftLen,
                    speed * Time.deltaTime);
            }
            else {
                transform.position = Vector3.Lerp(
                    transform.position,
                    new Vector3(playerPos.position.x, playerPos.position.y, -10),
                    speed * Time.deltaTime);
            }
            //transform.position = new Vector3(playerPos.position.x, playerPos.position.y, -10); 
            //transform.position = Vector3.Lerp(transform.position, new Vector3(playerPos.position.x, playerPos.position.y, -10), speed * Time.deltaTime);
        }
    }
}
