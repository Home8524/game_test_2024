using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInvoke : MonoBehaviour
{
    public GameObject B1;
    public GameObject B2;
    private void Start()
    {
        B2.SetActive(true);
        Invoke("Box1", 0.0f);
    }
    void Box1()
    {
        B2.SetActive(false);
        B1.SetActive(true);
        Invoke("Box2", 0.3f);
    }
    void Box2()
    {
        B1.SetActive(false);
        B2.SetActive(true);
        Invoke("Box1", 0.3f);
    }
}
