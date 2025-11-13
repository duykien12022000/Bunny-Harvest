using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] protected AnimatorHandle animatorHandle;
    [SerializeField] protected StatePatrol statePatrol;
    [SerializeField] protected float radiusDetected = 3;
    [SerializeField] protected Collider bodyCollider;
    [SerializeField] protected ParticleSystem deadVFX;

    [Range(0, 180)]
    [SerializeField] protected float viewAngle = 90f;


    private bool isFirstSpawn;

    public AudioClip[] deadSFX;
    

    protected PlayerController target;
    protected Collider m_collider;
    protected bool isDead;
    public bool isDetected { protected set; get; }
    public virtual void Initialize()
    {
        if (!isFirstSpawn)
        {
            isFirstSpawn = true;
            var cacheView = viewAngle;
            viewAngle = 0;
            StartCoroutine(Wait());
            IEnumerator Wait()
            {
                yield return new WaitForSeconds(2f);
                viewAngle = cacheView;
            }
        }
        target = PlayerController.Instance;
        isDead = false;
        m_collider = GetComponent<Collider>();
        m_collider.enabled = true;
        bodyCollider.enabled = true;
    }
    private void Update()
    {
        if (isDead) return;
        UpdateLogic();
    }
    public virtual void UpdateLogic()
    {
        isDetected = PlayerDetected();
    }
    private bool PlayerDetected()
    {
        if (target == null) return false;
        var p1 = transform.position;
        var p2 = target.transform.position;
        Vector3 dirToPlayer = (p2 - p1).normalized;
        float distance = Vector3.Distance(p1, p2);

        // 1️⃣ Kiểm tra trong bán kính
        if (distance > radiusDetected) return false;

        // 2️⃣ Kiểm tra góc nhìn
        float dot = Vector3.Dot(transform.forward, dirToPlayer);
        float cosHalfAngle = Mathf.Cos(viewAngle * 0.5f * Mathf.Deg2Rad);
        if (dot < cosHalfAngle) return false; // Player nằm ngoài góc nhìn
        return true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radiusDetected); // vòng tròn bán kính detect

        // Vẽ 2 đường biên góc nhìn
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * radiusDetected);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * radiusDetected);

        // Nếu đang thấy player, vẽ line đỏ
        if (isDetected && target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }
    protected void OnTakeDamage()
    {
        isDead = true;
        m_collider.enabled = false;
        bodyCollider.enabled = false;
        animatorHandle.PlayAnimation("Dead", 0.1f, 0);
        AudioManager.Instance.PlayOneShot(deadSFX[Random.Range(0, deadSFX.Length)], 1);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerFoot"))
        {
            var fc = other.GetComponent<FootContact>();
            fc.OnTrigger();
            OnTakeDamage();
            deadVFX.Play();
        }
        else if (other.CompareTag("Shield"))
        {
            OnTakeDamage();
        }
    }
}
public enum StatePatrol
{
    CAN_MOVE,
    IDLE,
}
