using UnityEngine;
public abstract class Projectile : MonoBehaviour
{
    public GameObject display;
    public GameObject impact;
    protected PlayerController target;
    public LayerMask layerContact;
    public float speed = 20f;
    public bool isActive;
    public float lifeTime = 3f;
    public AudioClip activeSFX, impactSFX;
    protected float lifeTimer;
    public virtual void Initialize(PlayerController target)
    {
        this.target = target;

        if (GetComponent<Collider2D>() != null)
        {
            GetComponent<Collider2D>().enabled = true;
        }
        if (display != null)
            display.SetActive(true);
        if (impact != null)
            impact.SetActive(false);
        isActive = true;
        lifeTimer = lifeTime;
    }
    public virtual void UpdateLogic() { }
    private void Update()
    {
        //if (GameManager.currentState != GameState.PLAY) return;
        UpdateLogic();
    }
}

