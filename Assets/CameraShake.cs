using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 resetPos;
    public void Start(){
        resetPos = transform.localPosition;
    }
   public IEnumerator Shake (float duration, float magnitude){
    Vector3 originalPos = transform.localPosition;
    float elapsed = 0.0f;
    while(elapsed < duration){
        float x = Random.Range(-1f, 1f) * magnitude;
        float y = Random.Range(-1f, 1f) * magnitude;
        transform.localPosition = new Vector3(x, y, originalPos.z);
        elapsed += Time.deltaTime;
        yield return null;
    }
    transform.localPosition = originalPos;
   }

    public void posReset(){
        transform.localPosition = resetPos;
    }
}
