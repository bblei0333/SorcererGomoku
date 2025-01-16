using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
public class IntSync : RealtimeComponent{
    public IntSyncModel _model;
    public int gaga;
    public int pwwin;
    public int pbwin;
    private IntSyncModel model{
        set{
            _model = value;
        }
    }
    private void Update(){
        gaga = _model.turner;
        pbwin = _model.bwin;
        pwwin = _model.wwin;
    }
    public void BlackWin(){
        _model.bwin++;
    }
    public void WhiteWin(){
        _model.wwin++;
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
