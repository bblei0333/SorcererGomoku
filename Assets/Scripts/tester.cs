using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class tester : RealtimeComponent
{
    public int testint = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            testint++;
        }
        if(Input.GetMouseButtonDown(1))
        {
            Debug.Log(testint);
        }
    }
}
