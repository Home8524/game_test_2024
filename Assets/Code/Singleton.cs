using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton 
{
    static private Singleton Instance;
    static public Singleton GetInstance
    {
        get
        {
            if (Instance == null)
            {
                Instance = new Singleton();
            }
            return Instance;
        }
    }

    // 0 : Ball1 , 1 : Ball2
    public int BallSet = 0;
    public Vector2 PosSave = new Vector2(5.8f,5.7f);
    public int TimeNum = 0;
    public bool Coll = false;
    public float WayRoute = -1.0f;
    public bool SlowObjectGo = false;
    public bool StartActive = true;
    public bool Resume = false;
    public float Timer = 0.0f;
    public bool Die = false;
    public bool LeftView = false;
    public Vector2 P1Pos = new Vector2(6.6f, 4.7f);
    public Vector2 P2Pos = new Vector2(5.8f, 5.7f);
    public Vector3 CameraPos = new Vector3(7.06f, 4.69f, -9.0f);
    public Vector3 BGpos = new Vector2(7.0f, 4.44f);
}
