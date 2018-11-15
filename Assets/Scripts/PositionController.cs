using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionController : MonoBehaviour {

    public int pos;
    public string posname;
    public bool set = false;

    Dictionary<string,Vector3> ChildInit = new Dictionary<string,Vector3>();
    void Awake ()
    {
    }

    public void SetInit(Transform TRuck)
    {
        foreach (Transform child in gameObject.transform)
        {
            ChildInit.Add(child.gameObject.name,TRuck.position - child.position);
        }
    }
	
	void Update () {
	}

    public void SetObjects(int direction, Vector3 spawnPos)// 1 left 2 right
    {
        if (direction == 1)
        {
            foreach(Transform child in gameObject.transform)
            {
                child.position = new Vector3(spawnPos.x + ChildInit[child.name].x, spawnPos.y - ChildInit[child.name].y, spawnPos.z - ChildInit[child.name].z);
            }
        }
        else if (direction == 2)
        {
            foreach (Transform child in gameObject.transform)
            {
                child.position = new Vector3(spawnPos.x - ChildInit[child.name].x, spawnPos.y - ChildInit[child.name].y, spawnPos.z - ChildInit[child.name].z);
            }
        }
        else
            Debug.Log("error position in setobject position");
        set = true;
    }
}
