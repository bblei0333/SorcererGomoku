using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
public class IntSync : RealtimeComponent{
    public IntSyncModel _model;
    public int gaga;
    public int pwwin;
    public int pbwin;
    public int pbgrab, pwgrab;
    private IntSyncModel model{
        set{
            _model = value;
        }
    }
    private void Update(){
        gaga = _model.turner;
        pbwin = _model.bwin;
        pwwin = _model.wwin;
        pbgrab = _model.bgrab;
        pwgrab = _model.wgrab;
    }
    public void WhiteGrab(int num){
        _model.wgrab = num;
    }
    public void BlackGrab(int num){
        _model.bgrab = num;
    }
    public void BlackWin(){
        _model.bwin++;
    }
    public void WhiteWin(){
        _model.wwin++;
    }
    public void Turn(){
    if(_model.turner == 0){
        //Debug.Log("Turner++");
        _model.turner++;
    }
    else{
        //Debug.Log("Turner--");
        _model.turner--;
    }
    }
    

}
