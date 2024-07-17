using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightInvoke : MonoBehaviour
{
    void Start()
    {
        Invoke("Alpha", 3.0f);
    }

    void Alpha()
    {
        SpriteRenderer Tmp = transform.GetComponent<SpriteRenderer>();
      //  Debug.Log(Tmp.color);
        Color AlphaColor;
        AlphaColor = Tmp.color;
        AlphaColor.a = 0.5f;
        Tmp.color = AlphaColor;
    }

}
