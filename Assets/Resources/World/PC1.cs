using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PC1 : MonoBehaviour
{
    [SerializeField] GameObject P1;
    public GameObject Switch;
    private bool PressKey;
    [SerializeField] private GameObject BoxPrefab;
    public GameObject Camera;
    private GameObject BG;
    private void Awake()
    {
        BoxPrefab = Resources.Load("Prefabs/ChangeLightBox") as GameObject;
    }
    private void Start()
    {
        transform.position = Singleton.GetInstance.P1Pos;
        Time.timeScale = 1;
        P1 = GameObject.Find("PlayerBall2");
        BG = GameObject.Find("Worldbg");
        Singleton.GetInstance.LeftView = false;
        BG.transform.position = Singleton.GetInstance.BGpos;
        Camera.transform.position = Singleton.GetInstance.CameraPos;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&& !GameObject.Find("Switch"))
            PressKey = true;
        else
            PressKey = false;
    }
    private void FixedUpdate()
    {
        if(!GameObject.Find("Switch"))
        {
            transform.RotateAround(P1.transform.position, Vector3.back, 3.5f);
        }
        Singleton.GetInstance.P1Pos = transform.position;
        Singleton.GetInstance.BGpos = BG.transform.position;
        Singleton.GetInstance.CameraPos = Camera.transform.position;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!GameObject.Find("Switch") && PressKey)
        {
            float Tmp = Vector3.Distance(P1.transform.position, collision.transform.position);
            if(Tmp<=1.5f && Tmp != 0.0f)
            {
                //Debug.Log(transform.name+" "+collision.transform.name);
                PressKey = false;
                if (collision.transform.name == "NextStage")
                {
                    SceneManager.LoadScene("Fire&Ice");
                }
                if (collision.transform.name == "PrevStage")
                {
                    SceneManager.LoadScene("Main");
                }
                if (collision.transform.childCount == 0)
                {
                    GameObject Obj = Instantiate(BoxPrefab);
                    Obj.transform.position = collision.transform.position;
                    Obj.transform.parent = collision.transform;
                }
                if (collision.tag == "Tile")
                {
                    float Reverse = 1.0f;
                    if (transform.position.x > collision.transform.position.x)
                        Reverse = -1.0f;
                    GameObject Obj = GameObject.Find("Worldbg");
                    if (collision.transform.name == "GoGo"&&Reverse>0.0f)
                    {
                        Camera.transform.Translate(2.0f, 0.0f, 0.0f);
                        Obj.transform.Translate(2.0f, 0.0f, 0.0f);
                        Singleton.GetInstance.LeftView = true;
                    }
                    Camera.transform.Translate(1.1f * Reverse, 0.0f, 0.0f);
                    Obj.transform.Translate(1.1f * Reverse, 0.0f, 0.0f);
                }
                if (collision.transform.name == "PrevTile"&& Singleton.GetInstance.LeftView)
                {
                    GameObject Obj = GameObject.Find("Worldbg");
                    Camera.transform.Translate(-3.1f, 0.0f, 0.0f);
                    Obj.transform.Translate(-3.1f, 0.0f, 0.0f);
                    Singleton.GetInstance.LeftView = false;
                }
                transform.position = collision.transform.position;
                Switch.SetActive(true);
            }
        }
    }
}
