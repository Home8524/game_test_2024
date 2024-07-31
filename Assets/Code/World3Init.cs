using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World3Init : MonoBehaviour
{
    void Start()
    {
        Singleton.GetInstance.SlowObjectGo = false;
        Singleton.GetInstance.StartActive = true;
        Singleton.GetInstance.Resume = false;
    }

}
