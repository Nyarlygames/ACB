using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public PlayerRuckController SelectedPlayer;

    public List<string> Annonces = new List<string>();
    Text TAnnonce;
    Text TResult;
    Text TTimer;
    Text TLog;
    string SelectedAnnonce = "";

    public bool phase0 = false; // annonce
    public bool phase1 = false; // position
    public bool phase2 = false; // premiere passe
    public bool phase3 = false; // deuxieme passe

    public float timerPhase0 = 0.0f;
    public float timerPhase1 = 0.0f;
    public float timerPhase2 = 0.0f;
    public float timerPhase3 = 0.0f;

    float initPhase0 = 3.0f;
    float initPhase1 = 10.0f;
    float initPhase2 = 5.0f;
    float initPhase3 = 5.0f;


    void Awake () {
        PauseUI = GameObject.Find("UI_Pause");
        HelpUI = GameObject.Find("UI_Help");
        TCamera = GameObject.Find("Camera").GetComponent<Transform>();
        TBall = GameObject.Find("Ball").GetComponent<Transform>();
        TRuck = GameObject.Find("Ruck").GetComponent<Transform>();

        TAnnonce = GameObject.Find("T_Annonce").GetComponent<Text>();
        TResult = GameObject.Find("T_LastResult").GetComponent<Text>();
        TTimer = GameObject.Find("T_Timers").GetComponent<Text>();
        TLog = GameObject.Find("T_Log").GetComponent<Text>();

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
        //initRuck();
        initAnnonce();
        Time.timeScale = 0.0f;
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

    void initAnnonce()
    {
        Annonces.Add("Zero");
        Annonces.Add("Black");
        Annonces.Add("Soutien");
        Annonces.Add("Appuie");
        Annonces.Add("10");
        timerPhase0 = initPhase0;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && (PauseUI.activeSelf == false) && (HelpUI.activeSelf == false)) // open pause
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0.0f;
        }

        if (phase0 == false)
        {

            /*if (lastresult == 1)
                T_Last.color = Color.green;
            else
                T_Last.color = Color.red;
            if (lastresult == 1)
                T_Last.text = "Touche réussie !";
            else if (lastresult == 2)
                T_Last.text = "Echec : Mauvais sauteur.";
            else if (lastresult == 3)
                T_Last.text = "Echec : Mauvaise position.";
            else if (lastresult == 4)
                T_Last.text = "Echec : Mauvaise combinaison.";*/
            TTimer.text = ((int)timerPhase0 + 1).ToString();
            timerPhase0 -= Time.deltaTime;
            if ((timerPhase0 <= 0.0f) && (phase0 == false))
            {
                phase0 = true;
                TResult.color = Color.black;
                TResult.text = "";
                StartAnnounce();
            }
        }
        else if (phase1 == false) // select pos
        {
            TTimer.text = ((int)timerPhase1 + 1).ToString();
            timerPhase1 -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.LeftArrow) && (PauseUI.activeSelf == false) && (HelpUI.activeSelf == false)) // previous pos
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

            if (Input.GetKeyDown(KeyCode.RightArrow) && (PauseUI.activeSelf == false) && (HelpUI.activeSelf == false)) // next pos
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
                    Positions[Positions.Count - 1].gameObject.SetActive(false);
                    if (initPos.set == false)
                        initPos.SetObjects(initSpawn.direction, initSpawn.transform.position);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space) && (PauseUI.activeSelf == false) && (HelpUI.activeSelf == false) || (timerPhase1 <= 0.0f) && (phase1 == false))
            {
                CheckPhase1();
                if (phase1 == false)
                    Retry();
                else
                    timerPhase2 = initPhase2;
            }
        }
        else if (phase2 == false) // select player
        {
            TTimer.text = ((int)timerPhase2 + 1).ToString();
            timerPhase2 -= Time.deltaTime;
            if ((timerPhase2 <= 0.0f) && (phase2 == false))
            {
            }
        }
    }

    void StartAnnounce()
    {
        SelectedAnnonce = Annonces[Random.Range(0, Annonces.Count)];
        TAnnonce.text = SelectedAnnonce + " !"; 
        initRuck();
        timerPhase1 = initPhase1;
    }

    public void CheckPhase1()
    {
        Debug.Log(initPos.name + " / " + SelectedAnnonce);
        switch (SelectedAnnonce)
        {
            case "Zero":
                if (string.Compare(initPos.name,"0") == 0)
                    phase1 = true;
                break;
            case "Black":
                if (string.Compare(initPos.name, "Black") == 0)
                    phase1 = true;
                break;
            case "Soutien":
                if (string.Compare(initPos.name, "Soutien") == 0)
                    phase1 = true;
                break;
            case "Appuie":
                if (string.Compare(initPos.name, "Appuie") == 0)
                    phase1 = true;
                break;
            case "10":
                if (string.Compare(initPos.name, "10") == 0)
                    phase1 = true;
                break;
            default:
                Debug.Log("failed announce at phase 1");
                break;
        }

    }
    public void Retry()
    {
        Debug.Log("retry");

    }
}
