using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
public class PlaySync : RealtimeComponent{
    //private int syncedint;
    public PlaySyncModel _model;
    public int papa;
    private PlaySyncModel model{
        set{
            _model = value;
        }
    }
    private void Start(){
        papa = _model.play;
    }
    private void Update(){
        if(papa != _model.play){
            papa = _model.play;
            GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().SyncGrid();
        }
    }

    public void Play(){
        _model.play++;
        papa = _model.play;
    }
    

}