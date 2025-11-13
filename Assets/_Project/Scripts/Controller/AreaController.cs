using System.Collections.Generic;
using UnityEngine;

public class AreaController : MonoBehaviour
{
    [SerializeField] Area[] areas;
    [SerializeField] List<Vector3Int> allPosSpawn = new List<Vector3Int>();
    [ContextMenu("GetArea")]
    void GetArea()
    {
        areas = GetComponentsInChildren<Area>();
        allPosSpawn.Clear();
        foreach (Area area in areas)
        {
            area.CreatAllPositions();
            allPosSpawn.AddRange(area.AllPositions);
        }
        for (int i = allPosSpawn.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (allPosSpawn[i], allPosSpawn[j]) = (allPosSpawn[j], allPosSpawn[i]);
        }
    }
    public List<Vector3Int> selected = new List<Vector3Int>();
    public List<Vector3Int> remaining = new List<Vector3Int>();

    public void PushSe2Re(Vector3Int element)
    {
        selected.Remove(element);
        remaining.Add(element);
    }
    public void PushRe2Se(Vector3Int element)
    {
        selected.Add(element);
        remaining.Remove(element);
    }
    public void CreatRandomPositions(int amount)
    {
        List<Vector3Int> copy = new List<Vector3Int>(allPosSpawn);
        for (int i = copy.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (copy[i], copy[j]) = (copy[j], copy[i]);
        }
        int count = Mathf.Min(amount, copy.Count);
        selected = copy.GetRange(0, count);
        remaining = copy.GetRange(count, copy.Count - count);
    }
    public List<Vector3Int> GetRandomPositionFrRemaining(int amount)
    {
        return remaining.GetRange(0, amount);
    }
}
