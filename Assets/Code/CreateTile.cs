using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTile : MonoBehaviour
{
    [SerializeField] private GameObject TilePrefab1;
    [SerializeField] private GameObject TilePrefab2;
    private int Swap=0;
    private int Change = 1;
    private float Gajunchi1_x = 0;
    private float Gajunchi1_y = 0f;
    private float Gajunchi2_x = 0;
    private float Gajunchi2_y = 0;
    private void Awake()
    {
        TilePrefab1 = Resources.Load("Prefabs/b1") as GameObject;
        TilePrefab2 = Resources.Load("Prefabs/RotateTile") as GameObject;
    }
    void Start()
    {
        //Time Create
        //윗줄 12개
        Vector2 Size = new Vector2(1.3f, 1.2f); //1.3,1.2
        Vector2 Size2 = new Vector2(1.2f, 1.0f);
        {
            GameObject Tmp = GameObject.Find("Tile");

            GameObject Obj = Instantiate(TilePrefab1);
            Obj.transform.name = "Root Tile";
            Obj.transform.parent = Tmp.transform;

            Vector2 Pos = new Vector2(5.8f,5.7f);
            Obj.transform.position = Pos;
        }
        for (int i=0; i<12; ++i)
        {
            GameObject Tmp = GameObject.Find("Tile");

            GameObject Obj = Instantiate(TilePrefab1);
            Obj.transform.name = "Tile "+ i;
            Obj.transform.parent = Tmp.transform;
            
            Vector2 Pos =new Vector2();
            Pos.y = Obj.transform.position.y;
            Pos.x = Obj.transform.position.x + 1.2f * i;
            Obj.transform.position = Pos;

            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size;
            Box.isTrigger = true;

                
        }
        //방향 전환용 2개
        for(int i=12; i<14; ++i)
        {
            GameObject Tmp = GameObject.Find("Tile");

            GameObject Obj = Instantiate(TilePrefab2);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;

            Vector2 Pos = new Vector2(21.3f,5.715f);
            Pos.y = Pos.y - (i - 12);
            Pos.x = Pos.x + ((i - 12) * 0.02f);
            if(i==12)
                Obj.transform.Rotate(0.0f, 0.0f, -90.0f);
            else
                Obj.transform.Rotate(0.0f, 0.0f, -180.0f);
            Obj.transform.position = Pos;

            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size2;
            Box.isTrigger = true;
        }
        //두번재 줄 7개
        for(int i =14; i<21; ++i)
        {
            GameObject Tmp = GameObject.Find("Tile");

            GameObject Obj = Instantiate(TilePrefab1);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;

            Vector2 Pos = new Vector2(20.2f,4.72f);
            Pos.x = Pos.x - 1.2f * (i - 14);
            Obj.transform.position = Pos;
            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size;
            Box.isTrigger = true;

        }
        //방향 전환용 2개
        for(int i=21; i<23; ++i)
        {
            GameObject Tmp = GameObject.Find("Tile");

            GameObject Obj = Instantiate(TilePrefab2);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;

            Vector2 Pos = new Vector2(11.92f, 4.72f);
            Pos.y -= (i - 21);
            Obj.transform.position = Pos;
            if (i == 22)
                Obj.transform.Rotate(0.0f, 0.0f, 90.0f);
            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size2;
            Box.isTrigger = true;
        }
        //세번째 줄 9개
        for(int i=23; i<32; ++i)
        {
            GameObject Tmp = GameObject.Find("Tile");

            GameObject Obj = Instantiate(TilePrefab1);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;
            Vector2 Pos= new Vector2(13.0f, 3.74f);
            Pos.x += (i - 23) * 1.2f;
            Obj.transform.position = Pos;
            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size;
            Box.isTrigger = true;
        }
        //세로줄 12개
        for (int i=32; i<44; ++i)
        {
            if (Swap == 2)
            {
                Swap = 0;
                Change = Change * -1;
                Gajunchi1_y += 0.02f;           
            }
            //Tmp가 1일땐 우측, -1일땐 좌측
            GameObject Tmp = GameObject.Find("Tile");

            GameObject Obj = Instantiate(TilePrefab2);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;

            if(Change==1)
            {
                Vector2 Pos = new Vector2(23.7f, 3.76f);
                Pos.x += Gajunchi1_x;
                Pos.y += Gajunchi1_y;
                Obj.transform.position = Pos;
                Gajunchi1_x += 0.02f;
                Gajunchi1_y -= 1.0f;
                if(Swap==0)
                    Obj.transform.Rotate(0.0f, 0.0f, -90.0f);
                else
                    Obj.transform.Rotate(0.0f, 0.0f, -180.0f);
            }
            else
            {
                Vector2 Pos = new Vector2(22.72f, 2.76f);
                Pos.x += Gajunchi2_x;
                Pos.y += Gajunchi2_y;
                Obj.transform.position = Pos;
                if(Swap!=0)
                {
                    Obj.transform.Rotate(0.0f, 0.0f, 90.0f);
                    Gajunchi2_y += 0.04f;
                }

                Gajunchi2_x += 0.02f;
                Gajunchi2_y -= 1.0f;   
            }

            ++Swap;

            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size2;
            Box.isTrigger = true;
        }
        //가로줄 4개
        for(int i=44; i<48; ++i)
        {
            GameObject Tmp = GameObject.Find("Tile");
            GameObject Obj = Instantiate(TilePrefab1);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;
            Vector2 Pos = new Vector2(23.92f, -2.14f);
            Pos.x += (i - 44) * 1.2f;
            Obj.transform.position = Pos;
            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size;
            Box.isTrigger = true;
        }
        //세로줄 12개
        for(int i=48; i<60; ++i)
        {
            if(i==48)
            {
                Gajunchi1_x = 0;
                Gajunchi1_y = 0;
                Gajunchi2_x = 0;
                Gajunchi2_y = 0;
                Change = 1;
                Swap = 0;
            }
            if (Swap == 2)
            {
                Swap = 0;
                Change = Change * -1;
                Gajunchi1_y += 0.02f;
            }
            //Tmp가 1일땐 우측, -1일땐 좌측
            GameObject Tmp = GameObject.Find("Tile");

            GameObject Obj = Instantiate(TilePrefab2);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;

            if (Change == 1)
            {
                Vector2 Pos = new Vector2(28.62f, -2.13f);
                Pos.x += Gajunchi1_x;
                Pos.y += Gajunchi1_y;
                Obj.transform.position = Pos;
                Gajunchi1_x += 0.02f;
                Gajunchi1_y -= 1.0f;
                if (Swap == 0)
                    Obj.transform.Rotate(0.0f, 0.0f, -90.0f);
                else
                    Obj.transform.Rotate(0.0f, 0.0f, -180.0f);
            }
            else
            {
                Vector2 Pos = new Vector2(27.64f, -3.13f);
                Pos.x += Gajunchi2_x;
                Pos.y += Gajunchi2_y;
                Obj.transform.position = Pos;
                if (Swap != 0)
                {
                    Obj.transform.Rotate(0.0f, 0.0f, 90.0f);
                    Gajunchi2_y += 0.04f;
                }

                Gajunchi2_x += 0.02f;
                Gajunchi2_y -= 1.0f;
            }

            ++Swap;

            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size2;
            Box.isTrigger = true;
        }
        //가로줄 4개
        for(int i=60; i<64; ++i)
        {
            GameObject Tmp = GameObject.Find("Tile");
            GameObject Obj = Instantiate(TilePrefab1);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;
            Vector2 Pos = new Vector2(28.74f, -8.04f);
            Pos.x += (i - 60) * 1.2f;
            Obj.transform.position = Pos;
            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size;
            Box.isTrigger = true;
        }
        //가로 트위스트 26
        for(int i=64; i<90; ++i)
        {
            //4*6 + 2
            GameObject Tmp = GameObject.Find("Tile");
            GameObject Obj = Instantiate(TilePrefab2);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;
            if(i==64)
            {
                Gajunchi1_x = 0;
                Gajunchi1_y = 0;
            }
            // 33.45 ,-8.04 / 33.43,-7.04 /34.43,-7.02 /34.43,-8.02
            //     180             0           -90         90
            // 다음 step x +=1 ,y +=0.2
            int Twist = (i - 64) % 4;
            if (i<88)
            {
                Vector2 Pos = new Vector2(33.45f, -8.04f);
                Pos.x += Gajunchi1_x;
                Pos.y += Gajunchi1_y;
                Obj.transform.position = Pos;

                switch(Twist)
                {
                    case 0:
                        Obj.transform.Rotate(0.0f, 0.0f, 180.0f);
                        Gajunchi1_x -= 0.02f;
                        Gajunchi1_y += 1.0f;
                        break;
                    case 1:
                        Gajunchi1_x += 1.0f;
                        Gajunchi1_y += 0.02f;
                        break;
                    case 2:
                        Obj.transform.Rotate(0.0f, 0.0f, -90.0f);
                        Gajunchi1_y -= 1.0f;
                        break;
                    default:
                        Obj.transform.Rotate(0.0f, 0.0f, 90.0f);
                        Gajunchi1_x += 1.0f;
                        Gajunchi1_y += 0.02f;
                        break;
                }

            }
            else
            {
                Vector2 Pos = new Vector2(33.45f, -8.04f);
                Pos.x += Gajunchi1_x;
                Pos.y += Gajunchi1_y;
                Obj.transform.position = Pos;

                switch (Twist)
                {
                    case 0:
                        Obj.transform.Rotate(0.0f, 0.0f, 180.0f);
                        Gajunchi1_x -= 0.02f;
                        Gajunchi1_y += 1.0f;
                        break;
                    default:
                        break;
                }
            }

            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size2;
            Box.isTrigger = true;
        }
        //가로줄 11개
        for(int i=90; i<101; ++i)
        {
            GameObject Tmp = GameObject.Find("Tile");
            GameObject Obj = Instantiate(TilePrefab1);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;
            Vector2 Pos = new Vector2(46.41f, -6.8f);
            Pos.x += (i - 90) * 1.2f;
            Obj.transform.position = Pos;
            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size;
            Box.isTrigger = true;
        }
        //세로줄 4개
        for(int i=101; i<105; ++i)
        {
            if (i == 101)
            {
                Gajunchi1_x = 0;
                Gajunchi1_y = 0;
                Gajunchi2_x = 0;
                Gajunchi2_y = 0;
                Change = 1;
                Swap = 0;
            }
            if (Swap == 2)
            {
                Swap = 0;
                Change = Change * -1;
                Gajunchi1_y += 0.02f;
            }
            //Tmp가 1일땐 우측, -1일땐 좌측
            GameObject Tmp = GameObject.Find("Tile");

            GameObject Obj = Instantiate(TilePrefab2);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;

            if (Change == 1)
            {
                Vector2 Pos = new Vector2(59.5f, -6.78f);
                Pos.x += Gajunchi1_x;
                Pos.y += Gajunchi1_y;
                Obj.transform.position = Pos;
                Gajunchi1_x += 0.02f;
                Gajunchi1_y -= 1.0f;
                if (Swap == 0)
                    Obj.transform.Rotate(0.0f, 0.0f, -90.0f);
                else
                    Obj.transform.Rotate(0.0f, 0.0f, -180.0f);
            }
            else
            {
                Vector2 Pos = new Vector2(58.52f, -7.78f);
                Pos.x += Gajunchi2_x;
                Pos.y += Gajunchi2_y;
                Obj.transform.position = Pos;
                if (Swap != 0)
                {
                    Obj.transform.Rotate(0.0f, 0.0f, 90.0f);
                    Gajunchi2_y += 0.04f;
                }

                Gajunchi2_x += 0.02f;
                Gajunchi2_y -= 1.0f;
            }

            ++Swap;

            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size2;
            Box.isTrigger = true;
        }
        //가로줄4개
        for(int i=105; i<109; ++i)
        {
            GameObject Tmp = GameObject.Find("Tile");
            GameObject Obj = Instantiate(TilePrefab1);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;
            Vector2 Pos = new Vector2(59.63f, -8.76f);
            Pos.x += (i - 105) * 1.2f;
            Obj.transform.position = Pos;
            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size;
            Box.isTrigger = true;
        }
        //세로줄 4개
        for (int i = 109; i < 113; ++i)
        {
            if (i == 109)
            {
                Gajunchi1_x = 0;
                Gajunchi1_y = 0;
                Gajunchi2_x = 0;
                Gajunchi2_y = 0;
                Change = 1;
                Swap = 0;
            }
            if (Swap == 2)
            {
                Swap = 0;
                Change = Change * -1;
                Gajunchi1_y += 0.02f;
            }
            //Tmp가 1일땐 우측, -1일땐 좌측
            GameObject Tmp = GameObject.Find("Tile");

            GameObject Obj = Instantiate(TilePrefab2);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;

            if (Change == 1)
            {
                Vector2 Pos = new Vector2(64.32f, -8.74f);
                Pos.x += Gajunchi1_x;
                Pos.y += Gajunchi1_y;
                Obj.transform.position = Pos;
                Gajunchi1_x += 0.02f;
                Gajunchi1_y -= 1.0f;
                if (Swap == 0)
                    Obj.transform.Rotate(0.0f, 0.0f, -90.0f);
                else
                    Obj.transform.Rotate(0.0f, 0.0f, -180.0f);
            }
            else
            {
                Vector2 Pos = new Vector2(63.34f, -9.74f);
                Pos.x += Gajunchi2_x;
                Pos.y += Gajunchi2_y;
                Obj.transform.position = Pos;
                if (Swap != 0)
                {
                    Obj.transform.Rotate(0.0f, 0.0f, 90.0f);
                    Gajunchi2_y += 0.04f;
                }

                Gajunchi2_x += 0.02f;
                Gajunchi2_y -= 1.0f;
            }

            ++Swap;

            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size2;
            Box.isTrigger = true;
        }
        //가로줄4개
        for (int i = 113; i < 117; ++i)
        {
            GameObject Tmp = GameObject.Find("Tile");
            GameObject Obj = Instantiate(TilePrefab1);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;
            Vector2 Pos = new Vector2(64.45f, -10.72f);
            Pos.x += (i - 113) * 1.2f;
            Obj.transform.position = Pos;
            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size;
            Box.isTrigger = true;
        }
        //세로줄 4개
        for (int i = 117; i < 121; ++i)
        {
            if (i == 117)
            {
                Gajunchi1_x = 0;
                Gajunchi1_y = 0;
                Gajunchi2_x = 0;
                Gajunchi2_y = 0;
                Change = 1;
                Swap = 0;
            }
            if (Swap == 2)
            {
                Swap = 0;
                Change = Change * -1;
                Gajunchi1_y += 0.02f;
            }
            //Tmp가 1일땐 우측, -1일땐 좌측
            GameObject Tmp = GameObject.Find("Tile");

            GameObject Obj = Instantiate(TilePrefab2);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;

            if (Change == 1)
            {
                Vector2 Pos = new Vector2(69.14f, -10.7f);
                Pos.x += Gajunchi1_x;
                Pos.y += Gajunchi1_y;
                Obj.transform.position = Pos;
                Gajunchi1_x += 0.02f;
                Gajunchi1_y -= 1.0f;
                if (Swap == 0)
                    Obj.transform.Rotate(0.0f, 0.0f, -90.0f);
                else
                    Obj.transform.Rotate(0.0f, 0.0f, -180.0f);
            }
            else
            {
                Vector2 Pos = new Vector2(68.16f, -11.7f);
                Pos.x += Gajunchi2_x;
                Pos.y += Gajunchi2_y;
                Obj.transform.position = Pos;
                if (Swap != 0)
                {
                    Obj.transform.Rotate(0.0f, 0.0f, 90.0f);
                    Gajunchi2_y += 0.04f;
                }

                Gajunchi2_x += 0.02f;
                Gajunchi2_y -= 1.0f;
            }

            ++Swap;

            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size2;
            Box.isTrigger = true;
        }
        //가로줄 4개
        for (int i = 121; i < 125; ++i)
        {
            GameObject Tmp = GameObject.Find("Tile");
            GameObject Obj = Instantiate(TilePrefab1);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;
            Vector2 Pos = new Vector2(69.27f, -12.68f);
            Pos.x += (i - 121) * 1.2f;
            Obj.transform.position = Pos;
            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size;
            Box.isTrigger = true;
        }
        //세로줄 4개
        for (int i = 125; i < 129; ++i)
        {
            if (i == 125)
            {
                Gajunchi1_x = 0;
                Gajunchi1_y = 0;
                Gajunchi2_x = 0;
                Gajunchi2_y = 0;
                Change = 1;
                Swap = 0;
            }
            if (Swap == 2)
            {
                Swap = 0;
                Change = Change * -1;
                Gajunchi1_y += 0.02f;
            }
            //Tmp가 1일땐 우측, -1일땐 좌측
            GameObject Tmp = GameObject.Find("Tile");

            GameObject Obj = Instantiate(TilePrefab2);
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;

            if (Change == 1)
            {
                Vector2 Pos = new Vector2(73.96f, -12.66f);
                Pos.x += Gajunchi1_x;
                Pos.y += Gajunchi1_y;
                Obj.transform.position = Pos;
                Gajunchi1_x += 0.02f;
                Gajunchi1_y -= 1.0f;
                if (Swap == 0)
                    Obj.transform.Rotate(0.0f, 0.0f, -90.0f);
                else
                    Obj.transform.Rotate(0.0f, 0.0f, -180.0f);
            }
            else
            {
                Vector2 Pos = new Vector2(72.98f, -13.66f);
                Pos.x += Gajunchi2_x;
                Pos.y += Gajunchi2_y;
                Obj.transform.position = Pos;
                if (Swap != 0)
                {
                    Obj.transform.Rotate(0.0f, 0.0f, 90.0f);
                    Gajunchi2_y += 0.04f;
                }

                Gajunchi2_x += 0.02f;
                Gajunchi2_y -= 1.0f;
            }

            ++Swap;

            Obj.AddComponent<BoxCollider2D>();
            BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
            Box.size = Size2;
            Box.isTrigger = true;
        }
        //마지막 구간
        for(int i=129; i<160; ++i)
        {
            if(i==129)
            { 
                    Gajunchi1_x = 0;
                    Gajunchi1_y = 0;
            }
            int TileCheck = i - 129;
            TileCheck = TileCheck % 8;

            GameObject Tmp = GameObject.Find("Tile");
            GameObject Obj;
            if (TileCheck==0||TileCheck==3||i==159)
            {
                 Obj = Instantiate(TilePrefab1);
                Obj.AddComponent<BoxCollider2D>();
                BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
                Box.size = Size;
                Box.isTrigger = true;
            }
            else
            {
                Obj = Instantiate(TilePrefab2);
                Obj.AddComponent<BoxCollider2D>();
                BoxCollider2D Box = Obj.GetComponent<BoxCollider2D>();
                Box.size = Size2;
                Box.isTrigger = true;
            }
            Obj.transform.name = "Tile " + i;
            Obj.transform.parent = Tmp.transform;
            Vector2 Pos = new Vector2(73.0f, -14.66f);
            switch(TileCheck)
            {
                case 0:
                    Gajunchi1_x += 1.1f;
                    Gajunchi1_y += 0.02f;
                    break;
                case 1:
                    Gajunchi1_x += 1.1f;
                    Obj.transform.Rotate(0.0f, 0.0f, 180.0f);
                    break;
                case 2:
                    Gajunchi1_x -= 0.02f;
                    Gajunchi1_y += 1.0f;
                    break;
                case 3:
                    Gajunchi1_x += 1.1f;
                    break;
                case 4:
                    Gajunchi1_x += 1.08f;
                    Gajunchi1_y += 0.02f;
                    Obj.transform.Rotate(0.0f, 0.0f, -90.0f);
                    break;
                case 5:
                    Gajunchi1_x += 0.02f;
                    Gajunchi1_y -= 1.0f;
                    Obj.transform.Rotate(0.0f, 0.0f, 180.0f);
                    break;
                case 6:
                    Gajunchi1_x -= 1.0f;
                    break;
                default:
                    Gajunchi1_x += 0.02f;
                    Gajunchi1_y -= 1.0f;
                    Obj.transform.Rotate(0.0f, 0.0f, 90.0f);
                    break;
            }
            if (i == 159)
                Gajunchi1_x -= 0.1f;
            Pos.x += Gajunchi1_x;
            Pos.y += Gajunchi1_y;
            Obj.transform.position = Pos;
        }    
}

}
