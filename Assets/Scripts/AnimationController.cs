using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject bp,wp, sp, flip1, flip2, tempthing;
    public Animation animator1;
    private Renderer pieceRenderer;
    // Start is called before the first frame update
    public void DoAFlip(int p1, int p2, Vector3 pos)
    {
        if(p1 == 1){p1 = wp};
        if(p1 == 2){p1 = bp};
        if(p1 == 3){p1 = sp};
        flip1 = Instantiate(p1, pos, Quaternion.identity);
        StartCoroutine(Waiter());
        animator1 = p1.GetComponent<Animation>();
        flip2 = p2;
    }

    IEnumerator Waiter(){
        yield return new WaitForSeconds(1f);
        flip2 = Instantiate(flip2,flip1.transform.position, flip1.transform.rotation);
        Destroy(flip1);
        Animation animator2 = flip2.GetComponent<Animation>();
        animator2.Play("FlipDown");
    }
    
    //public void OnAnimationEnd(){
    //}

    void Start(){
        DoAFlip(bp, wp, new Vector3(0,0,0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
