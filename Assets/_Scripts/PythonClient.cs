using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using SharpConnect;
using System.Security.Permissions;

public class PythonClient: MonoBehaviour {
    public Connector serverConnection = new Connector();
    string lastMessage;

    void Start() {
        Debug.Log(serverConnection.fnConnectResult("localhost", 10000));
    }
    
    void Update() {
    }

    void OnApplicationQuit() {
        try { serverConnection.fnDisconnect(); } catch { }
    }
}