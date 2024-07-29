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

    //카메라 스크립트
    private CameraControll cControll;

    private GameObject textBox;
    private GameObject lightBox;
    private void Awake()
    {
        lightPrefabs = Resources.Load("Prefabs/Light") as GameObject;
        textPrefabs1 = Resources.Load("Prefabs/Great") as GameObject;
        textPrefabs2 = Resources.Load("Prefabs/Fast") as GameObject;
        textPrefabs3 = Resources.Load("Prefabs/Wrong") as GameObject;
        boom = Resources.Load("Prefabs/SparkBlue") as GameObject;

        blueRouteLine = GameObject.Find("BlueRoute");
        redRouteLine = GameObject.Find("RedRoute");

        lightBox = GameObject.Find("LightBox");
        textBox = GameObject.Find("TextBox");

        //메인카메라에 스크립트를 달아둘 것
        GameObject camera = Camera.main.gameObject;
        cControll = Utils.FindComp<CameraControll>(camera.transform, "");
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
        /*
        if(Vector2.Distance(ballPos, tilePos) > 1.0f)
        {
            currentBall = isRed ? blueBall : redBall;
            prevBall = isRed ? redBall : blueBall;
            die = 1;
            audioS.Stop();
            return;
        }
        */
        Transform obj = isRed ? blueBall : redBall;

        //성공시 타일에 빛남
        GameObject light = Instantiate(lightPrefabs);
        light.transform.position = tilePos;
        light.transform.parent = lightBox.transform;

        var textObj = Vector3.Distance(obj.position, tilePos) < 0.5f ? Instantiate(textPrefabs1) : Instantiate(textPrefabs2);

        //성공시 텍스트 띄움
        Vector2 Pos = tilePos;
        Pos.x -= 0.5f;
        Pos.y += 1.3f;
        textObj.transform.name = "Text " + tileNum;
        textObj.transform.parent = textBox.transform;
        textObj.transform.position = Pos;

        //메인 공 플래그 변경
        isRed = !isRed;

        //성공시 타일에 지금 회전중이던 공 붙임
        obj.position = tilePos;

        Vector3 diff = tilePos - posList[idx - 1];
        cControll.SetPos(diff);

        backGround.transform.position += diff;
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