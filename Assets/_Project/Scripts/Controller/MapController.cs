using UnityEngine;

public class MapController : Singleton<MapController>
{
    [SerializeField] AreaController areaController;
    public AreaController AreaController => areaController;
    public Transform startPos;
    public void SetUpMap()
    {
        GameController.Instance.SetUpMap();
        PlayerController.Instance.transform.position = startPos.position;
    }
}
