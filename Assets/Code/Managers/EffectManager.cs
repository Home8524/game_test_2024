using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

//인게임 내에서 사용하는 연출들을 관리하는 매니저
public class EffectManager : MonoBehaviour
{
    private List<SpriteRenderer> renderList;
    private Image flash;
    private SpriteRenderer endBg;

    private void Awake()
    {
        Managers.effectManager = this;   
    }

    public void SetObject()
    {
        renderList = new List<SpriteRenderer>();

        GameObject moveObj = GameObject.Find("MovingObject");
        GameObject stopObj = GameObject.Find("StopObject");
        GameObject bg = GameObject.Find("BackGround");
        SpriteRenderer bgRender = Utils.FindComp<SpriteRenderer>(bg.transform, "");
        bgRender.color = Color.white;
        renderList.Add(bgRender);
        endBg = Utils.FindComp<SpriteRenderer>(bg.transform, "EndBG");
        endBg.color = new Color(1, 1, 1, 0);

        for(int i = 0; i < moveObj.transform.childCount; i++)
        {
            Transform child = moveObj.transform.GetChild(i);
            SpriteRenderer render = Utils.FindComp<SpriteRenderer>(child, "");
            renderList.Add(render);
        }

        for (int i = 0; i < stopObj.transform.childCount; i++)
        {
            Transform child = stopObj.transform.GetChild(i);
            SpriteRenderer render = Utils.FindComp<SpriteRenderer>(child, "");
            renderList.Add(render);
        }

        GameObject flashObj = GameObject.Find("Flash");
        flash = Utils.FindComp<Image>(flashObj.transform, "");
    }

    public void ClearAction()
    {
        endBg.color = Color.white;
        ActiveFlash();
        renderList.Clear();
    }
    public void EffectAction(EffectData data)
    {
        Debug.Log("start action - type : " + data.type);

        switch(data.type)
        {
            case "flash":
                ActiveFlash();
                break;
            case "color":
                CustomColor custom = data.color;
                Color color = new Color(custom.r, custom.g, custom.b);
                ChangeColor(color);
                break;
            default:
                break;
        }
    }

    private void ChangeColor(Color color)
    {
        foreach(var render in renderList)
        {
            if (render == null) continue;

            render.color = color;
        }
    }

    private void ActiveFlash()
    {
        flash.color = Color.white;

        StartCoroutine(coFlash());
    }

    private IEnumerator coFlash()
    {
        float dt = Time.deltaTime * 3.0f;
        Color color = Color.white;

        while(true)
        {
            if (color.a <= 0) break;

            color.a -= dt;
            flash.color = color;

            yield return new WaitForSeconds(dt);
        }
    }
}
