using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuAvantController : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Button temp = GameObject.Find("B_Touche").GetComponent<Button>();
        temp.onClick.AddListener(Touche_Click);
        temp = GameObject.Find("B_Ruck").GetComponent<Button>();
        temp.onClick.AddListener(Ruck_Click);
        temp = GameObject.Find("B_Back").GetComponent<Button>();
        temp.onClick.AddListener(Back_Click);
    }
	
	// Update is called once per frame
	void Update () {

    }
    void Touche_Click()
    {
        SceneManager.LoadScene("Game_Avant_Touche", LoadSceneMode.Single);
    }
    void Ruck_Click()
    {
        SceneManager.LoadScene("Game_Avant_Ruck", LoadSceneMode.Single);
    }
    void Back_Click()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
