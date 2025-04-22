using System.Collections;
using Normal.Realtime;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject bp, wp, sp, flip1, flip2, tempthing, h1, h2, fist, palm;
    private string str2;
    public bool flipped;
    
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
        if(flipped || flip1 != null) return;
        flipped = true;
        
        string b1 = GetPrefabName(p1);
        str2 = GetPrefabName(p2);
        
        Debug.Log($"Instantiating flip at position: {pos}");

        // Instantiate parent container
        GameObject flipParent1 = new GameObject("FlipParent");
        flipParent1.transform.position = pos;
        
        // Instantiate animated child with offset
        flip1 = Realtime.Instantiate(b1, Vector3.zero, Quaternion.identity, option);
        flip1.transform.SetParent(flipParent1.transform);
        flip1.transform.localPosition = new Vector3(0, 0.2f, 0); // Your original offset
        
        Animation animator1 = flip1.GetComponent<Animation>();
        animator1.Play("FlipUp");
        
        StartCoroutine(Waiter(flipParent1, pos));
    }

    IEnumerator Waiter(GameObject parent1, Vector3 pos)
    {
        yield return new WaitForSeconds(1f);
        
        // Create second parent container
        GameObject flipParent2 = new GameObject("FlipParent");
        flipParent2.transform.position = pos;
        
        // Instantiate second animated child
        flip2 = Realtime.Instantiate(str2, Vector3.zero, Quaternion.identity, option);
        flip2.transform.SetParent(flipParent2.transform);
        flip2.transform.localPosition = new Vector3(0, 0.2f, 0); // Same offset
        
        // Clean up first flip
        Destroy(parent1);
        Realtime.Destroy(flip1);
        
        Animation animator2 = flip2.GetComponent<Animation>();
        animator2.Play("FlipDown");
        
        yield return new WaitForSeconds(0.8f);
        
        GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().AnimationOver(1);
        Destroy(flipParent2);
        Realtime.Destroy(flip2);
        flipped = false;
    }

    private string GetPrefabName(int pieceType)
    {
        return pieceType switch
        {
            1 => "AnimatedWhite",
            2 => "AnimatedBlack",
            3 => "AnimatedShare",
            _ => null
        };
    }

    void Start(){}
    void Update(){}
}