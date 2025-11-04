using DG.Tweening;
using FastFood;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] float moveSpeed;
    [SerializeField] AnimatorHandle animatorHandle;
    [SerializeField] InteractArea interactArea;
    private Joystick joystick;
    private Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        joystick = UIManager.Instance.GetScreen<InGameUI>().Joystick;
    }
    private void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        if (animatorHandle.GetBool("IsInteracting")) return;
        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 targetPosition = rb.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
            Quaternion targetRotation = Quaternion.LookRotation(-moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 0.2f));
        }
        animatorHandle.SetFloat("MoveAmount", moveDirection.magnitude, 1);
        CameraController.Instance.FollowTo(transform.position);
    }
    public void PickUp()
    {
        if (interactArea.objectInteracts.Count == 0) return;
        animatorHandle.PlayAnimation("PickUp", 0.1f, 0, true, 2);
        GameManager.Instance.Delay(0.75f, () => { animatorHandle.SetBool("IsInteracting", false); });
        foreach (var obj in interactArea.objectInteracts)
        {
            obj.OnClaiming();
        }
    }
    public void CancelPickUp()
    {
        animatorHandle.SetBool("IsInteracting", false);
        foreach (var obj in interactArea.objectInteracts)
        {
            obj.CancelClaim();
        }
    }
}
