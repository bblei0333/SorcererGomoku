using System.Collections;
using System.Collections.Generic;
using Normal.Realtime;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public GameObject bp, wp, sp, flip1, flip2, tempthing, h1, h2, fist, palm;
    private string str2;
    public bool flipped;
    private Renderer pieceRenderer;
    private Vector3 Posi;
    
    // References to complete prefabs
    public GameObject blackFlipPrefab;  // Bc with AnimatedBlack as child
    public GameObject whiteFlipPrefab;  // Wc with AnimatedWhite as child
    public GameObject shareFlipPrefab;  // Sc with AnimatedShare as child
    
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
        if(flipped) return;
        flipped = true;
        
        GameObject prefab1 = null;
        GameObject prefab2 = null;
        
        // Determine which prefab to use based on piece type
        if(p1 == 1) prefab1 = whiteFlipPrefab;
        if(p1 == 2) prefab1 = blackFlipPrefab;
        if(p1 == 3) prefab1 = shareFlipPrefab;
        
        if(p2 == 1) prefab2 = whiteFlipPrefab;
        if(p2 == 2) prefab2 = blackFlipPrefab;
        if(p2 == 3) prefab2 = shareFlipPrefab;
        
        if (prefab1 == null || prefab2 == null) {
            Debug.LogError("Flip prefab is null! Check your piece types: " + p1 + ", " + p2);
            return;
        }
        
        // Store the name of the second prefab for later use
        str2 = prefab2.name;
        
        // Instantiate the first prefab
        flip1 = Realtime.Instantiate(prefab1.name, pos, Quaternion.identity, option);
        
        // Use the AnimationStateSync component to play the animation on all clients
        AnimationStateSync animSync = flip1.GetComponent<AnimationStateSync>();
        if (animSync != null)
        {
            animSync.PlayFlipUp();
        }
        else
        {
            Debug.LogWarning("AnimationStateSync component not found on " + prefab1.name);
        }
        
        StartCoroutine(Waiter(pos));
    }
    
    IEnumerator Waiter(Vector3 pos)
    {
        yield return new WaitForSeconds(1.0f);
        
        // Instantiate the second prefab
        flip2 = Realtime.Instantiate(str2, pos, Quaternion.identity, option);
        
        // Use the AnimationStateSync component to play the animation on all clients
        AnimationStateSync animSync = flip2.GetComponent<AnimationStateSync>();
        if (animSync != null)
        {
            animSync.PlayFlipDown();
        }
        else
        {
            Debug.LogWarning("AnimationStateSync component not found on " + str2);
        }
        
        Realtime.Destroy(flip1);
        
        yield return new WaitForSeconds(0.8f);
        GameObject.Find("GomokuBoard").GetComponent<GomokuControl>().AnimationOver(1);
        Realtime.Destroy(flip2);
        flipped = false;
    }

    void Start(){
        // Validate prefab references
        if (blackFlipPrefab == null || whiteFlipPrefab == null || shareFlipPrefab == null) {
            Debug.LogError("One or more flip prefabs not assigned in the inspector. Please assign all prefabs.");
        }
    }

    void Update()
    {
        
    }
}