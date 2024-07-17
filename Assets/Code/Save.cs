using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Save : MonoBehaviour
{

    //현재 안착한 Tile 위치
    private Vector2 PosSave = new Vector2();
    //회전할 Target 식별
    private int MyName;
    //키보드 입력 확인
    private bool PressKey = false;

    //통과한 타일에 빛남겨주는 이펙트
    private GameObject LightPrefabs;


    //방향 전환시 회전값에 곱해줄 값
    private float WayRoute;

    //판정선
    private GameObject RouteLine;
    private float Scale;

    //Flash
    private GameObject Flash;
    private bool FlashBool;


    //회전용 변수
    private GameObject P2;
    private TrailRenderer Trail;

    //ui , text , blur 순
    public GameObject Canvas1;
    public GameObject Canvas2;
    public GameObject Canvas3;
    [SerializeField] private Text CanvansText;
    public Text DieText;
    GameObject BackGround;
    private float Timer;

    private GameObject TextPrefabs1;
    private GameObject TextPrefabs2;
    private GameObject TextPrefabs3;
    private GameObject Boom;
    //텍스트 박스 띄우기용
    private GameObject TextObj;

    private int BallSet;

    //Audio Controll
    public AudioSource AudioS;

    //Ball Speed
    private float Speed;

    //Die
    private int Die;

    private bool coll = false;
    private void Awake()
    {
        LightPrefabs = Resources.Load("Prefabs/Light") as GameObject;
        TextPrefabs1 = Resources.Load("Prefabs/Great") as GameObject;
        TextPrefabs2 = Resources.Load("Prefabs/Fast") as GameObject;
        TextPrefabs3 = Resources.Load("Prefabs/Wrong") as GameObject;
        Boom = Resources.Load("Prefabs/SparkBlue") as GameObject;
    }
    private void Start()
    {
        RouteLine = GameObject.Find("BlueRoute");
        BackGround = GameObject.Find("BackGround");
        Flash = GameObject.Find("Flash");
        FlashBool = false;
        Timer = 0;
        //리로드시 타임스케일 0->1로 변경하여 재시작
        Time.timeScale = 1;
        CanvansText = GameObject.Find("Ready").GetComponent<Text>();
        //캔버스 비활성화
        Canvas1.SetActive(false);
        Canvas3.SetActive(false);
        Vector2 Tmp = new Vector2(5.8f, 5.7f);
        //현재 타일의 위치 저장
        Singleton.GetInstance.PosSave = Tmp;
        MyName = 0;
        WayRoute = -1.0f;

        //잔상
        P2 = GameObject.Find("PlayerBall2");
        Trail = transform.GetComponent<TrailRenderer>();
        Trail.sortingLayerName = "2";
        Trail.sortingOrder = 0;

        Scale = 0.0f;
        Speed = 5.5f;
        Die = 0;
    }
    private void Update()
    {
        GameObject Obj1 = GameObject.Find("Tile " + Singleton.GetInstance.TimeNum);
        //스페이스바를 누른 순간만 PressKey가 true
        if (Input.GetKeyDown(KeyCode.Space) && Singleton.GetInstance.BallSet == MyName)
        {
            PressKey = true;
            if (!coll)
            {
                Time.timeScale = 0;
                Debug.Log("Ball transform :" + transform.position + "Obj : transform" + Obj1.transform.position);
                Debug.Log(Vector3.Distance(transform.position, Obj1.transform.position));
            }
        }
        else PressKey = false;

        //UI
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Canvas2.SetActive(false);
            Canvas3.SetActive(true);
            Canvas1.SetActive(true);
            Time.timeScale = 0;
            Singleton.GetInstance.StartActive = false;
            AudioS.Pause();
        }
        if (Singleton.GetInstance.TimeNum != 160)
        {
            if (PressKey && MyName == BallSet && Vector3.Distance(transform.position, Obj1.transform.position) > 1.5f)
            {
                TextObj = Instantiate(TextPrefabs3);
                //성공시 텍스트 띄움
                Vector2 Pos = Singleton.GetInstance.PosSave;
                Pos.x -= 0.5f;
                Pos.y += 1.3f;
                GameObject TextBox = GameObject.Find("TextBox");
                TextObj.transform.name = "Text " + Singleton.GetInstance.TimeNum;
                TextObj.transform.parent = TextBox.transform;
                TextObj.transform.position = Pos;
                Die = 1;
                AudioS.Stop();
            }
        }
        if (Die == 2)
        {
            GameObject Obj = Instantiate(Boom);
            Obj.transform.position = transform.position;
            P2.gameObject.SetActive(false);
            transform.gameObject.SetActive(false);
            //진행도
            float Tmp = (float)Singleton.GetInstance.TimeNum / 160.0f * 100.0f;
            DieText.gameObject.SetActive(true);
            DieText.text = string.Format("{0:0.#}", Tmp) + "%";
            Singleton.GetInstance.Die = true;
        }
    }
    private void FixedUpdate()
    {
        if (!Singleton.GetInstance.StartActive && Singleton.GetInstance.Resume)
        {
            //Ready Action
            Timer = Singleton.GetInstance.Timer;
            Timer += Time.deltaTime * 1.0f;
            if (Timer >= 1.2f)
            {
                CanvansText.gameObject.SetActive(false);
                Singleton.GetInstance.StartActive = true;
                AudioS.UnPause();
            }
            else if (Timer >= 1.0f)
                CanvansText.text = "시작!";
            else if (Timer >= 0.75f)
                CanvansText.text = "1";
            else if (Timer >= 0.5f)
                CanvansText.text = "2";
            else if (Timer >= 0.25f)
                CanvansText.text = "3";
            Singleton.GetInstance.Timer = Timer;
        }
        if (Singleton.GetInstance.StartActive)
        {
            //사망시
            if (Die == 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, P2.transform.position, 0.01f);
                if (transform.position == P2.transform.position)
                    Die = 2;
            }

            //카메라도 같이 움직이게 해주려고 값 받음
            Singleton.GetInstance.WayRoute = WayRoute;

            if (Singleton.GetInstance.TimeNum == 14)
                WayRoute = 1.0f;
            if (Singleton.GetInstance.TimeNum == 23)
                WayRoute = -1.0f;

            //작업을 수행할 볼넘버 받아옴
            BallSet = Singleton.GetInstance.BallSet;

            //현재 회전해야할 공이 맞는지 확인하고 맞을시 회전
            if (BallSet == MyName)
            {
                PosSave = Singleton.GetInstance.PosSave;
                transform.RotateAround(P2.transform.position, Vector3.back, Speed);
                RouteLine.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
                Scale = 0.0f;
            }
            //아닐시 판정선 확대
            else
            {
                if (Scale < 0.5f)
                    Scale += Time.deltaTime * 1.0f;
                RouteLine.transform.localScale = new Vector3(Scale, Scale, 1.0f);
            }

            //Ready Action
            Timer = Singleton.GetInstance.Timer;
            Timer += Time.deltaTime * 1.0f;
            if (Timer >= 1.2f)
                CanvansText.gameObject.SetActive(false);
            else if (Timer >= 1.0f)
                CanvansText.text = "시작!";
            else if (Timer >= 0.75f)
                CanvansText.text = "1";
            else if (Timer >= 0.5f)
                CanvansText.text = "2";
            else if (Timer >= 0.25f)
                CanvansText.text = "3";
            Singleton.GetInstance.Timer = Timer;
            //Flash Action
            if (FlashBool)
            {
                Image Tmp = Flash.GetComponent<Image>();
                Color FlashColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                Tmp.color = FlashColor;
                FlashBool = false;
            }
            else
            {
                Image Tmp = Flash.GetComponent<Image>();
                if (Tmp.color.a > 0)
                {
                    Color FlashColor = Tmp.color;
                    FlashColor.a -= Time.deltaTime * 3.0f;
                    Tmp.color = FlashColor;
                }
            }
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Singleton.GetInstance.StartActive)
        {
            string Name = "Tile " + Singleton.GetInstance.TimeNum;
            //충돌시 충돌한 타일명이 지금 충돌해야할 타일인지 확인 , 스페이스바 입력여부 확인
            if (PressKey && collision.transform.name == Name)
            {
                coll = true;
                int BallSet = Singleton.GetInstance.BallSet;
                //충돌한 transform의 tag가 Tile 이고 현재 회전중인 공과 부딫힌게 맞는지 확인
                if (collision.transform.tag == "Tile" && BallSet == MyName)
                {
                    if (Name == "Tile 32")
                    {
                        Color Tmp = new Color(153.0f / 255.0f, 83.0f / 255.0f, 1.0f);
                        SpriteRenderer ColorChange = BackGround.GetComponent<SpriteRenderer>();
                        GameObject StopObject1 = GameObject.Find("StopObject1");
                        StopObject1.GetComponent<SpriteRenderer>().color = Tmp;
                        ColorChange.color = Tmp;
                        FlashBool = true;
                    }
                    if (Name == "Tile 96")
                    {
                        Color Tmp = new Color(1.0f, 1.0f, 1.0f);
                        SpriteRenderer ColorChange = BackGround.GetComponent<SpriteRenderer>();
                        ColorChange.color = Tmp;
                        FlashBool = true;
                        GameObject.Find("StopObject2").GetComponent<SpriteRenderer>()
                            .color = Tmp;
                        GameObject.Find("StopObject3").GetComponent<SpriteRenderer>()
                            .color = Tmp;
                        GameObject.Find("StopObject2").AddComponent<ObjectRun>();
                        GameObject.Find("StopObject3").AddComponent<ObjectRun>();
                        Singleton.GetInstance.SlowObjectGo = true;
                    }

                    //성공시 이전 타일에 빛남
                    GameObject Obj = Instantiate(LightPrefabs);
                    Obj.transform.position = Singleton.GetInstance.PosSave;
                    GameObject LightBox = GameObject.Find("LightBox");
                    Obj.transform.parent = LightBox.transform;

                    GameObject Obj1 = GameObject.Find("Tile " + Singleton.GetInstance.TimeNum);
                    if (Vector3.Distance(transform.position, Obj1.transform.position) < 0.5f)
                        TextObj = Instantiate(TextPrefabs1);
                    else if (Vector3.Distance(transform.position, Obj1.transform.position) < 1.0f)
                        TextObj = Instantiate(TextPrefabs2);


                    //성공시 텍스트 띄움
                    Vector2 Pos = Singleton.GetInstance.PosSave;
                    Pos.x -= 0.5f;
                    Pos.y += 1.3f;
                    GameObject TextBox = GameObject.Find("TextBox");
                    TextObj.transform.name = "Text " + Singleton.GetInstance.TimeNum;
                    TextObj.transform.parent = TextBox.transform;
                    TextObj.transform.position = Pos;

                    //성공시 타일에 지금 회전중이던 공 붙임
                    transform.position = collision.transform.position;
                    Vector2 SavePosition;
                    SavePosition.x = transform.position.x;
                    SavePosition.y = transform.position.y;

                    //현재 도달중인 타일 위치 갱신
                    Singleton.GetInstance.PosSave = SavePosition;

                    //Player1 ~ Plaeyr2 Switch
                    if (Singleton.GetInstance.BallSet == 0)
                        Singleton.GetInstance.BallSet = 1;
                    else
                        Singleton.GetInstance.BallSet = 0;

                    //이동 , 카메라 연산을 위해 타일넘버 갱신
                    Singleton.GetInstance.TimeNum++;
                    //카메라 이동에 맞춰 BackGround 소폭 이동

                    int TileNum = Singleton.GetInstance.TimeNum;
                    if (TileNum == 23 || TileNum == 14 || TileNum > 32 && TileNum < 45
                    || TileNum > 48 && TileNum < 61 || TileNum > 102 && TileNum < 106
                    || TileNum > 110 && TileNum < 114 || TileNum > 118 && TileNum < 122
                    || TileNum > 126 && TileNum < 130)
                        BackGround.transform.Translate(0.0f, -0.5f, 0.0f);
                    else if (TileNum > 64 && TileNum < 91)
                        BackGround.transform.Translate(0.4f * WayRoute * -1.0f, 0.0f, 0.0f);
                    else if (TileNum > 130 && TileNum < 161)
                    {
                        int TileCheck = (TileNum - 130) % 8;
                        if (TileCheck == 7)
                            BackGround.transform.Translate(0.0f, -0.5f, 0.0f);
                        else
                            BackGround.transform.Translate(0.8f * WayRoute * -1.0f, 0.0f, 0.0f);
                    }
                    else
                        BackGround.transform.Translate(1.1f * WayRoute * -1.0f, 0.0f, 0.0f);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        coll = false;
    }
}
