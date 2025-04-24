using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;
using Normal.Realtime;
using Normal.Realtime.Serialization;

public class AnimationController : MonoBehaviour
{
    public GameObject bp, wp, sp, flip1, flip2, tempthing, h1, h2, fist, palm;
    private string str2;
    public bool flipped;
    private Renderer pieceRenderer;
    private Vector3 Posi;
    // Start is called before the first frame update
    
Realtime.InstantiateOptions option = new Realtime.InstantiateOptions {
        ownedByClient = true,
        preventOwnershipTakeover = true
    };
    public void Grabbed(){
        palm = Instantiate(h1, Vector3.zero, Quaternion.identity);
        Animator animator3 = palm.GetComponent<Animator>();
        animator3.Play("palmgrab");
        StartCoroutine(Grabber());
    }
    
    IEnumerator Grabber(){
        yield return new WaitForSeconds(2);
        fist = Instantiate(h2, palm.transform.position, palm.transform.rotation);
        Destroy(palm);
        Animator animator4 = fist.GetComponent<Animator>();
        animator4.Play("fistgrab");
        yield return new WaitForSeconds(0.8f);
        GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().AnimationOver(2);
        Destroy(fist);
    }
    public void DoAFlip(int p1, int p2, Vector3 pos)
    {
        if(flipped){return;};
        flipped = true;
        string b1, b2;
        b1 = null;
        b2 = null;
        if(p1 == 1){b1 = "AnimatedWhite";};
        if(p1 == 2){b1 = "AnimatedBlack";};
        if(p1 == 3){b1 = "AnimatedShare";};
        if(p2 == 1){b2 = "AnimatedWhite";};
        if(p2 == 2){b2 = "AnimatedBlack";};
        if(p2 == 3){b2 = "AnimatedShare";};
        str2 = b2;
        Debug.Log(pos);
        flip1 = Realtime.Instantiate(b1,pos, Quaternion.identity, option);
        Animation animator1 = flip1.GetComponent<Animation>();
        animator1.Play("FlipUp");
        StartCoroutine(Waiter());
    }
    IEnumerator Waiter(){
        yield return new WaitForSeconds(100f);
        flip2 = Realtime.Instantiate(str2,flip1.transform.position, flip1.transform.rotation, option);
        Realtime.Destroy(flip1);
        Animation animator2 = flip2.GetComponent<Animation>();
        animator2.Play("FlipDown");
        yield return new WaitForSeconds(0.8f);
        GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().AnimationOver(1);
        Realtime.Destroy(flip2);
    }
    //public void OnAnimationEnd(){
    //}

    void Start(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
