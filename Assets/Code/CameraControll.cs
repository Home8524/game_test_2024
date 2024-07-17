using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CameraControll : MonoBehaviour
{
    private int TileNum;
    private Vector3 SavePos;
    private void Start()
    {
        TileNum = 0;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Bounce();
        if (Singleton.GetInstance.Die&&Input.GetKeyDown(KeyCode.Space))
            Restart();
    }
    private void FixedUpdate()
    {
        int Tmp = Singleton.GetInstance.TimeNum;
        if (TileNum != Tmp)
        {
            TileNum++;
            if (TileNum == 23 || TileNum == 14 || TileNum > 32 && TileNum < 45
               || TileNum > 48 && TileNum < 61 || TileNum > 102 && TileNum < 106
               || TileNum > 110 && TileNum < 114 || TileNum > 118 && TileNum < 122
               || TileNum > 126 && TileNum < 130)
                transform.Translate(0.0f, -0.5f, 0.0f);
            else if (TileNum > 64 && TileNum < 91)
                transform.position += Vector3.right * 0.4f *
                -1.0f * Singleton.GetInstance.WayRoute;
            else if (TileNum > 130 && TileNum < 161)
            {
                int TileCheck = (TileNum - 130) % 8;
                if (TileCheck == 7)
                    transform.Translate(0.0f, -0.5f, 0.0f);
                else
                    transform.Translate(0.4f * Singleton.GetInstance.WayRoute * -1.0f, 0.0f, 0.0f);
            }
            else
                transform.position += Vector3.right * 1.2f *
                -1.0f * Singleton.GetInstance.WayRoute;
        }
    }
    private void Bounce()
    {
        SavePos = transform.position;
        Vector3 Cont;
        Cont = SavePos;
        Cont.z += 0.3f;
        transform.position = Cont;
        Invoke("Reset", 0.1f);
    }
    private void Reset()
    {
        SavePos.x = transform.position.x;
        SavePos.y = transform.position.y;
        transform.position = SavePos;
    }
    private void Restart()
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
        Singleton.GetInstance.Die = false;
        SceneManager.LoadScene("Fire&Ice");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
