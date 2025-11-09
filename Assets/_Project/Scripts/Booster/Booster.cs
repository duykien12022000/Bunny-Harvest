using UnityEngine;

public abstract class Booster : MonoBehaviour
{
    [SerializeField] protected BoosterType type;
    [SerializeField] protected GameObject display, impact;
    [SerializeField] protected int time;
    protected Collider m_collider;
    protected InGameUI ingameUI
    {
        get { return UIManager.Instance.GetScreen<InGameUI>(); }
    }
    protected PlayerController playerController
    {
        get { return PlayerController.Instance; }
    }
    public BoosterType Type => type;
    public virtual void Initialize()
    {
        m_collider = GetComponent<Collider>();
        SetUpDisplay();
    }
    public abstract void ApplyEffect();
    public void SetUpDisplay(bool isDespawn = false)
    {
        if (!isDespawn)
        {
            display.SetActive(true);
            impact.SetActive(false);
            m_collider.enabled = true;
        }
        else
        {
            display.SetActive(false);
            impact.SetActive(true);
            m_collider.enabled = false;
        }
    }

}
public enum BoosterType
{
    ADD_HEART,
    SPEED_UP,
    MAGNET,
    SHIELD
}

