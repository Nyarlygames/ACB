using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PosArrowController : MonoBehaviour {

    public Sprite ArrowNormal;
    public Sprite ArrowHighlight;
    public Sprite ArrowClick;
    public SpriteRenderer ArrowRenderer;
    public Transform ArrowTransform;
    public int dir = 0;

    void Awake ()
    {
        ArrowRenderer = gameObject.GetComponent<SpriteRenderer>();
        ArrowNormal = Resources.Load<Sprite>("Touche/arrow_normal");
        ArrowHighlight = Resources.Load<Sprite>("Touche/arrow_highlight");
        ArrowClick = Resources.Load<Sprite>("Touche/arrow_click");
        Button temp = gameObject.GetComponent<Button>();
        //temp.onClick.AddListener(Back_Click);
    }

    void OnMouseEnter()
    {
        Debug.Log("la");
        ArrowRenderer.sprite = ArrowHighlight;
        if (Input.GetMouseButtonDown(0))
        {
            ArrowRenderer.sprite = ArrowClick;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("la");
        ArrowRenderer.sprite = ArrowHighlight;
        if (Input.GetMouseButtonDown(0))
        {
            ArrowRenderer.sprite = ArrowClick;
        }
    }

    void OnMouseExit()
    {
        ArrowRenderer.sprite = ArrowNormal;
    }


    void Update () {
		
	}
}
