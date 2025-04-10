using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
public class ByteSync : RealtimeComponent{
    //private int syncedint;
    public ByteSyncModel _model;
    public ByteSyncModel model{
        get{
            return _model;
        }
        set{
            _model = value;
        }
    }
    public bool checkEmpty(int x, int y){
        int oned = ((y*15) + x);
        if(_model.bytes[oned] == (byte)0){
            //Debug.Log(oned + " empty");
            return true;
        }
        else{
            //Debug.Log(oned + " filled");
            return false;
        }
    }
    public void doPlace(int x, int y, int bID){
        int oned = y*15 + x;
        byte[] bytes = model.bytes;
        model.bytes[oned] = (byte)bID;
        //Debug.Log("Set at: " + oned + " equals " + model.bytes[oned]);
        model.bytes = bytes;
    }
    

}
