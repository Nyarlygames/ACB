using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MenuPauseController : MonoBehaviour {

    public string SceneBack = "";
    GameObject AllUI;
    public GameObject HelpUI;

    void Start () {

        AllUI = GameObject.Find("UI");
     //   HelpUI = GameObject.Find("UIHelp");
        Button temp = GameObject.Find("B_BackMain").GetComponent<Button>();
        temp.onClick.AddListener(Back_Click);
        temp = GameObject.Find("B_Resume").GetComponent<Button>();
        temp.onClick.AddListener(Resume_Click);
        temp = GameObject.Find("B_OpenHelp").GetComponent<Button>();
        temp.onClick.AddListener(Help_Click);
    }
	
	void Update () {

    }

    void Help_Click()
    {
        AllUI.SetActive(false);
        HelpUI.SetActive(true);
        HelpUI.GetComponent<MenuHelpController>().OnSetActive();
    }

    void Back_Click()
    {
        SceneManager.LoadScene(SceneBack, LoadSceneMode.Single);
    }
    void Resume_Click()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }
}
