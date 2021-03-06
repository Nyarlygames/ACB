﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRuckController : MonoBehaviour {
    RuckController RuckControl;
    public MeshRenderer MR;
    public string pname;
    public bool selected = false;

	void Awake () {
        MR = gameObject.GetComponent<MeshRenderer>();
        RuckControl = GameObject.Find("GameController").GetComponent<RuckController>();
	}
	
	void Update () {
		
	}

    public void SetSelected()
    {
        if (RuckControl.phase2 == false)
        {
            if (RuckControl.SelectedPlayer1 != null)
                RuckControl.SelectedPlayer1.UnSelect();
            selected = true;
            RuckControl.SelectedPlayer1 = this;
            MR.material.color = Color.green;
        }
        else
        {
            if (RuckControl.SelectedPlayer2 != null)
                RuckControl.SelectedPlayer2.UnSelect();
            selected = true;
            RuckControl.SelectedPlayer2 = this;
            MR.material.color = Color.green;
        }
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
        if ((selected != true) && (RuckControl.phase1 == true))
        {
            MR.material.color = Color.blue;
            if (Input.GetMouseButtonDown(0))
            {
                SetSelected();
            }
        }
    }
}
