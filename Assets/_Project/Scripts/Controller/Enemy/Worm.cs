using UnityEngine;

public class Worm : EnemyController
{
    public float rotationSpeed = 180;
    public Transform firePos;
    public ParticleSystem warningVFX;

    private Vector3 _direction;
    private bool isFiring;
    public override void Initialize()
    {
        base.Initialize();
        animatorHandle.OnEventAnimation += SendEvent;
        _direction = PlayerController.Instance.transform.position - transform.position;
        float angle = Mathf.Atan2(_direction.normalized.x, _direction.normalized.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void SendEvent(string eventName)
    {
        if (eventName == "Fire")
        {
            isFiring = true;
            var p = FactoryObject.Spawn<NormalProjectile>("Projectile", "NormalProjectile");
            p.transform.localPosition = firePos.position;
            p.Initialize(target);
        }
        if (eventName == "EndFire")
        {
            isFiring = false;
        }
        if (eventName == "WarningVFX")
        {
            warningVFX.Play();
        }
        if (eventName == "OnDead")
        {
            GameController.Instance.DespawnEnemy(this);
            Debug.Log("OH SHIT");
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (GameManager.currentState != GameState.PLAY)
        {
            animatorHandle.SetFloat("AttackAmount", 0);
            return;
        }
        if (isDetected)
        {
            if (!isFiring)
            {
                _direction = target.transform.position - transform.position;
                float angle = Mathf.Atan2(_direction.normalized.x, _direction.normalized.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
            animatorHandle.SetFloat("AttackAmount", 1);
        }
        else
        {
            animatorHandle.SetFloat("AttackAmount", 0);
        }
    }
}
