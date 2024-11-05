using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class PiecePool : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject bp;
    public GameObject wp;
    private GameObject slot1;
    private GameObject slot2;
    private GameObject slot3;
    private int pcount = 0;
    public int[] pool = new int[40];
    Random rnd = new Random();
    
    void Start()
    {
        for(int x=0; x<8;){
            int r = rnd.Next(0, 40);
            if(pool[r] == 0){
                pool[r] = 3;
                x++;
            }
        }
        for(int x=0; x<40; x++){
        Debug.Log(pool[x]);}
        slotView(0);
    }
    public GameObject setSlot(int num){
        if(pool[num] == 0){
            return bp;
        }
        else{
            return wp;
        }
    }
    void PiecePlaced(){
        pcount++;
        slotView(pcount);
    }
    public void slotView(int num){
        if(!slot1){
            //teehee
        }
        else{
            Destroy(slot1);
            Destroy(slot2);
            Destroy(slot3);
        }
        for(int x=(0+num);x<(3+num);x++){
            if(x - num == 0){
            slot1 = Instantiate(setSlot(x), new Vector3(6,0,(- 4 + (x-num))), Quaternion.identity);}
            if(x - num == 1){
            slot2 = Instantiate(setSlot(x), new Vector3(6,0,(- 4 + (x-num))), Quaternion.identity);}
            if(x - num == 2){
            slot3 = Instantiate(setSlot(x), new Vector3(6,0,(- 4 + (x-num))), Quaternion.identity);}
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
