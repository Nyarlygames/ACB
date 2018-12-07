using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PosArrowController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

    public Sprite ArrowNormal;
    public Sprite ArrowHighlight;
    public Sprite ArrowClick;
    public Image ArrowRenderer;
    public Transform ArrowTransform;
    RuckController RC;
    public int dir = 0;

    void Awake ()
    {
        ArrowRenderer = gameObject.GetComponent<Image>();
        ArrowNormal = Resources.Load<Sprite>("Touche/arrow_normal");
        ArrowHighlight = Resources.Load<Sprite>("Touche/arrow_highlight");
        ArrowClick = Resources.Load<Sprite>("Touche/arrow_click");
        RC = GameObject.Find("GameController").GetComponent<RuckController>();
        Button temp = gameObject.GetComponent<Button>();
        temp.onClick.AddListener(PosMove);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        ArrowRenderer.sprite = ArrowHighlight;
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        ArrowRenderer.sprite = ArrowNormal;
    }

    public void PosMove()
    {
        if (!Input.GetKeyDown(KeyCode.Space) && (RC.PauseUI.activeSelf == false) && (RC.HelpUI.activeSelf == false) && (dir == -1))
        {
            RC.PreviousPos();
        }
        if (!Input.GetKeyDown(KeyCode.Space) && (RC.PauseUI.activeSelf == false) && (RC.HelpUI.activeSelf == false) && (dir == 1))
        {
            RC.NextPos();
        }
    }

}
