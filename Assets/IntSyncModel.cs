using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;
using System;

[RealtimeModel]
public partial class IntSyncModel {
[RealtimeProperty(1, true, true)] 
public int _testnum;
public event Action<IntSyncModel, int> intDidChange;
private void OnTestNumChanged() {
        // Invoke the event to notify listeners
        if (intDidChange != null) {
            intDidChange(this, _testnum);
        }
    }
public void SetTestNum(int value) {
        if (_testnum != value) {
            _testnum = value;
            OnTestNumChanged();
        }
    }
}
