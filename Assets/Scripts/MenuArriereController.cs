using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuArriereController : MonoBehaviour {


	// Use this for initialization
	void Start ()
    {
        Button temp = GameObject.Find("B_Back").GetComponent<Button>();
        temp.onClick.AddListener(Back_Click);
    }
	
	// Update is called once per frame
	void Update () {

    }
    void Back_Click()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
