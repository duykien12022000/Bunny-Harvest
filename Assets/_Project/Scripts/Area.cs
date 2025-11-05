using UnityEngine;
using System;
using System.Drawing;
using Color = UnityEngine.Color;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Area : MonoBehaviour
{
    [SerializeField] MinMaxArea posArray;
    [SerializeField] Color gizmoColor = Color.green;
    [SerializeField] List<Vector3Int> allPositions = new List<Vector3Int>();
    public List<Vector3Int> AllPositions => allPositions;
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        // Tính toán tâm và kích thước của box
        Vector3 center = ((Vector3)posArray.min + (Vector3)posArray.max) / 2f + transform.position;
        Vector3 size = (Vector3)(posArray.max - posArray.min);

        // Nếu object có transform, cộng thêm vị trí của nó
        center += transform.position;

        Gizmos.DrawWireCube(center, size);
    }
    public void CreatAllPositions()
    {
        allPositions.Clear();
        for (int x = posArray.min.x; x <= posArray.max.x; x++)
        {
            for (int z = posArray.min.z; z <= posArray.max.z; z++)
            {
                allPositions.Add(new Vector3Int(x, 0, z));
            }
        }
    }
}

[Serializable]
public struct MinMaxArea
{
    public Vector3Int min;
    public Vector3Int max;
}
