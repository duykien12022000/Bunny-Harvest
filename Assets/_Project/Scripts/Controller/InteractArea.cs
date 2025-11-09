using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractArea : MonoBehaviour
{
    public List<Vegetable> vegetables = new List<Vegetable>();
    [SerializeField] LayerMask objInteractLayer;
    private SphereCollider sphereCollider;
    private float baseRadius;
    private bool usingMagnet = false;
    public void Initialize()
    {
        sphereCollider = GetComponent<SphereCollider>();
        baseRadius = sphereCollider.radius;
    }
    public void OnApplyBoosterMagnet()
    {
        sphereCollider.radius = baseRadius * 2;
        usingMagnet = true;
    }
    public void OnCancelBoosterMagnet()
    {
        sphereCollider.radius = baseRadius;
        usingMagnet = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & objInteractLayer) != 0)
        {
            var v = other.GetComponent<Vegetable>();
            v.SetStatus(true);
            if (!v.IsAvailable2Claim) return;
            vegetables.Add(other.GetComponent<Vegetable>());
            if (usingMagnet)
            {
                PlayerController.Instance.OnCollectVegetable();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        var v = other.GetComponent<Vegetable>();
        if (v!=null)
        {
            v.SetStatus(false);
            if (vegetables.Contains(v))
            {
                vegetables.Remove(v);
            }
        }
    }
    public void RemoveObjInteract(Vegetable obj)
    {
        vegetables.Remove(obj);
    }
}
