using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    Transform uiCanvas;
    Transform textCanvas;
    Transform blurCanvas;
    Transform volumeCanvas;
    Transform readyText;

    //ui 연관된 캔버스는 해당 매니저에서 관리, 기능작동
    UnityAction readyTextEvent;

    private void Awake()
    {
        Managers.uiManager = this;
    }

    public void SetReadyTextEvent(UnityAction onAction)
    {
        readyTextEvent = onAction;
    }

    public void LoadUI()
    {
        GameObject rootCanvas = GameObject.Find("CanvasRoot");

        uiCanvas = Utils.FindComp<Transform>(rootCanvas.transform, "UI");
        textCanvas = Utils.FindComp<Transform>(rootCanvas.transform, "Text_Canvas");
        blurCanvas = Utils.FindComp<Transform>(rootCanvas.transform, "Blur");
        volumeCanvas = Utils.FindComp<Transform>(rootCanvas.transform, "VolumeSet");
        readyText = Utils.FindComp<Transform>(textCanvas, "Ready");
    }

    private void DisableAll()
    {
        uiCanvas.gameObject.SetActive(false);
        textCanvas.gameObject.SetActive(false);
        blurCanvas.gameObject.SetActive(false);
        volumeCanvas.gameObject.SetActive(false);
    }

    public void ActiveUi(eCanvas type)
    {
        DisableAll();

        if(type == eCanvas.text)
        {
            textCanvas.gameObject.SetActive(true);
            return;
        }

        blurCanvas.gameObject.SetActive(true);
        switch (type)
        {
            case eCanvas.ui:
                uiCanvas.gameObject.SetActive(true);
                break;
            case eCanvas.volume:
                volumeCanvas.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void ActiveReadyText()
    {
        readyText.gameObject.SetActive(true);
        readyTextEvent?.Invoke();
    }
}
