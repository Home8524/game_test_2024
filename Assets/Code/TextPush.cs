using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPush : MonoBehaviour
{
    //텍스트 프리펩
    private GameObject TextPrefabs1;
    private GameObject TextPrefabs2;
    private GameObject TextPrefabs3;

    public GameObject Player1;
    public GameObject Player2;

    //텍스트 박스 띄우기용
    private GameObject TextObj;

    // Start is called before the first frame update
    void Start()
    {
        TextPrefabs1 = Resources.Load("Prefabs/Great") as GameObject;
        TextPrefabs2 = Resources.Load("Prefabs/Fast") as GameObject;
        TextPrefabs3 = Resources.Load("Prefabs/Wrong") as GameObject;
    }

    private void Update()
    {
        if(Singleton.GetInstance.StartActive&&Input.GetKeyDown(KeyCode.Space))
        {
            GameObject Obj1 = GameObject.Find("Tile " + Singleton.GetInstance.TimeNum);
            Debug.Log(Vector3.Distance(Player1.transform.position, Obj1.transform.position));
            if (Vector3.Distance(Player1.transform.position, Obj1.transform.position) < 0.5f)
                TextObj = Instantiate(TextPrefabs1);
            else if (Vector3.Distance(Player1.transform.position, Obj1.transform.position) < 1.0f)
                TextObj = Instantiate(TextPrefabs2);
              

            //성공시 텍스트 띄움
            Vector2 Pos = Singleton.GetInstance.PosSave;
            Pos.x -= 0.5f;
            Pos.y += 1.3f;
            GameObject TextBox = GameObject.Find("TextBox");
            TextObj.transform.name = "Text " + Singleton.GetInstance.TimeNum;
            TextObj.transform.parent = TextBox.transform;
            TextObj.transform.position = Pos;
        }
    }

}
