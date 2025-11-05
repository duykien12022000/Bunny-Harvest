using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractArea : MonoBehaviour
{
    public List<Vegetable> vegetables = new List<Vegetable>();
    [SerializeField] LayerMask objInteractLayer;
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & objInteractLayer) != 0)
        {
            vegetables.Add(other.GetComponent<Vegetable>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        vegetables.Remove(other.GetComponent<Vegetable>());
    }
    public void RemoveObjInteract(Vegetable obj)
    {
        vegetables.Remove(obj);
    }
}
