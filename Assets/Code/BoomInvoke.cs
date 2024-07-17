using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomInvoke : MonoBehaviour
{
    void Start()
    {
        Invoke("TimeStop",0.4f);
    }

    private void TimeStop()
    {
        Time.timeScale = 0;
    }

}
