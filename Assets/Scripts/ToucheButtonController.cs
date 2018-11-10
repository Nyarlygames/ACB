using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToucheButtonController : MonoBehaviour {

    public int command = 0; // 0 = rien, 1 sol, 2 passe, 3 garde, 4 sort

    public SpriteRenderer ButtonRenderer;
    public Transform ButtonTransform;
    public PlayerToucheController PTC;
    public Sprite Button_Normal;
    public Sprite Button_Highlight;
    public Sprite Button_Click;

    void Awake () {
        ButtonTransform = gameObject.GetComponent<Transform>();
        ButtonRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
	
	void Update () {
		
	}



    void OnMouseOver()
    {
        ButtonRenderer.sprite = Button_Highlight;
        if (Input.GetMouseButtonDown(0))
        {
            ButtonRenderer.sprite = Button_Click;
            if (PTC != null)
                PTC.MakeCommand(command);
        }
    }
    void OnMouseExit()
    {
        ButtonRenderer.sprite = Button_Normal;
    }



    public void setPos()
    {
        if ((command == 2) || (command == 4))
            ButtonTransform.position = PTC.PTransform.position + new Vector3(1.2f, 3.5f, 0.0f);
        else
            ButtonTransform.position = PTC.PTransform.position + new Vector3(-1.2f, 3.5f, 0.0f);
    }
}
