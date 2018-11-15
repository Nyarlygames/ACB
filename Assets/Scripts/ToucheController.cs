using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToucheController : MonoBehaviour {

    GameObject PlayerPrefab;
    List<GameObject> Players = new List<GameObject>();
    public List<PlayerToucheController> PlayersControllers = new List<PlayerToucheController>();
    public PlayerToucheController PTC;
    List<string> PreAnnounce = new List<string>();
    List<string> PreAnnounce2 = new List<string>();
    List<string> NumAnnounce = new List<string>();
    List<string> ModAnnounce = new List<string>();
    string Pre = "";
    string Pre2 = "";
    string Num = "";
    string Mod = "";
    GameObject PauseUI;
    GameObject HelpUI;
    Text Timers;
    Text Annonce;
    public float timerAnnonce = 0.0f;
    public float timerPlacement = 0.0f;
    public float timerUp = 0.0f;
    public float timerDown = 0.0f;

    float initAnnonce = 5.0f;
    float initPlacement = 5.0f;
    float initUp = 5.0f;
    float initDown = 5.0f;

    public bool lockedAnnonce = false;
    public bool lockedPlacement = false;
    public bool lockedUp = false;
    public bool lockedDown = false;

    public int size = 7;

    public float MaxPos = 20.5f;
    public float initX = 0;
    public float initY = 2.0f;
    public float initZ = 2.0f;
    string LastAnnounce = "";
    public Vector3 initBall = Vector3.zero;
    public float ballUp = 1.85f;
    public float ballDown = 0.0f;
    GameObject Arrow;
    public ToucheArrowController ArrowLeft;
    public ToucheArrowController ArrowRight;

    GameObject TouchButton;
    public ToucheButtonController Sol;
    public ToucheButtonController Passe;

    public ToucheButtonController Garde;
    public ToucheButtonController Sort;

    public GameObject Ball;
    public GameObject Ten;
    public Text T_Last;
    public Text T_Log;
    int failedPos = 0;
    int failedMod = 0;
    int failedPlayer = 0;
    int success = 0;

    int lastresult = 0;

    public Sprite Crouch;
    public Sprite Lift;
    public Sprite Up;
    public Sprite Normal;
    public Sprite Keep;

    void Start()
    {
        FillAnnounces();

        Crouch = Resources.Load<Sprite>("Touche/crouch");
        Lift = Resources.Load<Sprite>("Touche/lift");
        Up = Resources.Load<Sprite>("Touche/jump");
        Normal = Resources.Load<Sprite>("Common/Player_normal");
        Keep = Resources.Load<Sprite>("Touche/garde");

       // PlayerPrefab = Resources.Load("Prefabs/PlayerTouchePrefab") as GameObject;
        PlayerPrefab = Resources.Load("Common/AvantTouche") as GameObject;
        Annonce = GameObject.Find("T_Annonce").GetComponent<Text>();
        Timers = GameObject.Find("T_Timers").GetComponent<Text>();
        PauseUI = GameObject.Find("UIPause");
        HelpUI = GameObject.Find("UIHelp");
        T_Log = GameObject.Find("T_Log").GetComponent<Text>();
        T_Last = GameObject.Find("T_LastResult").GetComponent<Text>();


        Ten = GameObject.Find("10");
        Ball = GameObject.Find("Ball");
        initBall = Ball.transform.position;
        PauseUI.SetActive(false);

        InitTouch();
        Arrow = Resources.Load("Prefabs/Arrow") as GameObject;
        ArrowLeft = Instantiate(Arrow).GetComponent<ToucheArrowController>();
        ArrowLeft.gameObject.name = "ArrowLeft";
        ArrowLeft.dir = -1;
        ArrowRight = Instantiate(Arrow).GetComponent<ToucheArrowController>();
        ArrowRight.gameObject.name = "ArrowRight";
        ArrowRight.dir = 1;
        ArrowRight.ArrowTransform.Rotate(new Vector3(0.0f, -180.0f, 0.0f));
        ArrowLeft.gameObject.SetActive(false);
        ArrowRight.gameObject.SetActive(false);


        TouchButton = Resources.Load("Prefabs/Button") as GameObject;
        Sol = Instantiate(TouchButton).GetComponent<ToucheButtonController>();
        Sol.gameObject.name = "Sol";
        Sol.Button_Normal = Resources.Load<Sprite>("Touche/B_sol_normal");
        Sol.Button_Highlight = Resources.Load<Sprite>("Touche/B_sol_highlight");
        Sol.Button_Click = Resources.Load<Sprite>("Touche/B_sol_click");
        Sol.command = 1;
        Sol.ButtonRenderer.sprite = Sol.Button_Normal;
        Sol.gameObject.SetActive(false);

        Passe = Instantiate(TouchButton).GetComponent<ToucheButtonController>();
        Passe.gameObject.name = "Passe";
        Passe.Button_Normal = Resources.Load<Sprite>("Touche/B_pass_normal");
        Passe.Button_Highlight = Resources.Load<Sprite>("Touche/B_pass_highlight");
        Passe.Button_Click = Resources.Load<Sprite>("Touche/B_pass_click");
        Passe.command = 2;
        Passe.ButtonRenderer.sprite = Passe.Button_Normal;
        Passe.gameObject.SetActive(false);

        Garde = Instantiate(TouchButton).GetComponent<ToucheButtonController>();
        Garde.gameObject.name = "Garde";
        Garde.Button_Normal = Resources.Load<Sprite>("Touche/B_keep_normal");
        Garde.Button_Highlight = Resources.Load<Sprite>("Touche/B_keep_highlight");
        Garde.Button_Click = Resources.Load<Sprite>("Touche/B_keepclick");
        Garde.command = 3;
        Garde.ButtonRenderer.sprite = Garde.Button_Normal;
        Garde.gameObject.SetActive(false);

        Sort = Instantiate(TouchButton).GetComponent<ToucheButtonController>();
        Sort.gameObject.name = "Pass";
        Sort.Button_Normal = Resources.Load<Sprite>("Touche/B_pass_normal");
        Sort.Button_Highlight = Resources.Load<Sprite>("Touche/B_pass_highlight");
        Sort.Button_Click = Resources.Load<Sprite>("Touche/B_pass_click");
        Sort.command = 4;
        Sort.ButtonRenderer.sprite = Sort.Button_Normal;
        Sort.gameObject.SetActive(false);

        timerAnnonce = initAnnonce;
        Time.timeScale = 0.0f;

    }

    void FixedUpdate()
    {
        T_Log.text = "Dernier lancement : " + LastAnnounce + "\n";
        T_Log.text += "Lancement réussis : " + success + "\n";
        T_Log.text += "Mauvais joueur : " + failedPlayer + "\n";
        T_Log.text += "Mauvaise position : " + failedPos + "\n";
        T_Log.text += "Mauvais lancement : " + failedMod;
        if (Input.GetKeyDown(KeyCode.Escape) && (PauseUI.activeSelf == false) && (HelpUI.activeSelf == false)) // open pause
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0.0f;
        }
        if ((lockedPlacement == false) || (lockedAnnonce == false))
        {
            // left
            if (Input.GetKeyDown(KeyCode.LeftArrow) && (PTC != null))
            {
                PTC.MovePlayer(-1);
                ArrowLeft.ArrowRenderer.sprite = ArrowLeft.ArrowClick;
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow) && (PTC != null))
            {
                ArrowLeft.ArrowRenderer.sprite = ArrowLeft.ArrowNormal;
            }
            //right
            if (Input.GetKeyDown(KeyCode.RightArrow) && (PTC != null))
            {
                PTC.MovePlayer(1);
                ArrowRight.ArrowRenderer.sprite = ArrowRight.ArrowClick;
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow) && (PTC != null))
            {
                ArrowRight.ArrowRenderer.sprite = ArrowRight.ArrowNormal;
            }
            else if (Input.GetKeyUp(KeyCode.Space) && (PTC != null))
            {
                timerPlacement = 0.0f;
            }
        }


        if (lockedAnnonce == false) // annonce
        {
            if (lastresult == 1)
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
                T_Last.text = "Echec : Mauvaise combinaison.";
            Timers.text = ((int)timerAnnonce + 1).ToString();
            timerAnnonce -= Time.deltaTime;
            if ((timerAnnonce <= 0.0f) && (lockedAnnonce == false))
            {
                lockedAnnonce = true;
                T_Last.color = Color.black;
                T_Last.text = "";
                StartAnnounce();
            }
        }
        else if (lockedPlacement == false) // placement
        {
            Timers.text = ((int)timerPlacement + 1).ToString();
            timerPlacement -= Time.deltaTime;

            if ((timerPlacement <= 0.0f) && (lockedPlacement == false))
            {
                int result = VerifyPlacement();
                if (lockedPlacement == false)
                {
                    Retry(result);
                }
                else
                {
                    if (string.Compare(Mod, "Shap") == 0)
                    {
                        endTouch();
                    }
                    else
                    {
                        PTC.SetUp();
                        ArrowLeft.gameObject.SetActive(false);
                        ArrowRight.gameObject.SetActive(false);
                        Passe.PTC = PTC;
                        Sol.PTC = PTC;
                        Passe.setPos();
                        Sol.setPos();
                        Passe.gameObject.SetActive(true);
                        Sol.gameObject.SetActive(true);
                        Ball.transform.position = PTC.PTransform.position + new Vector3(0.0f, ballUp, 0.0f);
                        timerUp = initUp;
                    }
                }
            }
        }
        else if (lockedUp == false) // saut
        {
            Timers.text = ((int)timerUp + 1).ToString();
            timerUp -= Time.deltaTime;

            if ((timerUp <= 0.0f) && (lockedUp == false))
            {
                // check decide down or pass
                int result = VerifyUp();

                if (lockedUp == false)
                {
                    Retry(result);
                }
                else
                {
                    Passe.gameObject.SetActive(false);
                    Sol.gameObject.SetActive(false);

                    Garde.PTC = PTC;
                    Sort.PTC = PTC;
                    Garde.setPos();
                    Sort.setPos();
                    Garde.gameObject.SetActive(true);
                    Sort.gameObject.SetActive(true);
                    PTC.SetDown();
                    Ball.transform.position = PTC.PTransform.position + new Vector3(0.0f, ballDown, -0.1f); ;

                    if (PTC.command == 2) // run and pass
                        endTouch();
                    timerDown = initDown;
                }
            }
        }
        else if (lockedDown == false) // down
        {
            Timers.text = ((int)timerDown + 1).ToString();
            timerDown -= Time.deltaTime;

            if ((timerDown <= 0.0f) && (lockedDown == false))
            {
                // check decide keep or pass
                int result = VerifyDown();

                if (lockedUp == false)
                {
                    Retry(result);
                }
                else
                {
                    endTouch();
                }
            }
        }
    }

    void endTouch()
    {
        success++;
        Retry(1);
    }

    int VerifyDown()
    {
        int result = 0;
        switch (Mod)
        {
            case "Drive":
                if (PTC.command == 3)
                    lockedDown = true;
                else if (PTC.command == 0)
                {
                    failedMod++;
                    result = 4;
                }
                else
                {
                    failedMod++;
                    result = 4;
                }
                break;
            case "Tempo":
                if (PTC.command == 4)
                    lockedDown = true;
                else if (PTC.command == 0)
                {
                    failedMod++;
                    result = 4;
                }
                else
                {
                    failedMod++;
                    result = 4;
                }
                break;
            default:
                Debug.Log("FAILED verif mod2, check switch");
                break;
        }
        return result;
    }

    int VerifyUp()
    {
        int result = 0;
        switch (Mod){
            case "Drive":
                if (PTC.command == 1)
                    lockedUp = true;
                else if (PTC.command == 0)
                {
                    failedMod++;
                    result = 4;
                }
                else
                {
                    failedMod++;
                    result = 4;
                }
                break;
            case "Run":
                if (PTC.command == 2)
                    lockedUp = true;
                else if (PTC.command == 0)
                {
                    failedMod++;
                    result = 4;
                }
                else
                {
                    failedMod++;
                    result = 4;
                }
                break;
            case "Tempo":
                if (PTC.command == 1)
                    lockedUp = true;
                else if (PTC.command == 0)
                {
                    failedMod++;
                    result = 4;
                }
                else
                {
                    failedMod++;
                    result = 4;
                }
                break;
            default:
                Debug.Log("FAILED verif mod, check switch");
                break;
        }
        return result;
    }

    void Retry(int result) // 1 success 2 joueur 3 position 4 lancement
    {
        if (result == 1)
        {
            LastAnnounce = Annonce.text + " (réussi)";
            lastresult = result;
        }
        else if (result == 2)
        {
            LastAnnounce = Annonce.text + " (mauvais sauteur)";
            lastresult = result;
        }
        else if (result == 3)
        {
            LastAnnounce = Annonce.text + " (mauvaise position)";
            lastresult = result;
        }
        else if (result == 4)
        {
            LastAnnounce = Annonce.text + " (mauvais lancement)";
            lastresult = result;
        }
        else
            LastAnnounce = Annonce.text + " (???)";
        lockedAnnonce = false;
        lockedDown = false;
        lockedPlacement = false;
        lockedUp = false;
        timerAnnonce = initAnnonce;
        timerPlacement = initPlacement;
        timerUp = initUp;
        timerDown = initDown;
        foreach(GameObject P in Players)
            Destroy(P);
        Players.Clear();
        PlayersControllers.Clear();
        PTC = null;

        ArrowLeft.PTC = null;
        ArrowRight.PTC = null;
        ArrowLeft.ArrowRenderer.sprite = ArrowLeft.ArrowNormal;
        ArrowRight.ArrowRenderer.sprite = ArrowRight.ArrowNormal;
        ArrowLeft.gameObject.SetActive(false);
        ArrowRight.gameObject.SetActive(false);

        Sol.ButtonRenderer.sprite = Sol.Button_Normal;
        Passe.ButtonRenderer.sprite = Passe.Button_Normal;
        Sol.gameObject.SetActive(false);
        Passe.gameObject.SetActive(false);

        Garde.ButtonRenderer.sprite = Garde.Button_Normal;
        Sort.ButtonRenderer.sprite = Sort.Button_Normal;
        Garde.gameObject.SetActive(false);
        Sort.gameObject.SetActive(false);

        Ball.transform.position = initBall;

        PreAnnounce.Clear();
        PreAnnounce2.Clear();
        NumAnnounce.Clear();
        ModAnnounce.Clear();
        FillAnnounces();

        Timers.text = "";
        Annonce.text = "";
        InitTouch();
    }

    int VerifyPlacement()
    {
        int result = 0;
        if (PTC == null)
        {
            failedPlayer++;
            result = 2;
        }
        else
        {
            if (string.Compare(Mod, "Shap") == 0)
            {
                if (PTC.pos == 0)
                    lockedPlacement = true;
                else
                {
                    failedPlayer++;
                    result = 2;
                }
            }
            else
            {
                switch (Num)
                {
                    case "10":
                        if ((PTC.pos == 5) && (PTC.side == 0))
                            lockedPlacement = true;
                        else if ((PTC.side != 0))
                        {
                            failedPos++;
                            result = 3;
                        }
                        else if ((PTC.pos != 5))
                        {
                            failedPlayer++;
                            result = 2;
                        }
                        break;
                    case "11":
                        if ((PTC.pos == 5) && (PTC.side == -1))
                            lockedPlacement = true;
                        else if ((PTC.side != -1))
                        {
                            failedPos++;
                            result = 3;
                        }
                        else if ((PTC.pos != 5))
                        {
                            failedPlayer++;
                            result = 2;
                        }
                        break;
                    case "12":
                        if ((PTC.pos == 5) && (PTC.side == 1))
                            lockedPlacement = true;
                        else if ((PTC.side != 1))
                        {
                            failedPos++;
                            result = 3;
                        }
                        else if ((PTC.pos != 5))
                        {
                            failedPlayer++;
                            result = 2;
                        }
                        break;
                    case "20":
                        if ((PTC.pos == 4) && (PTC.side == 0))
                            lockedPlacement = true;
                        else if ((PTC.side != 0))
                        {
                            failedPos++;
                            result = 3;
                        }
                        else if ((PTC.pos != 4))
                        {
                            failedPlayer++;
                            result = 2;
                        }
                        break;
                    case "21":
                        if ((PTC.pos == 4) && (PTC.side == -1))
                            lockedPlacement = true;
                        else if ((PTC.side != -1))
                        {
                            failedPos++;
                            result = 3;
                        }
                        else if ((PTC.pos != 4))
                        {
                            failedPlayer++;
                            result = 2;
                        }
                        break;
                    case "22":
                        if ((PTC.pos == 4) && (PTC.side == 1))
                            lockedPlacement = true;
                        else if ((PTC.side != 1))
                        {
                            failedPos++;
                            result = 3;
                        }
                        else if ((PTC.pos != 4))
                        {
                            failedPlayer++;
                            result = 2;
                        }
                        break;
                    case "30":
                        if ((PTC.pos == 3) && (PTC.side == 0))
                            lockedPlacement = true;
                        else if ((PTC.side != 0))
                        {
                            failedPos++;
                            result = 3;
                        }
                        else if ((PTC.pos != 3))
                        {
                            failedPlayer++;
                            result = 2;
                        }
                        break;
                    case "31":
                        if ((PTC.pos == 3) && (PTC.side == -1))
                            lockedPlacement = true;
                        else if ((PTC.side != -1))
                        {
                            failedPos++;
                            result = 3;
                        }
                        else if ((PTC.pos != 3))
                        {
                            failedPlayer++;
                            result = 2;
                        }
                        break;
                    case "32":
                        if ((PTC.pos == 3) && (PTC.side == 1))
                            lockedPlacement = true;
                        else if ((PTC.side != 1))
                        {
                            failedPos++;
                            result = 3;
                        }
                        else if ((PTC.pos != 3))
                        {
                            failedPlayer++;
                            result = 2;
                        }
                        break;
                    case "40":
                        if ((PTC.pos == 2) && (PTC.side == 0))
                            lockedPlacement = true;
                        else if ((PTC.side != 0))
                        {
                            failedPos++;
                            result = 3;
                        }
                        else if ((PTC.pos != 2))
                        {
                            failedPlayer++;
                            result = 2;
                        }
                        break;
                    case "41":
                        if ((PTC.pos == 2) && (PTC.side == -1))
                            lockedPlacement = true;
                        else if ((PTC.side != -1))
                        {
                            failedPos++;
                            result = 3;
                        }
                        else if ((PTC.pos != 2))
                        {
                            failedPlayer++;
                            result = 2;
                        }
                        break;
                    case "42":
                        if ((PTC.pos == 2) && (PTC.side == 1))
                            lockedPlacement = true;
                        else if ((PTC.side != 10))
                        {
                            failedPos++;
                            result = 3;
                        }
                        else if ((PTC.pos != 2))
                        {
                            failedPlayer++;
                            result = 2;
                        }
                        break;
                    case "50":
                        if ((PTC.pos == 1) && (PTC.side == 0))
                            lockedPlacement = true;
                        else if ((PTC.side != 0))
                        {
                            failedPos++;
                            result = 3;
                        }
                        else if ((PTC.pos != 1))
                        {
                            failedPlayer++;
                            result = 2;
                        }
                        break;
                    case "51":
                        if ((PTC.pos == 1) && (PTC.side == -1))
                            lockedPlacement = true;
                        else if ((PTC.side != -1))
                        {
                            failedPos++;
                            result = 3;
                        }
                        else if ((PTC.pos != 1))
                        {
                            failedPlayer++;
                            result = 2;
                        }
                        break;
                    case "52":
                        if ((PTC.pos == 1) && (PTC.side == 1))
                            lockedPlacement = true;
                        else if ((PTC.side != 1))
                        {
                            failedPos++;
                            result = 3;
                        }
                        else if ((PTC.pos != 1))
                        {
                            failedPlayer++;
                            result = 2;
                        }
                        break;
                    default:
                        Debug.Log("???");
                        //failedPlayer++;
                        break;
                }
            }
        }
        return result;
    }

    void StartAnnounce()
    {
        string NewAnnounce = "";

        Pre = PreAnnounce[Random.Range(0, PreAnnounce.Count)];

        if ((string.Compare(Pre, "Matesh") == 0) && (LastAnnounce != "") && ((size == 7) || ((size == 6) && (Num[0] != 1)) || ((size == 5) && (Num[0] != 1) && (Num[0] != 2)) || ((size == 4) && (Num[0] != 1) && (Num[0] != 2) && (Num[0] != 3))))
            NewAnnounce = "Matesh (" + Num + ") " + Mod + " Bobi";
        else
        {
            Pre = PreAnnounce[Random.Range(1, PreAnnounce.Count)];

            if (string.Compare(Pre, "") != 0)
                Pre2 = PreAnnounce2[Random.Range(0, PreAnnounce2.Count)];
            switch (size)
            {
                case 4:
                    Num = NumAnnounce[Random.Range(9, NumAnnounce.Count)];
                    break;
                case 5:
                    Num = NumAnnounce[Random.Range(6, NumAnnounce.Count)];
                    break;
                case 6:
                    Num = NumAnnounce[Random.Range(3, NumAnnounce.Count)];
                    break;
                case 7:
                    Num = NumAnnounce[Random.Range(0, NumAnnounce.Count)];
                    break;
            }
            Mod = ModAnnounce[Random.Range(0, ModAnnounce.Count)];
            if (string.Compare(Pre2, "") != 0)
                NewAnnounce = Pre + " " + Pre2 + " " + Num + " " + Mod + " Bobi";
            else
                NewAnnounce = Pre + " " + Num + " " + Mod + " Bobi";
        }

        Annonce.text = NewAnnounce; // add delay for announces
        timerPlacement = initPlacement;
    }

    void InitTouch()
    {
        size = Random.Range(4, 7);
        for (int i= 0; i<size; i++)
        {
            GameObject temp = Instantiate(PlayerPrefab);
            PlayerToucheController tempcontrol = temp.GetComponent<PlayerToucheController>();
            temp.name = "Player" + i;
            tempcontrol.pos = i;
            tempcontrol.SetPlace(initX, initY, initZ, MaxPos, size);
            Players.Add(temp);
            PlayersControllers.Add(tempcontrol);
        }
    }

    void FillAnnounces()
    {
        PreAnnounce.Add("Matesh");
        PreAnnounce.Add("");

        for (int i = 0; i < 8; i++)
            PreAnnounce.Add(((int)Random.Range(0, 9999)).ToString());

        PreAnnounce2.Add("");
        PreAnnounce2.Add("");
        PreAnnounce2.Add("");
        PreAnnounce2.Add("");
        PreAnnounce2.Add(((int)Random.Range(0, 100)).ToString());

        NumAnnounce.Add("10");
        NumAnnounce.Add("11");
        NumAnnounce.Add("12");
        NumAnnounce.Add("20");
        NumAnnounce.Add("21");
        NumAnnounce.Add("22");
        NumAnnounce.Add("30");
        NumAnnounce.Add("31");
        NumAnnounce.Add("32");
        NumAnnounce.Add("40");
        NumAnnounce.Add("41");
        NumAnnounce.Add("42");
        NumAnnounce.Add("50");
        NumAnnounce.Add("51");
        NumAnnounce.Add("52");

        ModAnnounce.Add("Drive");
        ModAnnounce.Add("Run");
        ModAnnounce.Add("Tempo");
        ModAnnounce.Add("Shap");
    }
}
