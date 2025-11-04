using System;
using UnityEngine;

public abstract class ObjectInteract : MonoBehaviour
{

    [SerializeField] protected float radius = 1f;
    [SerializeField] protected GameObject model;
    public float Radius => radius;
    protected State currentState;
    protected PlayerController player;
    protected bool playerOnArea;
    protected bool isClaimed;

    private void Start()
    {
        Initialize();
    }
    public virtual void Initialize()
    {
        currentState = State.SMALL;
        player = PlayerController.Instance;
        playerOnArea = false;
        isClaimed = false;
    }
    private void Update()
    {
        UpdateLogic();
    }
    protected virtual void UpdateLogic()
    {
        var p1 = new Vector3(transform.position.x, 0, transform.position.z);
        var p2 = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        if (Vector3.Distance(p1, p2) < radius)
        {
            playerOnArea = true;
        }
        else
        {
            playerOnArea = false;
        }
    }
    protected void SwitchSate(State newState)
    {
        currentState = newState;
    }
    public abstract void OnClaiming();
    public abstract void CancelClaim();
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
public enum State
{
    SMALL, MEDIUM, FULLY
}
