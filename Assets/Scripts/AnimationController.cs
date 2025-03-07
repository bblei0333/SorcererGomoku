using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject bp, wp, sp, flip1, flip2, tempthing;
    private Renderer pieceRenderer;
    private Vector3 Posi;
    // Start is called before the first frame update
    public void DoAFlip(int p1, int p2, Vector3 pos)
    {
        GameObject b1, b2;
        b1 = null;
        b2 = null;
        if(p1 == 1){b1 = wp;};
        if(p1 == 2){b1 = bp;};
        if(p1 == 3){b1 = sp;};
        if(p2 == 1){b2 = wp;};
        if(p2 == 2){b2 = bp;};
        if(p2 == 3){b2 = sp;};
        flip2 = b2;
        flip1 = Instantiate(b1, pos, Quaternion.identity);
        Animation animator1 = flip1.GetComponent<Animation>();
        animator1.Play("FlipUp");
        StartCoroutine(Waiter());
    }

    IEnumerator Waiter(){
        flip2 = Instantiate(flip2,flip1.transform.position, flip1.transform.rotation);
        Destroy(flip1);
        Animation animator2 = flip2.GetComponent<Animation>();
        animator2.Play("FlipDown");
        if(GameObject.Find("Normy").GetComponent<Spawner>().ID != GameObject.Find("Normy").GetComponent<IntSync>().LPlayer){GameObject.Find("Normy").GetComponent<IntSync>().SetAnimation(0);}
        yield return new WaitForSeconds(0.8f);
        GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().OngoingAnimation = false;
        GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().AnimationOver(1);
        yield return new WaitForSeconds(0.8f);
        Destroy(flip2);
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
