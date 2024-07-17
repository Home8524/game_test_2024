using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class JsonTileData
{
    public List<TileData> datas;
}

[Serializable]
public class TileData
{
    public MoveVector startPos;
    public bool isRotate;
    public int rotateVal;
    public int cnt;
    public MoveVector movePos;
}

public class MoveVector
{
    public float x;
    public float y;
}
