using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRuckController : MonoBehaviour {
    RuckController RuckControl;
    MeshRenderer MR;
    bool selected = false;

	void Awake () {
        MR = gameObject.GetComponent<MeshRenderer>();
        RuckControl = GameObject.Find("GameController").GetComponent<RuckController>();
	}
	
	void Update () {
		
	}

    public void SetSelected()
    {
        if (RuckControl.SelectedPlayer != null)
            RuckControl.SelectedPlayer.UnSelect();
        selected = true;
        RuckControl.SelectedPlayer = this;
        MR.material.color = Color.green;
    }

    public void UnSelect()
    {
        MR.material.color = Color.black;
        selected = false;
    }

    void OnMouseExit()
    {
        if (selected == false)
            MR.material.color = Color.black;
        else
            MR.material.color = Color.green;
    }

    void OnMouseOver()
    {
        if ((selected != true) && (RuckControl.phase2 == true))
        {
            MR.material.color = Color.blue;
            if (Input.GetMouseButtonDown(0))
            {
                SetSelected();
            }
        }
    }
}
