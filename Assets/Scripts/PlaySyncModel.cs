using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class PlaySyncModel {
[RealtimeProperty(3, true, true)] 
public int _play;
}
