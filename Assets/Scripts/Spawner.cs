using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public void doSpawn(int num, Vector3 spot, Quaternion rot){
        if(Realtime.Instantiate("BpNorm", spot, rot).GetComponent<RealtimeView>().ownerIDSelf == 0){
        Realtime.Instantiate("BpNorm", spot, rot);}
        else{
        Realtime.Instantiate("WpNorm", spot, rot);}
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
