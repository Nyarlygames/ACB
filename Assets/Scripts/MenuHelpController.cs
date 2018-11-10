using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHelpController : MonoBehaviour {

    GameObject AllUI;
    public GameObject PauseUI;
	void Start ()
    {
        AllUI = GameObject.Find("UI");
      //  PauseUI = GameObject.Find("UIPause");
        Button temp = GameObject.Find("B_HelpClose").GetComponent<Button>();
        temp.onClick.AddListener(Close_Click);
        AllUI.SetActive(false);
    }
	
	void Update () {

    }

    public void OnSetActive()
    {
        AllUI.SetActive(false);
    }

    void Close_Click()
    {
        if (PauseUI.activeSelf == false)
            Time.timeScale = 1.0f;
        gameObject.SetActive(false);
        AllUI.SetActive(true);
    }
}
