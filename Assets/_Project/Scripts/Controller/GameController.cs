using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public static int CurrentScore = 0;
    public int numberGenerate = 20;
    private int currentHeart;
    private AreaController areaCtrl;
    private List<Vegetable> vegetables = new List<Vegetable>();
    private List<Worm> worms = new List<Worm>();
    private InGameUI inGameUI;
    public void Initialize()
    {
        areaCtrl = AreaController.Instance;
        areaCtrl.CreatRandomPositions(numberGenerate);
        var rp = areaCtrl.selected;
        for (int i = 0; i < rp.Count; i++)
        {
            SpawnVegetable(rp[i]);
        }
        HandleRespawn(SpawnEnemy);
        CurrentScore = 0;

        inGameUI = UIManager.Instance.GetScreen<InGameUI>();

        currentHeart = DataManager.MaxtHeart;
        UpdateHealth(currentHeart);
    }
    public void DespawnVegetable(Vegetable vegetableRemove)
    {
        areaCtrl.PushSe2Re(Vector3Int.CeilToInt(vegetableRemove.transform.position));
        FactoryObject.Despawn("Vegetable", vegetableRemove.transform);
        vegetables.Remove(vegetableRemove);
        GameManager.Instance.Delay(3, () =>
        {
            HandleRespawn(SpawnVegetable);
        });
    }
    public void HandleRespawn(Action<Vector3Int> OnSpawn)
    {
        if (areaCtrl.selected.Count < numberGenerate)
        {
            int missing = numberGenerate - areaCtrl.selected.Count;
            var rp = areaCtrl.GetRandomPositionFrRemaining(missing);
            for (int i = 0; i < rp.Count; i++)
            {
                OnSpawn?.Invoke(rp[i]);
                areaCtrl.PushRe2Se(rp[i]);
            }
        }
    }
    private void SpawnVegetable(Vector3Int position)
    {
        var x = FactoryObject.Spawn<Vegetable>("Vegetable", "Radish");
        x.Initialize();
        x.transform.position = position;
        vegetables.Add(x);
    }
    private void SpawnEnemy(Vector3Int position)
    {
        var x = FactoryObject.Spawn<Worm>("Enemy", "Worm");
        x.Initialize();
        x.transform.position = position;
        worms.Add(x);
    }
    public void UpdateScore(int score)
    {
        CurrentScore += score;
        inGameUI.UpdateCurrentScore(CurrentScore);
    }
    public void UpdateHealth(int amount)
    {
        currentHeart += amount;
        inGameUI.UpdateCurrentHealth(amount);
    }
}
