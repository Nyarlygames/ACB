using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToucheArrowController : MonoBehaviour
{

    public int dir = 0;
    public PlayerToucheController PTC;
    public Sprite ArrowNormal;
    public Sprite ArrowHighlight;
    public Sprite ArrowClick;
    public SpriteRenderer ArrowRenderer;
    public Transform ArrowTransform;

    void Awake()
    {
        ArrowTransform = gameObject.GetComponent<Transform>();
        ArrowRenderer = gameObject.GetComponent<SpriteRenderer>();
        ArrowNormal = Resources.Load<Sprite>("Touche/arrow_normal");
        ArrowHighlight = Resources.Load<Sprite>("Touche/arrow_highlight");
        ArrowClick = Resources.Load<Sprite>("Touche/arrow_click");
    }

    void Update()
    {

    }


    void OnMouseOver()
    {
        ArrowRenderer.sprite = ArrowHighlight;
        if (Input.GetMouseButtonDown(0))
        {
            ArrowRenderer.sprite = ArrowClick;
            if (PTC != null)
                PTC.MovePlayer(dir);
        }
    }
    void OnMouseExit()
    {
        ArrowRenderer.sprite = ArrowNormal;
    }

    public void setPos()
    {
        if (dir == 1)
            ArrowTransform.position = PTC.PTransform.position + new Vector3(2.75f, 3.5f, 0.0f);
        else
            ArrowTransform.position = PTC.PTransform.position + new Vector3(-2.75f, 3.5f, 0.0f);
    }
}
