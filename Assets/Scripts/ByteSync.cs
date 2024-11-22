using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class ByteSync : RealtimeComponent{
    //private int syncedint;
    public ByteSyncModel _model;
    private ByteSyncModel model{
        get{
            return _model;
        }
        set{
            _model = value;
        }
    }
    private void Update(){
        byte[] bytes = model.bytes;
        _model.bytes = bytes;
    }
    public bool checkEmpty(int x, int y){
        int oned = ((y*15) + x);
        if(_model.bytes[oned] == (byte)0){
            Debug.Log(oned + " empty");
            return true;
        }
        else{
            Debug.Log(oned + " filled");
            return false;
        }
    }
    public void doPlace(int x, int y, int bID){
        int oned = ((y*15) + x);
        _model.bytes[oned] = (byte)bID;
        Debug.Log("Set at: " + oned);
    }
    

}
