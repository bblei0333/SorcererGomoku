using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class IntSyncModel {
[RealtimeProperty(1, true, true)] 
public int _turner;

[RealtimeProperty(4, true, true)]
public int _wwin;

[RealtimeProperty(5, true, true)] 
public int _bwin;
}
