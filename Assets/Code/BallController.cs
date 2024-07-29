using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class BallController : MonoBehaviour
{
    private Vector2 PosSave = new Vector2();

    //통과한 타일에 빛남겨주는 이펙트
    private GameObject lightPrefabs;

    private GameObject textPrefabs1;
    private GameObject textPrefabs2;
    private GameObject textPrefabs3;
    private GameObject boom;
    private float dt;
    private Text canvansText;
    //Audio Controll
    public AudioSource audioS;
    public Text dieText;

    //방향 전환시 회전값에 곱해줄 값
    private float wayRoute;

    //Die 사망 진행도에 관한 step << 추후 변경해야함.
    private int die;

    //회전용 오브젝트
    [SerializeField] private Transform redBall;
    [SerializeField] private Transform blueBall;
    private Transform currentBall;
    private Transform prevBall;

    //현재 움직이는 공 판단여부 - true일 경우 파란공이 움직이는중인것
    private bool isRed;

    //Ball Speed
    private float speed = 5.0f;

    //Flash
    private GameObject flash;
    private bool flashBool;

    //Tile Pos List
    private List<Vector2> posList;

    //현재 공이 존재하는 타일넘버
    private int tileNum;

    //타일의 최대 개수
    private int tileCount;

    //배경
    GameObject backGround;

    //판정선
    private GameObject redRouteLine;
    private GameObject blueRouteLine;

    private float redRouteScale;
    private float blueRouteScale;

    private void Awake()
    {
        lightPrefabs = Resources.Load("Prefabs/Light") as GameObject;
        textPrefabs1 = Resources.Load("Prefabs/Great") as GameObject;
        textPrefabs2 = Resources.Load("Prefabs/Fast") as GameObject;
        textPrefabs3 = Resources.Load("Prefabs/Wrong") as GameObject;
        boom = Resources.Load("Prefabs/SparkBlue") as GameObject;

        blueRouteLine = GameObject.Find("BlueRoute");
        redRouteLine = GameObject.Find("RedRoute");
    }

    private void Start()
    {
        Managers.uiManager.LoadUI();
        Managers.uiManager.ActiveUi(eCanvas.text);
        Managers.uiManager.SetReadyTextEvent(ReadyAction);

        //리로드시 타임스케일 0->1로 변경하여 재시작
        Time.timeScale = 1;
        canvansText = GameObject.Find("Ready").GetComponent<Text>();
        dt = Time.deltaTime;
        flash = GameObject.Find("Flash");
        flashBool = false;
        isRed = true;
        backGround = GameObject.Find("BackGround");

        if (Singleton.GetInstance.StartActive == false) return;

        tileNum = 0;
        wayRoute = -1.0f;

        LoadPlayStage();
        //Ready Action
        ReadyAction();
    }

    private void LoadPlayStage()
    {
        var dataList = Managers.dataManager.LoadTileData(Singleton.GetInstance.playStage);

        tileCount = dataList.Count;
        posList = new List<Vector2>();

        foreach(var data in dataList)
        {
            var pos = new Vector2(float.Parse(data.pos.x), float.Parse(data.pos.y));

            posList.Add(pos);
        }
    }
    private void Update()
    {
        // 사망처리 진행중에는 추가적인 입력 막기 위함
        if (die > 0) return;

        //다음 타일로의 이동판정
        if(Input.GetKeyDown(KeyCode.Space))
        {
            CheckNextTile();
        }

        //ui 열기
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Managers.uiManager.ActiveUi(eCanvas.ui);
            Time.timeScale = 0;
            Singleton.GetInstance.StartActive = false;
            audioS.Pause();
        }
    }

    private void CheckRedBall()
    {
        if(isRed)
        {
            PosSave = Singleton.GetInstance.PosSave;
            blueBall.RotateAround(redBall.position, Vector3.back, speed);
            blueRouteLine.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
            blueRouteScale = 0.0f;

            return;
        }

        
        if (blueRouteScale < 0.5f)
            blueRouteScale += Time.deltaTime * 1.0f;
        blueRouteLine.transform.localScale = new Vector3(blueRouteScale, blueRouteScale, 1.0f);
        
    }

    private void CheckBlueBall()
    {
        if (!isRed)
        {
            PosSave = Singleton.GetInstance.PosSave;
            redBall.RotateAround(blueBall.position, Vector3.back, speed);
            redRouteLine.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
            redRouteScale = 0.0f;

            return;
        }

        
        if (redRouteScale < 0.5f)
            redRouteScale += Time.deltaTime * 1.0f;
        redRouteLine.transform.localScale = new Vector3(redRouteScale, redRouteScale, 1.0f);
        
    }

    private void FixedUpdate()
    {
        if (Singleton.GetInstance.StartActive == false) return;

        //사망시
        if (die == 1)
        {
            currentBall.position = Vector3.MoveTowards(currentBall.position, prevBall.position, 0.01f);
            if (currentBall.position == prevBall.position)
                die = 2;
        }
        

        //카메라도 같이 움직이게 해주려고 값 받음
        Singleton.GetInstance.WayRoute = wayRoute;

        if (Singleton.GetInstance.TimeNum == 14)
            wayRoute = 1.0f;
        if (Singleton.GetInstance.TimeNum == 23)
            wayRoute = -1.0f;

        CheckRedBall();
        CheckBlueBall();

        //Flash Action
        if (flashBool)
        {
            Image Tmp = flash.GetComponent<Image>();
            Color FlashColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            Tmp.color = FlashColor;
            flashBool = false;
        }
        else
        {
            Image Tmp = flash.GetComponent<Image>();
            if (Tmp.color.a > 0)
            {
                Color FlashColor = Tmp.color;
                FlashColor.a -= Time.deltaTime * 3.0f;
                Tmp.color = FlashColor;
            }
        }
}

    private void CheckNextTile()
    {
        int idx = ++tileNum;

        Vector2 ballPos = isRed ? blueBall.position : redBall.position;

        //마지막 타일
        if (idx == tileCount - 1)
        {
            return;
        }

        Vector2 tilePos = posList[idx];

        //실패 -> 사망 액션 진행
        if(Vector2.Distance(ballPos, tilePos) > 1.0f)
        {
            currentBall = isRed ? blueBall : redBall;
            prevBall = isRed ? redBall : blueBall;
            die = 1;
            audioS.Stop();
            return;
        }

        Transform obj = isRed ? blueBall : redBall;

        isRed = !isRed;

        //성공시 타일에 지금 회전중이던 공 붙임
        obj.position = tilePos;

        Vector2 SavePosition;
        SavePosition.x = tilePos.x;
        SavePosition.y = tilePos.y;

        //현재 도달중인 타일 위치 갱신
        Singleton.GetInstance.PosSave = SavePosition;

        //이하의 코드는 추가적으로 손봐야함
        //이동 , 카메라 연산을 위해 타일넘버 갱신
        Singleton.GetInstance.TimeNum++;

        //카메라 이동 맞춰서 bg 이동
        if (tileNum == 23 || tileNum == 14 || tileNum > 32 && tileNum < 45
            || tileNum > 48 && tileNum < 61 || tileNum > 102 && tileNum < 106
            || tileNum > 110 && tileNum < 114 || tileNum > 118 && tileNum < 122
            || tileNum > 126 && tileNum < 130)
            backGround.transform.Translate(0.0f, -0.5f, 0.0f);
        else if (tileNum > 64 && tileNum < 91)
            backGround.transform.Translate(0.4f * wayRoute * -1.0f, 0.0f, 0.0f);
        else if (tileNum > 130 && tileNum < 161)
        {
            int TileCheck = (tileNum - 130) % 8;
            if (TileCheck == 7)
                backGround.transform.Translate(0.0f, -0.5f, 0.0f);
            else
                backGround.transform.Translate(0.8f * wayRoute * -1.0f, 0.0f, 0.0f);
        }
        else
            backGround.transform.Translate(1.1f * wayRoute * -1.0f, 0.0f, 0.0f);
    }
    private void ReadyAction()
    {
        StartCoroutine(coReadyAction());
    }

    private IEnumerator coReadyAction()
    {
        float updateTime = 0f;

        while (true)
        {
            updateTime += dt;

            if (updateTime >= 1.2f)
            {
                canvansText.gameObject.SetActive(false);
                Singleton.GetInstance.StartActive = true;
                audioS.UnPause();
                break;
            }
            else if (updateTime >= 1.0f)
                canvansText.text = "시작!";
            else if (updateTime >= 0.75f)
                canvansText.text = "1";
            else if (updateTime >= 0.5f)
                canvansText.text = "2";
            else if (updateTime >= 0.25f)
                canvansText.text = "3";

            yield return new WaitForSeconds(dt);
        }
    }
}