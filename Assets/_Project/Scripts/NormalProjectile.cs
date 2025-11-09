using UnityEngine;

public class NormalProjectile : Projectile
{
    public bool isLookAtTarget = false;
    public bool isMovingDirection = false;
    [Tooltip("Use only for moving direction")]
    private Vector3 _direction;
    public override void Initialize(PlayerController target)
    {
        this.target = target;
        Active();
        if (GetComponent<Collider>() != null)
        {
            GetComponent<Collider>().enabled = true;
        }

        lifeTimer = lifeTime;
        if (!isMovingDirection) return;
        if (target == null) return;
        AudioManager.Instance.PlayOneShot(activeSFX, 1);
        _direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(_direction.normalized.x, _direction.normalized.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }
    public override void UpdateLogic()
    {
        if (!isActive) return;
        if (target == null)
        {
            Deactive();
            return;
        }

        if (isMovingDirection)
        {
            lifeTimer -= Time.deltaTime;
            Vector3 pos = transform.position;
            pos += speed * Time.deltaTime * _direction.normalized;
            transform.position = new Vector3(pos.x, transform.position.y, pos.z);

            if (lifeTimer <= 0)
            {
                Deactive();
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            if ((Vector2)transform.position == (Vector2)target.transform.position)
            {
                //target.core.combat.TakeDamage(damageInfo);

                Deactive();
            }

            if (isLookAtTarget)
            {
                LootAtTarget(target.transform.position);
            }
        }
    }
    protected virtual void LootAtTarget(Vector3 target)
    {
        Vector3 diff = target - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (transform.localScale.x < 0)
        {
            transform.rotation = Quaternion.Euler(-180f, -180f, rot_z);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        }
    }
    protected void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        if (other.CompareTag("Player"))
        {
            target.OnTakeDamage();

            AudioManager.Instance.PlayOneShot(impactSFX, 1);

            Deactive();
        }
        else if (other.CompareTag("Environment") || other.CompareTag("Shield"))
        {
            AudioManager.Instance.PlayOneShot(impactSFX, 1);

            Deactive();
        }
        //IDamage id = other.GetComponent<IDamage>();
        //if (id != null && id.GetCharacterType != damageInfo.characterType)
        //{
        //    if (isSingleTarget)
        //    {
        //        GetComponent<Collider2D>().enabled = false;
        //        Deactive();
        //    }
        //    id.TakeDamage(damageInfo);
        //}
    }
    private void Active()
    {
        isActive = true;
        if (display)
            display.SetActive(true);
        if (impact)
            impact.SetActive(false);

    }
    private void Deactive()
    {
        isActive = false;
        if (display)
            display.SetActive(false);
        if (impact)
            impact.SetActive(true);
        FactoryObject.Despawn("Projectile", transform, 0.1f);
    }
}
