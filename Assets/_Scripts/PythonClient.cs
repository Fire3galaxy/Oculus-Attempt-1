using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using SharpConnect;
using System.Security.Permissions;

public class PythonClient: MonoBehaviour {
    public Connector serverConnection = new Connector();
    string lastMessage;
    public Transform PlayerCoord;

    void Start() {
        Debug.Log(serverConnection.fnConnectResult("localhost", 10000));
    }
    void Update() {
        // 3 arguments for "action": Action|Target|Coordinates
        if (Input.GetKeyDown("space")) {
            Debug.Log("space key = 0,0,0");
            serverConnection.fnPacketTest("MOVE|LARM|" + PlayerCoord.transform.position); 
        }
    }

    void OnApplicationQuit() {
        try { serverConnection.fnDisconnect(); } catch { }
    }
}