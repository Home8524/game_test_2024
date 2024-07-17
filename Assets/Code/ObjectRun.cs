using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRun : MonoBehaviour
{
    float Speed;

    void Start()
    {
        if (transform.tag == "Slow")
            Speed = 5.0f;
        else if (transform.tag == "Fast")
            Speed = 10.0f;
        else
            Speed = 15.0f;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Singleton.GetInstance.StartActive)
        { 
           if (gameObject.layer != 7 || Singleton.GetInstance.SlowObjectGo)
           {
               transform.Translate(Speed * Time.deltaTime * -1.0f, 0.0f, 0.0f);
               if (transform.position.x < -100.0f)
                   Destroy(gameObject);
           }
           if(gameObject.layer!=7&&Singleton.GetInstance.TimeNum==33)
           {
               Color Tmp = new Color(153.0f / 255.0f, 83.0f / 255.0f, 1.0f);
               transform.GetComponent<SpriteRenderer>().color = Tmp;
           }
        }


    }
}
