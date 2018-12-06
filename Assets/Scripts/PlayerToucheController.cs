using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToucheController : MonoBehaviour {

    public int pos = 0;
    public int side = 0;
    public int command = 0;
    public float upPos = 2.0f;
    public Transform PTransform;
    public Vector3 initPos;
    // public SpriteRenderer PSprite;
    public MeshRenderer MR;
    public float gap = 0;
    ToucheController TouchControl;
    public bool selected = false;
    
	void Awake ()
    {
        MR = gameObject.GetComponent<MeshRenderer>();
        TouchControl = GameObject.Find("GameController").GetComponent<ToucheController>();
        PTransform = gameObject.GetComponent<Transform>();
       // PSprite = gameObject.GetComponent<SpriteRenderer>();
    }
	
	void Update () {
		
	}

    public void SetUp()
    {
        //PSprite.sprite = TouchControl.Up;
        if (side == 1)
            TouchControl.PlayersControllers.Find(ptc => ptc.pos == pos - 1).MoveLift(side);
        if (side == -1)
            TouchControl.PlayersControllers.Find(ptc => ptc.pos == pos + 1).MoveLift(side);


        if (side == 0)
        {
            TouchControl.PlayersControllers.Find(ptc => ptc.pos == pos - 1).PTransform.position += new Vector3(gap, 0.0f, 0.0f); // new Vector3(PTransform.position.x + gap * 1, PTransform.position.y, PTransform.position.z);
            TouchControl.PlayersControllers.Find(ptc => ptc.pos == pos + 1).PTransform.position += new Vector3(-gap, 0.0f, 0.0f); //new Vector3(PTransform.position.x + gap * -1, PTransform.position.y, PTransform.position.z);

        }

       // TouchControl.PlayersControllers.Find(ptc => ptc.pos == pos - 1).PSprite.sprite = TouchControl.Lift;
        TouchControl.PlayersControllers.Find(ptc => ptc.pos == pos - 1).PTransform.Rotate(new Vector3(0.0f, -180.0f, 0.0f));
       // TouchControl.PlayersControllers.Find(ptc => ptc.pos == pos + 1).PSprite.sprite = TouchControl.Lift;
        PTransform.position = new Vector3(PTransform.position.x, PTransform.position.y + upPos, PTransform.position.z);
    }

    public void SetPlace(float startX, float startY, float startZ, float maxX, int size)
    {
        Vector3 newPlace = new Vector3();
        newPlace.y = startY;
        newPlace.z = startZ;
        newPlace.x = ((maxX - startX) / (size-1)) * pos;
        gap = (((maxX - startX) / (size - 1)) - gameObject.transform.localScale.x) / 2;
        initPos = newPlace;
        PTransform.position = newPlace;
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && (TouchControl.lockedPlacement == false)){
            PlayerToucheController LastSelected = TouchControl.PlayersControllers.Find(ptc => ptc.selected == true);
            if (LastSelected != null)
                LastSelected.SetUnselected();
            SetSelected();
        }
    }

    public void SetUnselected()
    {
        if (side == -1)
            PTransform.position = new Vector3(PTransform.position.x + gap, PTransform.position.y, PTransform.position.z);
        if (side == 1)
            PTransform.position = new Vector3(PTransform.position.x - gap, PTransform.position.y, PTransform.position.z);
        side = 0;
        MR.material.color = Color.black;
        selected = false;
    }

    void SetSelected()
    {
        TouchControl.ArrowLeft.gameObject.SetActive(true);
        TouchControl.ArrowRight.gameObject.SetActive(true);
        TouchControl.ArrowLeft.PTC = this;
        TouchControl.ArrowRight.PTC = this;
        TouchControl.ArrowLeft.setPos();
        TouchControl.ArrowRight.setPos();
        TouchControl.PTC = this;
        MR.material.color = Color.blue;
        selected = true;
    }

    public void MoveLift(int dir)
    {
        PTransform.position = new Vector3(PTransform.position.x + gap*2 * dir, PTransform.position.y, PTransform.position.z);
    }

    public void MovePlayer(int dir)
    {
        if (((side == -1) && (dir != -1)) || ((side == 1) && (dir != 1)) || (side == 0))
        {
            PTransform.position = new Vector3(PTransform.position.x + gap * dir, PTransform.position.y, PTransform.position.z);
            side = side + dir;
        }
    }

    public void ResetUp()
    {
       // TouchControl.PlayersControllers.Find(ptc => ptc.pos == pos - 1).PSprite.sprite = TouchControl.Lift;
      //  TouchControl.PlayersControllers.Find(ptc => ptc.pos == pos + 1).PSprite.sprite = TouchControl.Lift;
       // PSprite.sprite = TouchControl.Up;
    }

    public void SetDown()
    {
       // TouchControl.PlayersControllers.Find(ptc => ptc.pos == pos - 1).PSprite.sprite = TouchControl.Normal;
       // TouchControl.PlayersControllers.Find(ptc => ptc.pos == pos + 1).PSprite.sprite = TouchControl.Normal;
       // PSprite.sprite = TouchControl.Keep;
    }

    public void MakeCommand(int cmd)
    {
        command = cmd;
        if (command == 1) // sol
        {
            SetDown();
            PTransform.position = new Vector3(initPos.x + gap * side, initPos.y, initPos.z);
            TouchControl.Ball.transform.position = PTransform.position + new Vector3(0.0f, TouchControl.ballDown, -0.1f);
            TouchControl.timerUp = 0.0f;
        }
        if (command == 2) // run
        {
            ResetUp();
            PTransform.position = new Vector3(initPos.x + gap * side, initPos.y + upPos, initPos.z);
            TouchControl.Ball.transform.position = TouchControl.Ten.transform.position + new Vector3(0.0f, TouchControl.ballDown, -0.1f);
            TouchControl.timerUp = 0.0f;
        }


        if (command == 3) // drive
        {
            TouchControl.Ball.transform.position = PTransform.position + new Vector3(0.0f, TouchControl.ballDown, -0.1f);
            TouchControl.timerDown = 0.0f;
        }
        if (command == 4) // sort
        {
            TouchControl.Ball.transform.position = TouchControl.Ten.transform.position + new Vector3(0.0f, TouchControl.ballDown, -0.1f);
            TouchControl.timerDown = 0.0f;
        }
    }
}
