using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class Spawner : MonoBehaviour
{
    public int ding = 0; // A counter used to track when the connection has been established
    public int ID = 0; // The ID assigned to this instance when connected
    private int ownerIDSelf { get; } // Read-only property to get the owner's ID

    // This method spawns an object at the given position and rotation
    public void doSpawn(string name, Vector3 spot, Quaternion rot)
    {
        Realtime.Instantiate(name, spot, rot); // Instantiate the object over the network
    }

    // This method is called when the client connects and assigns a unique ID
    void Connecter()
    {
        ding++; // Increment the counter to indicate the connection process has started
        if (GetComponent<Realtime>().connected)
        {
            // Instantiate the "BpNorm" object and get its owner ID
            ID = Realtime.Instantiate("BpNorm", new Vector3(123123, 124124, 13434), Quaternion.identity)
                .GetComponent<RealtimeView>().ownerIDSelf;

            // Set the PiecePool's PID to the assigned ID
            GameObject.Find("GomokuBoard").GetComponent<PiecePool>().PID = ID;
            Debug.Log("ID: " + ID); // Log the assigned ID
        }
    }

    void Awake()
    {
        // Log the initial client ID when the object is awake
        Debug.Log("Client ID: " + ownerIDSelf);
    }

    // Update is called once per frame
    void Update()
    {
        // If the client is connected and 'ding' is 0, start the connection process
        if (GetComponent<Realtime>().connected && ding == 0)
        {
            Connecter(); // Call Connecter to set the ID
        }
    }
}
