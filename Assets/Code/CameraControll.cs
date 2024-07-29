using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CameraControll : MonoBehaviour
{
    private Vector3 SavePos;

    public void SetPos(Vector3 pos)
    {
        transform.position += pos;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Bounce();
        if (Singleton.GetInstance.Die&&Input.GetKeyDown(KeyCode.Space))
            Restart();
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
        Singleton.GetInstance.Resume = false;
        Singleton.GetInstance.Die = false;
        SceneManager.LoadScene("Fire&Ice");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
