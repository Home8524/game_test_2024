using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

class BallController : MonoBehaviour
{
    //통과한 타일에 빛남겨주는 이펙트
    private GameObject lightPrefabs;

    private GameObject textPrefabs1;
    private GameObject textPrefabs2;
    private GameObject textPrefabs3;
    private GameObject boom;
    private Text canvansText;
    //Audio Controll
    public AudioSource audioS;
    public Text dieText;

    private bool endGameProgress;

    //회전용 오브젝트
    [SerializeField] private Transform redBall;
    [SerializeField] private Transform blueBall;
    private Transform currentBall;
    private Transform prevBall;

    //현재 움직이는 공 판단여부 - true일 경우 파란공이 움직이는중인것
    private bool isRed;

    //Ball Speed
    private float speed = 5.0f;

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

    //연출 리스트
    private Queue<EffectData> eDatas;
    private int effectTileNum;

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
        isRed = true;
        backGround = GameObject.Find("BackGround");
        tileNum = 0;
        endGameProgress = false;

        Managers.effectManager.SetObject();

        LoadPlayStage();
        ReadyAction();
    }

    private void LoadPlayStage()
    {
        var jsonData = Managers.dataManager.LoadStageData(Singleton.GetInstance.playStage);
        var dataList = jsonData.datas;
        var effectList = jsonData.effects;

        tileCount = dataList.Count;
        posList = new List<Vector2>();

        foreach(var data in dataList)
        {
            var pos = new Vector2(float.Parse(data.pos.x), float.Parse(data.pos.y));

            posList.Add(pos);
        }

        Vector2 startPos = posList[0];
        redBall.position = startPos;

        startPos += Singleton.GetInstance.ballOffset;
        blueBall.position = startPos;

        effectTileNum = -1;
        eDatas = new Queue<EffectData>();

        if (effectList.Count == 0) return;

        //처음 연출 작동시점
        effectTileNum = effectList[0].tile;

        foreach(var data in effectList)
        {
            eDatas.Enqueue(data);    
        }

    }
    private void Update()
    {
        // 사망처리 진행중에는 추가적인 입력 막기 위함
        if (endGameProgress) return;

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
        
        CheckRedBall();
        CheckBlueBall();
    }

    private IEnumerator coDIeAction()
    {
        float dt = Time.fixedDeltaTime;

        while(true)
        {
            if (currentBall.position == prevBall.position) break;
            currentBall.position = Vector3.MoveTowards(currentBall.position, prevBall.position, 0.01f);

            yield return new WaitForSeconds(dt);
        }

        GameObject obj = Instantiate(boom);
        obj.transform.position = currentBall.position;
        prevBall.gameObject.SetActive(false);
        currentBall.gameObject.SetActive(false);

        //진행도
        float Tmp = (float)tileNum / 160.0f * 100.0f;
        dieText.gameObject.SetActive(true);
        dieText.text = string.Format("{0:0.#}", Tmp) + "%";

        //Singleton 의 die 플래그와 지역변수 die를 같이 쓰는 이유는 카메라에 달아놓은 restart 작동에 문제생기지 않게 하기 위함
        Singleton.GetInstance.Die = true;
    }

    private void CheckNextTile()
    {
        int idx = ++tileNum;

        Vector2 ballPos = isRed ? blueBall.position : redBall.position;

        Vector2 tilePos = posList[idx];

        //실패 -> 사망 액션 진행
        
        if(Vector2.Distance(ballPos, tilePos) > 1.0f)
        {
            currentBall = isRed ? blueBall : redBall;
            prevBall = isRed ? redBall : blueBall;
            endGameProgress = true;
            audioS.Stop();

            StartCoroutine(coDIeAction());

            var failText = Instantiate(textPrefabs3);
            tilePos.x -= 0.5f;
            tilePos.y += 1.3f;
            failText.transform.parent = textBox.transform;
            failText.transform.position = tilePos;
            return;
        }
        

        //클리어
        if (idx == tileCount - 1)
        {
            Managers.effectManager.ClearAction();
            dieText.gameObject.SetActive(true);
            dieText.text = "축하합니다!";
            endGameProgress = true;
            Invoke("ClearAction", 1.5f);
            return;
        }

        //연출 여부 체크
        CheckEffectAction();

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

    private void ClearAction()
    {
        SceneManager.LoadScene("World");
    }

    private void CheckEffectAction()
    {
        if (eDatas.Count == 0) return;
        if (tileNum != effectTileNum) return;

        while(true)
        {
            if (eDatas.Count == 0) break;

            int next = eDatas.Peek().tile;
            if (next != tileNum)
            {
                effectTileNum = next;
                break;
            }

            var data = eDatas.Dequeue();
            Managers.effectManager.EffectAction(data);
        }
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
            updateTime += Time.deltaTime;

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

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}