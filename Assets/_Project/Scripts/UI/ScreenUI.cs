using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ScreenUI : MonoBehaviour
{
    protected UIManager uiManager;
    public abstract void Initialize(UIManager uiManager);
    public virtual void Active()
    {
        gameObject.SetActive(true);
    }
    public virtual void Deactive()
    {
        gameObject.SetActive(false);
    }
}