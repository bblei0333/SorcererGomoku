using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal;

public class Spawner : MonoBehaviour
{
    public int ding = 0;
    public int ID;
    // Start is called before the first frame update
    public void doSpawn(int num, Vector3 spot, Quaternion rot){
        GameObject.Find("Normy").GetComponent<IntSync>().Turn();
        if(ID == 0){
        Realtime.Instantiate("BpNorm", spot, rot);;

        Debug.Log("Black placed");
        }
        else{
        Realtime.Instantiate("WpNorm", spot, rot);
        Debug.Log("White placed");;
        }
    }
    void Connecter()
    {
        ding++;
        if(GetComponent<Realtime>().connected){
        ID = Realtime.Instantiate("BpNorm", new Vector3(123123,124124,13434), Quaternion.identity).GetComponent<RealtimeView>().ownerIDSelf;
        Debug.Log("ID " + ID);}
    }
    void Awake(){
    }

    // Update is called once per frame
    void Update()
    {
        if(GetComponent<Realtime>().connected && ding == 0){
            Connecter();
        }
    }
}
