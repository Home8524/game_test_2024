using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class JsonStageData
{
    public List<EffectData> effects;
    public List<TileData> datas;
}

[Serializable]
public class EffectData
{
    public string type;
    public int tile;
    public CustomColor color;
}

[Serializable]
public class CustomColor
{
    public float r;
    public float g;
    public float b;
}

[Serializable]
public class TileData
{
    public MoveVector pos;
    public int rotate;
    public bool isRotate;
}

[Serializable]
public class MoveVector
{
    public string x;
    public string y;

    public MoveVector(float x, float y)
    {
        this.x = x.ToString();
        this.y = y.ToString();
    }
}
