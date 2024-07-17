using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World3Init : MonoBehaviour
{
    void Start()
    {
        Singleton.GetInstance.BallSet = 0;
        Singleton.GetInstance.PosSave = new Vector2(5.8f, 5.7f);
        Singleton.GetInstance.TimeNum = 0;
        Singleton.GetInstance.Coll = false;
        Singleton.GetInstance.WayRoute = -1.0f;
        Singleton.GetInstance.SlowObjectGo = false;
        Singleton.GetInstance.StartActive = true;
        Singleton.GetInstance.Timer = 0.0f;
        Singleton.GetInstance.Resume = false;
    }

}
