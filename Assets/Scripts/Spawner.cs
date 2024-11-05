using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public void doSpawn(int num, Vector3 spot, Quaternion rot){
        if(num == 1){
        Realtime.Instantiate("Pfabbp", spot, rot);}
        else{
        Realtime.Instantiate("Pfabwp", spot, rot);}
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
