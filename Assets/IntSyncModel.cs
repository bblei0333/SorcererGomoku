using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class IntSyncModel {
[RealtimeProperty(1, true, true)] 
public static int _testnum;
public static void increasenum(){
    _testnum++;
}
}