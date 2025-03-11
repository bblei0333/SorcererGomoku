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

[RealtimeProperty(6, true, true)] 
public int _wgrab;

[RealtimeProperty(7, true, true)] 
public int _bgrab;

[RealtimeProperty(8, true, true)] 
public int _p1LPP;

[RealtimeProperty(9, true, true)] 
public int _p2LPP;

[RealtimeProperty(13, true, true)] 
public int _p3LPP;

[RealtimeProperty(10, true, true)] 
public int _pflip1;

[RealtimeProperty(11, true, true)] 
public int _pflip2;
[RealtimeProperty(12, true, true)] 
public int _AnimatingP;

[RealtimeProperty(14, true, true)]
public byte[] _bombGrid = new byte[9];
}
