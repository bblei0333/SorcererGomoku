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
    public bool pbothBoom = false;
    public bool pPlayer0Boom;
    public bool pPlayer1Boom;
    public bool cameraShake = false;
    public byte[] bombGridPub= new byte[9];
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
        LPPy = _model.p2LPP % 100;
        LPPx = (_model.p2LPP - LPPy) / 100;
        f1 = _model.pflip1;
        f2 = _model.pflip2;
        bombGridPub = _model.bombGrid;
        if(_model.player0Boom && _model.player1Boom){
            _model.bothBoom = true;
        } else{
            _model.bothBoom = false;
        }
        pbothBoom = _model.bothBoom;
        pPlayer0Boom = _model.player0Boom;
        pPlayer1Boom = _model.player1Boom;
        cameraShake = _model.cameraShake;
        
        
    }
    
    public void setPlayer0Boom(bool isboom){
        _model.player0Boom = isboom;
    }
    public void setPlayer1Boom(bool isboom){
        _model.player1Boom = isboom;
    }

   
    
    public void SetBombGrid(int[] ints){
        for (int i = 0; i < ints.Length; i++)
        {
            _model.bombGrid[i] = (byte)ints[i];
        }


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

    public void cameraToggle(bool state){
        _model.cameraShake = state;
    }

}
