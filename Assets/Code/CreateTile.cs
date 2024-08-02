using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTile : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab1;
    [SerializeField] private GameObject tilePrefab2;

    private Stack<GameObject> defaultObjectContainer;
    private Stack<GameObject> rotateObjectContainer;

    [SerializeField] private Transform defaultParent;
    [SerializeField] private Transform rotateParent;

    Vector2 Size = new Vector2(1.3f, 1.2f); //1.3,1.2
    Vector2 Size2 = new Vector2(1.2f, 1.0f);
    GameObject root;

    private void Awake()
    {
        tilePrefab1 = Resources.Load("Prefabs/b1") as GameObject;
        tilePrefab2 = Resources.Load("Prefabs/RotateTile") as GameObject;
        root = GameObject.Find("Tile");
        
        tilePrefab1.AddComponent<BoxCollider2D>();
        BoxCollider2D Box = tilePrefab1.GetComponent<BoxCollider2D>();
        Box.size = Size;
        Box.isTrigger = true;

        
        tilePrefab2.AddComponent<BoxCollider2D>();
        BoxCollider2D Box2 = tilePrefab2.GetComponent<BoxCollider2D>();
        Box2.size = Size2;
        Box2.isTrigger = true;
        

        defaultObjectContainer = new Stack<GameObject>();
        rotateObjectContainer = new Stack<GameObject>();

        CreateObjectPool();
    }

    private void CreateObjectPool()
    {
        CreateDefaultObjectPool();
        CreateRotateObjectPool();
    }

    private void CreateDefaultObjectPool()
    {
        for(int i = 0; i < 100; i ++)
        {
            GameObject obj = Instantiate(tilePrefab1);
            obj.transform.SetParent(defaultParent);
            defaultObjectContainer.Push(obj);
        }
    }

    private void CreateRotateObjectPool()
    {
        for(int i = 0; i < 50; i ++)
        {
            GameObject obj = Instantiate(tilePrefab2);
            obj.transform.SetParent(rotateParent);
            rotateObjectContainer.Push(obj);
        }
    }

    void LoadTile()
    {
        int tileNum = 0;

        var jsonData = Managers.dataManager.LoadStageData(Singleton.GetInstance.playStage);
        var dataList = jsonData.datas;

        foreach(var data in dataList)
        {
            GameObject go;

            if (defaultObjectContainer.Count == 0) CreateDefaultObjectPool();
            if (rotateObjectContainer.Count == 0) CreateRotateObjectPool();

            if (data.isRotate == false) go = defaultObjectContainer.Pop();
            else go = rotateObjectContainer.Pop();

            go.transform.SetParent(root.transform);

            go.transform.position = new Vector2(float.Parse(data.pos.x), float.Parse(data.pos.y));
            go.transform.rotation = Quaternion.Euler(new Vector3(0,0,data.rotate));
            go.transform.name = "Tile " + tileNum;
            tileNum++;
        }
    }

    void Start()
    {
        LoadTile();
    }

}
