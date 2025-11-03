using FastFood;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] AnimatorHandle animatorHandle;
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
        Debug.Log(moveDirection.magnitude);
        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 targetPosition = rb.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);

            // Xoay theo hướng di chuyển (tùy chọn)
            Quaternion targetRotation = Quaternion.LookRotation(-moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 0.2f));
        }
        animatorHandle.SetFloat("MoveAmount", moveDirection.magnitude, 1);
        CameraController.Instance.FollowTo(transform.position);
    }
}
