using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartButton : MonoBehaviour
{
    public GameObject Canvas1;
    public GameObject Canvas2;
    public GameObject Canvas3;
    public Text CanvansText;
    //Active에 할당된 캔버스와 volumeset에 할당된 캔버스가
    //각각 다름
    public void Active()
    {
        Canvas1.SetActive(false);
        Canvas3.SetActive(false);
        Canvas2.SetActive(true);
        Time.timeScale = 1;
        CanvansText.gameObject.SetActive(true);
        Singleton.GetInstance.Timer = 0.0f;
        Singleton.GetInstance.Resume = true;
    }
    public void Restart()
    {
        Singleton.GetInstance.BallSet = 0;
        Singleton.GetInstance.PosSave = new Vector2(0.0f, 0.0f);
        Singleton.GetInstance.TimeNum = 0;
        Singleton.GetInstance.Coll = false;
        Singleton.GetInstance.WayRoute = -1.0f;
        Singleton.GetInstance.SlowObjectGo = false;
        Singleton.GetInstance.StartActive = true;
        Singleton.GetInstance.Timer = 0.0f;
        Singleton.GetInstance.Resume = false;
        SceneManager.LoadScene("Fire&Ice");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolumeSet()
    {
        Canvas2.SetActive(true);
        Canvas1.SetActive(false);
    }
    public void Back()
    {
        SceneManager.LoadScene("World");
    }
}
