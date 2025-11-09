using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterAddHealth : Booster
{
    public override void ApplyEffect()
    {


    }
    public void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            if (other.CompareTag("Player"))
            {
                GameController.Instance.UpdateHealth(1);
                SetUpDisplay(true);
                GameManager.Instance.Delay(0.5f, () =>
                {
                    FactoryObject.Despawn("Booster", this.transform);
                });
            }
        }
    }
}
