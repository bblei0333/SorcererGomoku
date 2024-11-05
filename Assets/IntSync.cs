using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class IntSync : RealtimeComponent<IntSyncModel>{
    private int syncedint;
    void Update(){
    if (Input.GetMouseButtonDown(0)){
        syncedint++;
        Debug.Log(syncedint);
    }
    }
    protected override void OnRealtimeModelReplaced(IntSyncModel previousModel, IntSyncModel currentModel){
        if(previousModel != null){
            previousModel.intDidChange -= IntDidChange;
        }
        if (currentModel != null) {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
                currentModel._testnum = syncedint;

            // Update the mesh render to match the new model
            UpdateValue();

            // Register for events so we'll know if the color changes later
            currentModel.intDidChange += IntDidChange;
        }

    }
        private void IntDidChange(IntSyncModel model, int value) {
        // Update the mesh renderer
        UpdateValue();
    }
    private void UpdateValue() {
        // Get the color from the model and set it on the mesh renderer.
        syncedint = model._testnum;
    }
    public void SetValue(int num) {
        // Set the color on the model
        // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
        model._testnum = num;
    }

    

}
