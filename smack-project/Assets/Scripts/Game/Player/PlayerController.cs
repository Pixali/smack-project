/* This is the player connection, which is created on joining the game
 * This is differnt from Player.cs which is the script responsible for the actually player you see ig (mmovement/combat/interaction/etc)
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PlayerController : NetworkBehaviour {

    private GameObject playerObj;
    private GameObject weaponObj;

	void Start () {
        if (isLocalPlayer) {
            Debug.Log("PlayerConnection created...");
            playerObj = Resources.Load<GameObject>("Prefabs/Player/player");
            weaponObj = Resources.Load<GameObject>("Prefabs/Player/weapon"); // maybe just attach the weapon directly to the player instead?
            DontDestroyOnLoad(gameObject);
            //loadScene();
        }
	}

    void OnEnable() {
        SceneManager.sceneLoaded += sceneFinishedLoading;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= sceneFinishedLoading;
    }

    void sceneFinishedLoading(Scene scene, LoadSceneMode mode) {
        // when new scene has loaded, if scene is overworld spawn the player (dont want a player on the mainmenu, etc)
        if (scene.name == "overworld") {
            CmdSpawnPlayer();
            //Instantiate(playerObj, spawnLocation.position, Quaternion.identity);
           //Instantiate(weaponObj, playerObj.transform.position, Quaternion.identity);
           
        }
    }   

    [Command]
    void CmdSpawnPlayer() {
        Debug.Log("spawning player...");
        Transform spawnLocation = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
        GameObject player = Instantiate(playerObj, spawnLocation.position, Quaternion.identity);
        //GameObject weapon = Instantiate(weaponObj, playerObj.transform.position, Quaternion.identity);
        NetworkServer.SpawnWithClientAuthority(player, connectionToClient);
        //NetworkServer.SpawnWithClientAuthority(weapon, connectionToClient);
        Debug.Log("spawning player... done");
    }

    private void loadScene() {
        Debug.Log("loading new scene...");
        SceneManager.LoadScene("overworld");
    }

	
}
