using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class IntSync : RealtimeComponent{
    //private int syncedint;
    public IntSyncModel _model;
    public int gaga;
    public int frame;
    public int hold;
    private IntSyncModel model{
        set{
            _model = value;
        }
    }
    private void Update(){
        gaga = _model.turner;
    }
    public void Turn(){
    if(_model.turner == 0){
        _model.turner++;
    }
    else{
        _model.turner--;
    }
    }
    

}
