using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyNetworkManager : NetworkManager {

    public string addr;
    public int port;

    public void host() {
        networkPort = port;
        StartHost();
    }

    public void join() {
        networkAddress = addr;
        networkPort = port;
        StartClient();
        Debug.Log("client started");
    }


}
