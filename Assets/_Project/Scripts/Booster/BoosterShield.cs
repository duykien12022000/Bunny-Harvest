using UnityEngine;

public class BoosterShiedl : Booster
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
                playerController.ApplyEffect(type);
                var b = ingameUI.GetBooster(type);
                b.SetActive(true);
                b.OnTime(time, () => { playerController.ResetBooster(type); });
                SetUpDisplay(true);
                GameManager.Instance.Delay(0.5f, () =>
                {
                    FactoryObject.Despawn("Booster", this.transform);
                });
            }
        }
    }
}
