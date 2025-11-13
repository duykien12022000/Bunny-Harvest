using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : Singleton<GameController>
{
    public Transform mapRoot;

    public static int CurretnRadish = 0;
    public int numberGenerate = 20;

    private int currentHeart;
    public AreaController areaCtrl;
    private List<Vegetable> vegetables = new List<Vegetable>();
    private List<Worm> worms = new List<Worm>();
    private InGameUI inGameUI;
    private PlayerController playerCtrl;
    public void Initialize()
    {
        playerCtrl = PlayerController.Instance;
        playerCtrl.Initialize();

        
        CurretnRadish = 0;
        inGameUI = UIManager.Instance.GetScreen<InGameUI>();

        currentHeart = DataManager.MaxtHeart;
        UpdateHealth(currentHeart);
    }
    public void SetUpMap()
    {
        areaCtrl.CreatRandomPositions(numberGenerate);
        var rp = areaCtrl.selected;
        for (int i = 0; i < rp.Count; i++)
        {
            SpawnRandom(rp[i]);
        }
    }
    public void UpdatePhysic()
    {
        playerCtrl.UpdatePhysic();
    }
    public void DespawnVegetable(Vegetable vegetableRemove)
    {
        areaCtrl.PushSe2Re(Vector3Int.CeilToInt(vegetableRemove.InitPos));
        FactoryObject.Despawn("Vegetable", vegetableRemove.transform);
        vegetables.Remove(vegetableRemove);
        GameManager.Instance.Delay(3, () =>
        {
            HandleRespawn(SpawnRandom);
        });
    }
    public void DespawnEnemy(Worm wormRemove)
    {
        areaCtrl.PushSe2Re(Vector3Int.CeilToInt(wormRemove.transform.position));
        FactoryObject.Despawn("Enemy", wormRemove.transform);
        worms.Remove(wormRemove);
        GameManager.Instance.Delay(3, () =>
        {
            HandleRespawn(SpawnRandom);
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
    private void SpawnRandom(Vector3Int position)
    {
        int random = Random.Range(0, 100);
        if( random <= 80)
        {
            var x = FactoryObject.Spawn<Vegetable>("Vegetable", "Radish");
            x.Initialize();
            x.transform.position = position;
            vegetables.Add(x);
        }
        else
        {
            var x = FactoryObject.Spawn<Worm>("Enemy", "Worm");
            x.Initialize();
            x.transform.position = position;
            worms.Add(x);
        }
    }
    public void UpdateScore(int score)
    {
        CurretnRadish += score;
        inGameUI.UpdateCurrentScore(CurretnRadish);
    }
    public void UpdateHealth(int amount)
    {
        currentHeart += amount;
        inGameUI.UpdateCurrentHealth(currentHeart);
    }
    public bool OnLose()
    {
        if(currentHeart < 1)
    {
            currentHeart = 0;
            GameManager.Instance.SwitchGameState(GameState.LOSE);
            return true;
        }
        return false;
    }
}
