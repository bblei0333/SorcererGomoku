using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
public class IntSync : RealtimeComponent{
    public IntSyncModel _model;
    public int gaga;
    public int pwwin;
    public int pbwin;
    public int pbgrab, pwgrab, LPPID, LPPx, LPPy, f1, f2, Animating, LPlayer;
    private IntSyncModel model{
        set{
            _model = value;
        }
    }
    private void Update(){
        Animating = _model.AnimatingP;
        LPlayer = _model.p3LPP;
        gaga = _model.turner;
        pbwin = _model.bwin;
        pwwin = _model.wwin;
        pbgrab = _model.bgrab;
        pwgrab = _model.wgrab;
        LPPID = _model.p1LPP;
        LPPy = (_model.p2LPP % 100);
        LPPx = ((_model.p2LPP - LPPy) / 100);
        f1 = _model.pflip1;
        f2 = _model.pflip2;
    }

    public void SetBombGrid(int id1, int id2, int id3, int id4, int id5, int id6, int id7, int id8, int id9 ){
        _model.bombGrid1 = id1;
        _model.bombGrid2 = id2;
        _model.bombGrid3 = id3;
        _model.bombGrid4 = id4;
        _model.bombGrid5 = id5;
        _model.bombGrid6 = id6;
        _model.bombGrid7 = id7;
        _model.bombGrid8 = id8;
        _model.bombGrid9 = id9;
    }

    public void SetAnimation(int num){
        _model.AnimatingP = num;
    }

    public void SetFlip(int num, int num2){
        _model.pflip1 = num;
        _model.pflip2 = num2;
    }

    public void SetLPP(int player, int num, int x, int y){
        _model.p1LPP = num;
        x = x * 100;
        _model.p2LPP = x + y;
        _model.p3LPP = player;
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
