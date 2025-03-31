using UnityEngine;
using System.Collections.Generic;
using Luban;
using UnityEngine.Windows;

public class Game : MonoBehaviour
{
    public List<int> _a = new List<int>();

    void Start()
    {
        var tables = new cfg.Tables(LoadByteBuf);

        foreach (var hero in tables.TbHero.DataList)
            Debug.Log($">> {hero}");
    }

    private static ByteBuf LoadByteBuf(string file)
    {
        return new ByteBuf(File.ReadAllBytes($"{Application.dataPath}/Packs/DataTables/{file}.bytes"));
    }

    public void Init()
    {
        _a.Add(1);
        _a.Add(5);
    }
}
