using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Save : MonoBehaviour
{

    //���� ������ Tile ��ġ
    private Vector2 PosSave = new Vector2();
    //ȸ���� Target �ĺ�
    private int MyName;
    //Ű���� �Է� Ȯ��
    private bool PressKey = false;

    //����� Ÿ�Ͽ� �������ִ� ����Ʈ
    private GameObject LightPrefabs;


    //���� ��ȯ�� ȸ������ ������ ��
    private float WayRoute;

    //������
    private GameObject RouteLine;
    private float Scale;

    //Flash
    private GameObject Flash;
    private bool FlashBool;


    //ȸ���� ����
    private GameObject P2;
    private TrailRenderer Trail;

    //ui , text , blur ��
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
    //�ؽ�Ʈ �ڽ� �����
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
        //���ε�� Ÿ�ӽ����� 0->1�� �����Ͽ� �����
        Time.timeScale = 1;
        CanvansText = GameObject.Find("Ready").GetComponent<Text>();
        //ĵ���� ��Ȱ��ȭ
        Canvas1.SetActive(false);
        Canvas3.SetActive(false);
        Vector2 Tmp = new Vector2(5.8f, 5.7f);
        //���� Ÿ���� ��ġ ����
        Singleton.GetInstance.PosSave = Tmp;
        MyName = 0;
        WayRoute = -1.0f;

        //�ܻ�
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
        //�����̽��ٸ� ���� ������ PressKey�� true
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
                //������ �ؽ�Ʈ ���
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
            //���൵
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
                CanvansText.text = "����!";
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
            //�����
            if (Die == 1)
            {
                transform.position = Vector3.MoveTowards(transform.position, P2.transform.position, 0.01f);
                if (transform.position == P2.transform.position)
                    Die = 2;
            }

            //ī�޶� ���� �����̰� ���ַ��� �� ����
            Singleton.GetInstance.WayRoute = WayRoute;

            if (Singleton.GetInstance.TimeNum == 14)
                WayRoute = 1.0f;
            if (Singleton.GetInstance.TimeNum == 23)
                WayRoute = -1.0f;

            //�۾��� ������ ���ѹ� �޾ƿ�
            BallSet = Singleton.GetInstance.BallSet;

            //���� ȸ���ؾ��� ���� �´��� Ȯ���ϰ� ������ ȸ��
            if (BallSet == MyName)
            {
                PosSave = Singleton.GetInstance.PosSave;
                transform.RotateAround(P2.transform.position, Vector3.back, Speed);
                RouteLine.transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);
                Scale = 0.0f;
            }
            //�ƴҽ� ������ Ȯ��
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
                CanvansText.text = "����!";
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
            //�浹�� �浹�� Ÿ�ϸ��� ���� �浹�ؾ��� Ÿ������ Ȯ�� , �����̽��� �Է¿��� Ȯ��
            if (PressKey && collision.transform.name == Name)
            {
                coll = true;
                int BallSet = Singleton.GetInstance.BallSet;
                //�浹�� transform�� tag�� Tile �̰� ���� ȸ������ ���� �΋H���� �´��� Ȯ��
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

                    //������ ���� Ÿ�Ͽ� ����
                    GameObject Obj = Instantiate(LightPrefabs);
                    Obj.transform.position = Singleton.GetInstance.PosSave;
                    GameObject LightBox = GameObject.Find("LightBox");
                    Obj.transform.parent = LightBox.transform;

                    GameObject Obj1 = GameObject.Find("Tile " + Singleton.GetInstance.TimeNum);
                    if (Vector3.Distance(transform.position, Obj1.transform.position) < 0.5f)
                        TextObj = Instantiate(TextPrefabs1);
                    else if (Vector3.Distance(transform.position, Obj1.transform.position) < 1.0f)
                        TextObj = Instantiate(TextPrefabs2);


                    //������ �ؽ�Ʈ ���
                    Vector2 Pos = Singleton.GetInstance.PosSave;
                    Pos.x -= 0.5f;
                    Pos.y += 1.3f;
                    GameObject TextBox = GameObject.Find("TextBox");
                    TextObj.transform.name = "Text " + Singleton.GetInstance.TimeNum;
                    TextObj.transform.parent = TextBox.transform;
                    TextObj.transform.position = Pos;

                    //������ Ÿ�Ͽ� ���� ȸ�����̴� �� ����
                    transform.position = collision.transform.position;
                    Vector2 SavePosition;
                    SavePosition.x = transform.position.x;
                    SavePosition.y = transform.position.y;

                    //���� �������� Ÿ�� ��ġ ����
                    Singleton.GetInstance.PosSave = SavePosition;

                    //Player1 ~ Plaeyr2 Switch
                    if (Singleton.GetInstance.BallSet == 0)
                        Singleton.GetInstance.BallSet = 1;
                    else
                        Singleton.GetInstance.BallSet = 0;

                    //�̵� , ī�޶� ������ ���� Ÿ�ϳѹ� ����
                    Singleton.GetInstance.TimeNum++;
                    //ī�޶� �̵��� ���� BackGround ���� �̵�

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
