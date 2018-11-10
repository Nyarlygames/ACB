using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	void Start () {

        Button temp = GameObject.Find("B_LaunchAvant").GetComponent<Button>();
        temp.onClick.AddListener(MenuAvant_Click);
        temp = GameObject.Find("B_LaunchArriere").GetComponent<Button>();
        temp.onClick.AddListener(MenuArriere_Click);
        temp = GameObject.Find("B_Exit").GetComponent<Button>();
        temp.onClick.AddListener(ExitButton_Click);
    }
	
	void Update () {
		
	}

    void MenuAvant_Click()
    {
        SceneManager.LoadScene("MenuAvant", LoadSceneMode.Single);
    }

    void MenuArriere_Click()
    {
        SceneManager.LoadScene("MenuArriere", LoadSceneMode.Single);
    }

    void ExitButton_Click()
    {
        Application.Quit();
    }
}
