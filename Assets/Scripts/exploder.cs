using UnityEngine;
using System.Collections;

// Applies an explosion force to all nearby rigidbodies
public class ExampleClass : MonoBehaviour
{
    public float radius = 100.0F;
    public float power = 500.0F;

    public bool boom = true;

    

    void Update(){
        //Debug.Log(Input.mousePosition);
        /*
        Vector3 mousePos = Input.mousePosition;
        Vector3 explosionPos = mousePos;
        if (Input.GetKeyDown(KeyCode.F)){
            
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 1.0F);
        }
             
        }
        */
        if(boom){
            boom = false;
            //StartCoroutine(Shit());

        }
    }
    void Start()
    {
        //StartCoroutine(Shit());

        
    }
    IEnumerator Shit(Vector3 position){
        yield return new WaitForSeconds(0.5f);
        boom = true;
        Vector3 explosionPos = Input.mousePosition;
        explosionPos.z = 1;
        Debug.Log(explosionPos);
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 0.001F);
                Debug.Log("BOOM");
        }
        
        
    }

    public void Boomer(Vector3 position){
        Debug.Log("boomer called");
        StartCoroutine(Shit(position));
    }
}

