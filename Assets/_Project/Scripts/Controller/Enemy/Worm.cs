using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : EnemyController
{
    private Vector3 _direction;
    public float rotationSpeed = 180;
    public Transform firePos;
    private bool isFiring;
    public override void Initialize()
    {
        base.Initialize();
        animatorHandle.OnEventAnimation += OnFire;
    }

    private void OnFire(string obj)
    {
        if (obj == "Fire")
        {
            isFiring = true;
            var p = FactoryObject.Spawn<NormalProjectile>("Projectile", "NormalProjectile");
            p.transform.parent = firePos;
            p.transform.localPosition = Vector3.zero;
            p.Initialize(target);
        }
        if(obj == "EndFire")
        {
            isFiring = false;
        }
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
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
