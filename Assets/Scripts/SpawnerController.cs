using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour {

    public int direction = 0; // 1 left 2 right

    void Start () {
		
	}
	
    public void SetObjects(Transform cam, Transform ruck, Transform ball)
    {
        if (direction == 2)
            cam.position = gameObject.transform.position + new Vector3(5.0f, 15.0f, -25.0f);
        else
            cam.position = gameObject.transform.position + new Vector3(-5.0f, 15.0f, -25.0f);
        ruck.position = gameObject.transform.position;
       // ball.position = gameObject.transform.position + new Vector3(0.0f,-1.0f,0.0f);
    }

    void Update()
    {
    }
}
