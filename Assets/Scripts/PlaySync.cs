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
            //check for win for both pieces when opponent goes. 
            GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().CheckForWin(1);
            GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().CheckForWin(2);
        }
    }

    public void Play(){
         //Debug.Log("Play");
        GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().FlippedTurn = false;
        _model.play++;
        papa = _model.play;
        //GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().Booming = false;
    }
    

}
