using System;
using UnityEngine;

public abstract class ObjectInteract : MonoBehaviour
{

    [SerializeField] protected float radius = 1f;
    [SerializeField] protected GameObject model;
    public float Radius => radius;
    protected State currentState;
    public State State => currentState;
    protected PlayerController player;
    protected bool isClaimed;

    public virtual void Initialize()
    {
        currentState = State.SMALL;
        player = PlayerController.Instance;
        //playerOnArea = false;
        isClaimed = false;
    }
    private void Update()
    {
        UpdateLogic();
    }
    protected virtual void UpdateLogic()
    {
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
