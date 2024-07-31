using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartButton : MonoBehaviour
{
    public Text CanvansText;
    //Active에 할당된 캔버스와 volumeset에 할당된 캔버스가
    //각각 다름
    public void Active()
    {
        Time.timeScale = 1;
        Singleton.GetInstance.Resume = true;

        Managers.uiManager.ActiveUi(eCanvas.text);
        Managers.uiManager.ActiveReadyText();
    }
    public void Restart()
    {
        Singleton.GetInstance.SlowObjectGo = false;
        Singleton.GetInstance.StartActive = true;
        Singleton.GetInstance.Resume = false;
        SceneManager.LoadScene("Fire&Ice");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void VolumeSet()
    {
        Managers.uiManager.ActiveUi(eCanvas.volume);
    }
    public void Back()
    {
        SceneManager.LoadScene("World");
    }
}
