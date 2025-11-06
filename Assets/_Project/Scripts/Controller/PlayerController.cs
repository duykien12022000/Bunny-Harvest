using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] float moveSpeed, jumpForce;
    [SerializeField] AnimatorHandle animatorHandle;
    [SerializeField] InteractArea interactArea;
    private Joystick joystick;
    private Rigidbody rb;

    [Header("Raycast Checking")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float footOffset, groundRayDistance;

    bool isJumpinng;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        joystick = UIManager.Instance.GetScreen<InGameUI>().Joystick;
    }
    private void FixedUpdate()
    {
        if (animatorHandle.GetBool("IsInteracting")) return;
        animatorHandle.SetBool("IsOnGround", IsOnGround() && rb.velocity.y <= 0.001f);
        HandleMoving();
        HandleJumping();
        CameraController.Instance.FollowTo(transform.position);
    }
    private void HandleMoving()
    {
        Vector3 moveDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 targetPosition = rb.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
            Quaternion targetRotation = Quaternion.LookRotation(-moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 0.2f));
        }
        animatorHandle.SetFloat("MoveAmount", moveDirection.magnitude, 1);
        animatorHandle.SetFloat("VelocityY", rb.velocity.y);
    }
    private void HandleJumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsOnGround())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                //animatorHandle.PlayAnimation("Jump", 0.1f, 0);
            }
        }
    }



    public void PickUp()
    {
        if (interactArea.vegetables.Count == 0) return;
        animatorHandle.PlayAnimation("PickUp", 0.1f, 0, true, 2);
        GameManager.Instance.Delay(0.75f, () => { animatorHandle.SetBool("IsInteracting", false); });
        for (int i = 0; i < interactArea.vegetables.Count; i++)
        {
            var v = interactArea.vegetables[i];
            v.OnClaiming();
            interactArea.RemoveObjInteract(v);
            GameController.Instance.UpdateScore(1);

        }
    }
    public void CancelPickUp()
    {
        animatorHandle.SetBool("IsInteracting", false);
        foreach (var obj in interactArea.vegetables)
        {
            obj.CancelClaim();
        }
    }
    public bool IsOnGround()
    {
        return RayCast(transform.position + new Vector3(0, 0, footOffset), Vector2.down, groundRayDistance, groundLayer);
    }

    private bool RayCast(Vector3 position, Vector3 direction, float distance, LayerMask layerMask)
    {
        Ray ray = new Ray(position, direction);
        var raycast = Physics.Raycast(ray, out RaycastHit hit, distance, layerMask);
        Color color = raycast ? Color.red : Color.green;
        Debug.DrawRay(position, Vector3.down * distance, color);
        return raycast;
    }
    public void OnTakeDamage()
    {
        GameController.Instance.UpdateHealth(-1);
    }
}
