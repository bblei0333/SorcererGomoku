using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;

[RealtimeModel]
public partial class ByteSyncModel {
[RealtimeProperty(2, true, true, includeEqualityCheck: false)]
public byte[] _bytes = new byte[225];
}
