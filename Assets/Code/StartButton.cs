using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartButton : MonoBehaviour
{
    public Text CanvansText;
    //Active�� �Ҵ�� ĵ������ volumeset�� �Ҵ�� ĵ������
    //���� �ٸ�
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
