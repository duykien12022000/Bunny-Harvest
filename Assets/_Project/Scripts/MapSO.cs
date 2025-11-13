using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MapData", menuName = "Data/MapData")]
public class MapSO : ScriptableObject
{
    [SerializeField] MapData[] maps;
    public int GetLenght() { return maps.Length; }
    [ContextMenu("GetIndex")]
    void GetIndex()
    {
        for (int i = 0; i < maps.Length; i++)
        {
            int index = i;
            maps[i].id = index;
            maps[i].radish = 1000 * index;
        }
    }
    public MapData GetMap(int id)
    {
        for (int i = 0; i < maps.Length; i++)
        {
            if (maps[i].id == id)
                return maps[i];

        }
        Debug.Log($"Fuck you! Map {id} doesn't exist!");
        return null;
    }
}
[Serializable]
public class MapData
{
    public int id;
    public int radish;
    public Sprite demoUI;
    public MapController prefabs;
    public int isBought
    {
        set { PlayerPrefs.SetInt($"Map_{id}_Bought", value); }
        get { return PlayerPrefs.GetInt($"Map_{id}_Bought", 0); }
    }
}