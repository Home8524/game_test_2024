using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LocalDataManager
{
    string localPath = Application.dataPath + "/Resources/Json/";

    private bool Load<T>(string fileName, out T data)
    {
        string path = localPath + fileName;
        if (File.Exists(path) == false)
        {
            data = default;
            return false;
        }

        string contents = File.ReadAllText(path);
        data = JsonUtility.FromJson<T>(contents);
        return true;
    }

    public List<TileData> LoadTileData()
    {
        Load("stage1.json", out JsonTileData data);
        return data.datas;
    }

    public bool Save<T>(string path, in T data)
    {
        string contents = JsonUtility.ToJson(data);
        File.WriteAllText(path, contents);
        return true;
    }
}
