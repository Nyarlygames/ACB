using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionController : MonoBehaviour {

    public int pos;
    public string posname;
    public bool set = false;
    void Start ()
    {
    }
	
	void Update () {
	}

    public void SetObjects(int direction, Vector3 spawnPos)// 1 left 2 right
    {
        if (direction == 1)
        {
            foreach(Transform child in gameObject.transform)
            {
                child.position = new Vector3(spawnPos.x - child.position.x, spawnPos.y + child.position.y, spawnPos.z + child.position.z - 2.0f);
            }
        }
        else if (direction == 2)
        {
            foreach (Transform child in gameObject.transform)
            {
                child.position = new Vector3(spawnPos.x + child.position.x, spawnPos.y + child.position.y, spawnPos.z + child.position.z - 2.0f);
            }
        }
        else
            Debug.Log("error position in setobject position");
        set = true;
    }
}
