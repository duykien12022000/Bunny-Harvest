using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractArea : MonoBehaviour
{
    public HashSet<ObjectInteract> objectInteracts = new HashSet<ObjectInteract>();
    [SerializeField] LayerMask objInteractLayer;
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & objInteractLayer) != 0)
        {
            objectInteracts.Add(other.GetComponent<ObjectInteract>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        objectInteracts.Remove(other.GetComponent<ObjectInteract>());
    }
    public void RemoveObjInteract(ObjectInteract obj)
    {
        objectInteracts.Remove(obj);
    }
}
