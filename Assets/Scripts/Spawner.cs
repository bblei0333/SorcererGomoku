using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal;

public class Spawner : MonoBehaviour
{
    public int ding = 0;
    public int ID = 0;
    // Start is called before the first frame update
    private int ownerIDSelf {get;}
    public void doSpawn(string name, Vector3 spot, Quaternion rot){
        Realtime.Instantiate(name, spot, rot);
    }
    void Connecter()
    {
        ding++;
        if(GetComponent<Realtime>().connected){
        ID = Realtime.Instantiate("BpNorm", new Vector3(123123,124124,13434), Quaternion.identity).GetComponent<RealtimeView>().ownerIDSelf;
        GameObject.Find("GomokuBoard").GetComponent<PiecePool>().PID = ID;
        Debug.Log("ID " + ID);}
    }

    void Awake()
    {
        Debug.Log("Client ID: " + ownerIDSelf);
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Realtime>().connected && ding == 0){
            Connecter();
        }
    }
}
