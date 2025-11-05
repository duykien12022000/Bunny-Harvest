using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public static int CurrentScore = 0;
    private AreaController areaCtrl;
    private List<Vegetable> vegetables = new List<Vegetable>();
    public int numberGenerate = 20;
    public void Initialize()
    {
        areaCtrl = AreaController.Instance;
        areaCtrl.CreatRandomPositions(numberGenerate);
        var rp = areaCtrl.selected;
        for (int i = 0; i < rp.Count; i++)
        {
            var x = FactoryObject.Spawn<Vegetable>("Vegetable", "Radish");
            x.Initialize();
            x.transform.position = rp[i];
            vegetables.Add(x);
        }
        CurrentScore = 0;
    }
    public void DespawnVegetable(Vegetable vegetableRemove)
    {
        areaCtrl.PushSe2Re(Vector3Int.CeilToInt(vegetableRemove.transform.position));
        FactoryObject.Despawn("Vegetable", vegetableRemove.transform);
        vegetables.Remove(vegetableRemove);
        Invoke("HandleRespawn", 3);
    }
    public void HandleRespawn()
    {
        if (areaCtrl.selected.Count < numberGenerate)
        {
            int missing = numberGenerate - areaCtrl.selected.Count;
            var rp = areaCtrl.GetRandomPositionFrRemaining(missing);
            for (int i = 0; i < rp.Count; i++)
            {
                var x = FactoryObject.Spawn<Vegetable>("Vegetable", "Radish");
                x.Initialize();
                x.transform.position = rp[i];
                vegetables.Add(x);
                areaCtrl.PushRe2Se(rp[i]);
            }
        }
    }
    public void UpdateScore(int score)
    {
        CurrentScore += score;
        UIManager.Instance.GetScreen<InGameUI>().UpdateCurrentScore(CurrentScore);  
    }
}
