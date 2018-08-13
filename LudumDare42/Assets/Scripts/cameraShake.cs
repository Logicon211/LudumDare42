using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraShake : MonoBehaviour {
  public float shakeStrength = 5;
    public float shake = 1;
   
    Vector3 originalPosition;
	// Use this for initialization
	void Start () {
		//originalPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void LateUpdate()
    {
        // if(Input.GetKeyDown(KeyCode.Q))
        // {
        //     shake = shakeStrength;
        // }
       
        Camera.main.transform.localPosition = (Random.insideUnitSphere * shake);
       
        shake = Mathf.MoveTowards(shake, 0, Time.deltaTime * shakeStrength);
       
        if(shake == 0)
        {
            //Camera.main.transform.localPosition = originalPosition;
        }
    }
	
}