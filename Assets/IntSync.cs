using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class IntSync : RealtimeComponent<IntSyncModel>{
    void Update(){
    if (Input.GetMouseButtonDown(0)){
        IntSyncModel.increasenum();
        Debug.Log(IntSyncModel._testnum);
    }
    }

}
