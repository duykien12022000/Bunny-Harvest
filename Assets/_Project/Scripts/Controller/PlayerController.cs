using System.Collections;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public Transform root;

    [SerializeField] float moveSpeed, jumpForce;
    [SerializeField] AnimatorHandle animatorHandle;
    [SerializeField] InteractArea interactArea;
    [SerializeField] FootContact footContact;
    [SerializeField] ParticleSystem dustVFX;
    private Joystick joystick;
    private Rigidbody rb;

    [Header("Raycast Checking")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float footOffset, groundRayDistance;

    [Header("Booster")]
    [SerializeField] GameObject shieldObject;
    private float cacheSpeed;

    bool isJumpinng;
    public void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        interactArea.Initialize();
        footContact.Initialize(this);
        joystick = UIManager.Instance.GetScreen<InGameUI>().Joystick;
        animatorHandle.OnEventAnimation += SendEvent;
        shieldObject.SetActive(false);
        cacheSpeed = moveSpeed;
    }

    private void SendEvent(string eventName)
    {
        if (eventName == "Collected")
        {
            animatorHandle.SetBool("IsInteracting", false);
            OnCollectVegetable();
        }
    }
    public void UpdatePhysic()
    {
        if (animatorHandle.GetBool("IsInteracting")) return;
        animatorHandle.SetBool("IsOnGround", IsOnGround() && rb.velocity.y <= 0.001f);
        if (GameManager.currentState != GameState.PLAY) return;
        HandleMoving();
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleJumping();
        }
#endif
        CameraController.Instance.FollowTo(transform.position);
    }
    private void HandleMoving()
    {

#if UNITY_EDITOR
        //Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
//#else

#endif
        Vector3 moveDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 targetPosition = rb.position + moveDirection.normalized * cacheSpeed * Time.fixedDeltaTime;
            rb.MovePosition(targetPosition);
            Quaternion targetRotation = Quaternion.LookRotation(-moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 0.2f));
        }
        animatorHandle.SetFloat("MoveAmount", moveDirection.magnitude, 1);
        animatorHandle.SetFloat("VelocityY", rb.velocity.y);
    }
    public void HandleJumping()
    {
        if (GameManager.currentState != GameState.PLAY) return;
        if (IsOnGround())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
    public void OnJumpingHigher()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animatorHandle.PlayAnimation("JumpHigher", 0.1f, 0);
    }
    public void PickUp()
    {
        if (GameManager.currentState != GameState.PLAY) return;
        if (interactArea.vegetables.Count == 0) return;
        animatorHandle.PlayAnimation("PickUp", 0.1f, 0, true, 2);
    }
    public void OnCollectVegetable()
    {
        for (int i = 0; i < interactArea.vegetables.Count; i++)
        {
            var v = interactArea.vegetables[i];
            v.OnClaiming();
            interactArea.RemoveObjInteract(v);
            int score = 0;
            if (v.State == State.MEDIUM) score = 1;
            else if (v.State == State.FULLY) score = 2;
            GameController.Instance.UpdateScore(score);
        }
    }
    private void CancelPickUp()
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
        if (GameController.Instance.OnLose())
        {
            animatorHandle.PlayAnimation("Dead", 0.1f, 0);
            GameManager.Instance.Delay(3.6f, () =>
            {
                UIManager.Instance.ShowPopup<PopupLose>(null);
            });
            AudioManager.Instance.PlayOneShot(SFXStr.LOSE_VOICE, 1);
            return;
        }
        animatorHandle.PlayAnimation("Hit", 0.1f, 0);
        CancelPickUp();
    }
    public void ApplyEffect(BoosterType type)
    {
        switch (type)
        {
            case BoosterType.MAGNET:
                interactArea.OnApplyBoosterMagnet();
                break;
            case BoosterType.SHIELD:
                shieldObject.SetActive(true);
                break;
            case BoosterType.SPEED_UP:
                cacheSpeed = moveSpeed;
                cacheSpeed *= 2f;
                break;

        }
    }
    public void ResetBooster(BoosterType type)
    {
        switch (type)
        {
            case BoosterType.MAGNET:
                interactArea.OnCancelBoosterMagnet();
                break;
            case BoosterType.SHIELD:
                shieldObject.SetActive(false);
                break;
            case BoosterType.SPEED_UP:
                cacheSpeed = moveSpeed;
                break;
        }
    }
    public void RebindAnimator()
    {
        StartCoroutine(WaitAFrame());
        IEnumerator WaitAFrame()
        {
            yield return null;
            animatorHandle.Rebind();
        }
    }
}
