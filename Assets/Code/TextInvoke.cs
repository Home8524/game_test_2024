using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInvoke : MonoBehaviour
{
    private bool Check;
    TextMesh MeshText;
    void Start()
    {
        Check = false;
        Invoke("AlphaObj", 1.0f);
        MeshText = transform.GetComponent<TextMesh>();
    }
    private void Update()
    {
        if(Check)
        {
            Color AlphaColor;
            AlphaColor = MeshText.color;
            AlphaColor.a -= 0.01f;
            MeshText.color = AlphaColor;
            if (AlphaColor.a < 0.0f)
                Destroy(gameObject);
        }
    }
    void AlphaObj()
    {
        Check = true;
    }
}
