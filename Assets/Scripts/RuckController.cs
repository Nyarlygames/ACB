using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuckController : MonoBehaviour {

    GameObject PauseUI;
    GameObject HelpUI;
    Transform TCamera;
    Transform TBall;
    Transform TRuck;

    List<SpawnerController> Spawners = new List<SpawnerController>();
    public SpawnerController initSpawn;
    List<PositionController> Positions = new List<PositionController>();
    public PositionController initPos;

    public bool phase0 = false; // wait for annonce
    public bool phase1 = false; // place players
    public bool phase2 = false; // premiere passe
    public bool phase3 = false; // deuxieme passe


    void Start () {
        PauseUI = GameObject.Find("UI_Pause");
        HelpUI = GameObject.Find("UI_Help");
        TCamera = GameObject.Find("Camera").GetComponent<Transform>();
        TBall = GameObject.Find("Ball").GetComponent<Transform>();
        TRuck = GameObject.Find("Ruck").GetComponent<Transform>();

        PauseUI.SetActive(false);
        foreach (GameObject spawn in GameObject.FindGameObjectsWithTag("Spawner"))
        {
            Spawners.Add(spawn.GetComponent<SpawnerController>());
        }

        foreach (GameObject pos in GameObject.FindGameObjectsWithTag("Position"))
        {
            pos.GetComponent<PositionController>().pos = Positions.Count;
            Positions.Add(pos.GetComponent<PositionController>());
            pos.SetActive(false);
        }
        initRuck();

    }

    void initRuck()
    {
        initSpawn = Spawners[Random.Range(0, Spawners.Count)];
        if (initSpawn != null)
            initSpawn.SetObjects(TCamera, TRuck, TBall);

        initPos = Positions[Random.Range(0, Positions.Count)];
        if (initPos != null)
        {
            initPos.gameObject.SetActive(true);
            initPos.SetObjects(initSpawn.direction, initSpawn.transform.position);
        }
    }

	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (PauseUI.activeSelf == false) && (HelpUI.activeSelf == false)) // open pause
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0.0f;
        }


        if (Input.GetKeyDown(KeyCode.LeftArrow) && (PauseUI.activeSelf == false) && (HelpUI.activeSelf == false)) // open pause
        {
            if (initPos.pos != 0)
            {
                initPos = Positions[initPos.pos - 1];
                initPos.gameObject.SetActive(true);
                Positions[initPos.pos + 1].gameObject.SetActive(false);
                if (initPos.set == false)
                    initPos.SetObjects(initSpawn.direction, initSpawn.transform.position);
            }
            else
            {
                initPos = Positions[Positions.Count - 1];
                initPos.gameObject.SetActive(true);
                Positions[0].gameObject.SetActive(false);
                if (initPos.set == false)
                    initPos.SetObjects(initSpawn.direction, initSpawn.transform.position);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && (PauseUI.activeSelf == false) && (HelpUI.activeSelf == false)) // open pause
        {
            if (initPos.pos != Positions.Count - 1)
            {
                initPos = Positions[initPos.pos + 1];
                initPos.gameObject.SetActive(true);
                Positions[initPos.pos - 1].gameObject.SetActive(false);
                if (initPos.set == false)
                    initPos.SetObjects(initSpawn.direction, initSpawn.transform.position);
            }
            else
            {
                initPos = Positions[0];
                initPos.gameObject.SetActive(true);
                Positions[Positions.Count-1].gameObject.SetActive(false);
                if (initPos.set == false)
                    initPos.SetObjects(initSpawn.direction, initSpawn.transform.position);
            }
        }

        if (phase0 == false) // annonce
        {
        }
    }

    public void Retry()
    {

    }
}
