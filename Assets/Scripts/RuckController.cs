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
    Vector3 InitBall;

    List<SpawnerController> Spawners = new List<SpawnerController>();
    public SpawnerController initSpawn;
    List<PositionController> Positions = new List<PositionController>();
    public PositionController initPos;
    public PlayerRuckController SelectedPlayer;

    public List<string> Annonces = new List<string>();
    Text TAnnonce;
    Text TAnnonce2;
    Text TResult;
    Text TTimer;
    Text TLog;
    string SelectedAnnonce = "";
    string SelectedModifier = "";

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

    int lastresult = 0; // 0 init, 1 won, 2 failed, ... ?


    void Awake () {
        PauseUI = GameObject.Find("UI_Pause");
        HelpUI = GameObject.Find("UI_Help");
        TCamera = GameObject.Find("Camera").GetComponent<Transform>();
        TBall = GameObject.Find("Ball").GetComponent<Transform>();
        TRuck = GameObject.Find("Ruck").GetComponent<Transform>();
        TRuck.gameObject.SetActive(false);
        InitBall = TRuck.position - TBall.position;

        TAnnonce = GameObject.Find("T_Annonce").GetComponent<Text>();
        TAnnonce2 = GameObject.Find("T_Annonce2").GetComponent<Text>();
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
            pos.GetComponent<PositionController>().SetInit(TRuck);
            Positions.Add(pos.GetComponent<PositionController>());
            pos.SetActive(false);
        }
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
            if (lastresult == 1)
            {
                TResult.color = Color.green;
                TResult.text = "Ruck réussi";
            }
            else if (lastresult != 0)
            {
                TResult.color = Color.red;
                TResult.text = "Ruck raté"; // add reason
            }
            else
                TResult.text = "";
            TTimer.text = ((int)timerPhase0 + 1).ToString();
            timerPhase0 -= Time.deltaTime;
            if ((timerPhase0 <= 0.0f) && (phase0 == false))
            {
                phase0 = true;
                TResult.color = Color.black;
                TResult.text = "Choisir la position.";
                StartAnnounce();
            }
        }
        else if (phase1 == false) // select pos
        {
            if (TRuck.gameObject.activeSelf == false)
                TRuck.gameObject.SetActive(true);
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
                {
                    initPhase2Modifier();
                    TResult.text = "Choisir la première passe.";
                    timerPhase2 = initPhase2;
                }
            }
        }
        else if (phase2 == false) // select player
        {
            TTimer.text = ((int)timerPhase2 + 1).ToString();
            timerPhase2 -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && (PauseUI.activeSelf == false) && (HelpUI.activeSelf == false) || (timerPhase2 <= 0.0f) && (phase2 == false))
            {
                CheckPhase2();
                if (phase2 == false)
                    Retry();
                else
                {
                    if (((string.Compare(initPos.posname, "Appuie") == 0) && (string.Compare(SelectedModifier, "Roll") == 0)) || ((string.Compare(SelectedAnnonce, "0") == 0) && (string.Compare(SelectedPlayer.pname, "0") == 0)))
                        ValidateRuck();
                    else
                    {
                        PassPhase2();
                        initPhase3Modifier();
                        TResult.text = "Choisir la deuxième.";
                        timerPhase3 = initPhase3;
                    }
                }
            }
        }
        else if (phase3 == false) // select 2d player
        {
            TTimer.text = ((int)timerPhase3 + 1).ToString();
            timerPhase3 -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space) && (PauseUI.activeSelf == false) && (HelpUI.activeSelf == false) || (timerPhase3 <= 0.0f) && (phase3 == false))
            {
                CheckPhase3();
                if (phase3 == false)
                    Retry();
                else
                    ValidateRuck();
            }
        }
    }

    public void PassPhase2()
    {
        TBall.position = SelectedPlayer.transform.position + new Vector3(0.5f, 0.0f, 1.5f);
        if (SelectedPlayer != null)
        {
            SelectedPlayer.MR.material.color = Color.black;
            SelectedPlayer = null;
        }
    }

    public void ValidateRuck()
    {
        lastresult = 1;
        Retry();
    }

    void StartAnnounce()
    {
        SelectedAnnonce = Annonces[Random.Range(0, Annonces.Count)];
        TAnnonce.text = SelectedAnnonce + " !"; 
        initRuck();
        timerPhase1 = initPhase1;
    }

    public void CheckPhase3()
    {
        if (SelectedPlayer == null)
            Debug.Log("No player selected phase3");
        else
        {
            switch (SelectedAnnonce)
            {
                case "Black":
                    if (string.Compare(initPos.posname, "Black") == 0)
                    {
                        if ((string.Compare(SelectedModifier, "Bis") == 0) &&  (string.Compare(SelectedPlayer.pname, "10") == 0))
                        {
                            phase3 = true;
                        }
                        else if ((string.Compare(SelectedPlayer.pname, "-1") == 0) || (string.Compare(SelectedPlayer.pname, "+1") == 0))
                        { 
                            phase3 = true;
                        }
                    }
                    break;
                case "Soutien":
                    if (string.Compare(initPos.posname, "Soutient") == 0)
                    {
                        if ((string.Compare(SelectedPlayer.pname, "-1") == 0) || (string.Compare(SelectedPlayer.pname, "+1") == 0))
                            phase3 = true;
                    }
                    break;
                case "Appuie":
                    if (string.Compare(initPos.posname, "Appuie") == 0)
                    {
                        if (string.Compare(SelectedModifier, "Roll") == 0)
                        {
                            if (string.Compare(SelectedPlayer.pname, "10") == 0)
                            {
                                phase3 = true;
                            }
                        }
                        else if (string.Compare(SelectedPlayer.pname, "+1") == 0)
                        { 
                                phase3 = true;
                        }
                    }
                    break;
                default:
                    Debug.Log("failed player at phase3 ");
                    break;
            }
        }
    }

    public void CheckPhase2()
    {
        if (SelectedPlayer == null)
            Debug.Log("No player selected phase2");
        else
        {
            switch (SelectedAnnonce)
            {
                case "Zero":
                    if (string.Compare(initPos.posname, "0") == 0)
                    {
                        if (string.Compare(SelectedPlayer.pname, "0") == 0)
                        {
                            phase2 = true;
                        }
                    }
                    break;
                case "Black":
                    if (string.Compare(initPos.posname, "Black") == 0)
                    {
                        if (string.Compare(SelectedPlayer.pname, "0") == 0)
                            phase2 = true;
                    }
                    break;
                case "Soutien":
                    if (string.Compare(initPos.posname, "Soutient") == 0)
                    {
                        if (string.Compare(SelectedPlayer.pname, "10") == 0)
                            phase2 = true;
                    }
                    break;
                case "Appuie":
                    if (string.Compare(initPos.posname, "Appuie") == 0)
                    {
                        if (string.Compare(SelectedModifier, "Roll") == 0)
                        {
                            if (string.Compare(SelectedPlayer.pname, "10") == 0)
                            {
                                phase2 = true;
                            }
                        }
                        else
                        {
                            if (string.Compare(SelectedPlayer.pname, "-1") == 0)
                                phase2 = true;
                        }
                    }
                    break;
                case "10":
                    if (string.Compare(initPos.posname, "10") == 0)
                    {
                        if (string.Compare(SelectedPlayer.pname, "10") == 0)
                        {
                            ValidateRuck();
                            phase2 = true;
                        }
                    }
                    break;
                default:
                    Debug.Log("failed player at phase 2");
                    break;
            }
        }
    }

    public void initPhase3Modifier()
    {
        SelectedModifier = "";
        if (string.Compare(initPos.posname, "Black") == 0)
        {
            int roll = Random.Range(0, 2);
            if (roll == 1)
            {
                SelectedModifier = "Bis";
                TAnnonce2.text = "Bis !";
            }
        }
        else if (string.Compare(initPos.posname, "Appuie") == 0)
        {
            int roll = Random.Range(0, 2);
            if (roll == 1)
            {
                SelectedModifier = "Roll";
                TAnnonce2.text = "Roll !";
            }
        }
    }

    public void initPhase2Modifier()
    {
        if (string.Compare(initPos.posname, "Appuie") == 0)
        {
            int roll = Random.Range(0, 2);
            if (roll == 1)
            {
                SelectedModifier = "Roll";
                TAnnonce2.text = "Roll !";
            }
        }
    }

    public void CheckPhase1()
    {
        switch (SelectedAnnonce)
        {
            case "Zero":
                if (string.Compare(initPos.posname, "0") == 0)
                    phase1 = true;
                break;
            case "Black":
                if (string.Compare(initPos.posname, "Black") == 0)
                    phase1 = true;
                break;
            case "Soutien":
                if (string.Compare(initPos.posname, "Soutient") == 0)
                    phase1 = true;
                break;
            case "Appuie":
                if (string.Compare(initPos.posname, "Appuie") == 0)
                    phase1 = true;
                break;
            case "10":
                if (string.Compare(initPos.posname, "10") == 0)
                    phase1 = true;
                break;
            default:
                Debug.Log("failed announce at phase 1");
                break;
        }

    }
    public void Retry()
    {
        SelectedAnnonce = "";
        SelectedModifier = "";
        TAnnonce.text = "";
        TAnnonce2.text = "";
        TResult.text = "";
        if (initPos != null)
        {
            initPos.gameObject.SetActive(false);
            initPos = null;
        }
        if (initSpawn != null)
        {
            initPos = null;
        }
        if (SelectedPlayer != null)
        {
            SelectedPlayer.MR.material.color = Color.black;
            SelectedPlayer = null;
        }
        foreach (PositionController p in Positions)
        {
            p.set = false;
        }
        timerPhase0 = initPhase0;
        timerPhase1 = initPhase1;
        timerPhase2 = initPhase2;
        timerPhase3 = initPhase3;
        TBall.position = TRuck.position - InitBall;
        phase0 = false;
        phase1 = false;
        phase2 = false;
        phase3 = false;
        TRuck.gameObject.SetActive(false);
        Debug.Log("retry");

    }
}
