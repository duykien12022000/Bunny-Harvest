using System;
using UnityEngine;

public class FootContact : MonoBehaviour
{
    private PlayerController playerCtrl;
    public void Initialize(PlayerController playerCtrl)
    {
        this.playerCtrl = playerCtrl;
    }
    public void OnTrigger()
    {
        playerCtrl.OnJumpingHigher();
    }
}
