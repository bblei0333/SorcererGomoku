using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using Normal;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    private int ownerIDSelf {get;}
    public void doSpawn(int num, Vector3 spot, Quaternion rot){
        if(num == 1){
        Realtime.Instantiate("Pfabbp", spot, rot);
        for (int i = 14; i >= 0; i--)
        {
            Debug.Log(GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().grinfo[0, i] + GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().grinfo[1, i] + GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().grinfo[2, i] + GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().grinfo[3, i] + GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().grinfo[4, i] + GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().grinfo[5, i] + GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().grinfo[6, i] + GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().grinfo[7, i] + GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().grinfo[8, i] + GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().grinfo[9, i] + GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().grinfo[10, i] + GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().grinfo[11, i] + GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().grinfo[12, i] + GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().grinfo[13, i] + GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().grinfo[14, i] + " ");
        }
        }
        else{
        Realtime.Instantiate("Pfabwp", spot, rot);}
        
    
    }
    void Start()
    {
    }

    void Awake()
    {
        Debug.Log("Client ID: " + ownerIDSelf);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
