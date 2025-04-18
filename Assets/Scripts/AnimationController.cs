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
    private Renderer pieceRenderer;
    private Vector3 Posi;
    // Start is called before the first frame update

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
    }

    IEnumerator Waiter(){
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
