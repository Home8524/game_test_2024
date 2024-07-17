using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallMove : MonoBehaviour
{
    [SerializeField] GameObject P1;
    private void Start()
    {
        P1 = GameObject.Find("PlayerBall1") as GameObject;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Singleton.GetInstance.P1Pos = new Vector2(6.6f, 4.7f);
            Singleton.GetInstance.P2Pos = new Vector2(5.8f, 5.7f);
            SceneManager.LoadScene("World");
        }
    }
    private void FixedUpdate()
    {
        transform.RotateAround(P1.transform.position, Vector3.back, 3.5f);
    }
}
