using DG.Tweening;
using System;
using UnityEngine;
public class CameraController : Singleton<CameraController>
{
    public Camera mainCamera;
    public Vector3 offset;
    public Transform startPos;
    private Vector3 velocity;
    public void Back2StartPos()
    {
        MoveTo(startPos.position, 0.5f, 0, Ease.InOutSine);
        RotateTo(startPos.eulerAngles, 0.5f, 0, Ease.InOutSine);
    }
    public void FollowTo(Vector3 target)
    {
        Vector3 newPosition = new Vector3(target.x + offset.x, 10, target.z + offset.z);
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, 0.1f);
    }
    public void MoveTo(Vector3 newPos, float duration, float delay, Ease ease, Action onComplete = null)
    {
        mainCamera.transform.DOLocalMove(newPos, duration).SetDelay(delay).SetEase(ease).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
    public void RotateTo(Vector3 newRot, float duration, float delay, Ease ease, Action onComplete = null)
    {
        mainCamera.transform.DORotate(newRot, duration).SetDelay(delay).SetEase(ease).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
    private Tween KillCameraShake;
    public void ShakeCamera(float dur, float stre, int vib, int randness, bool smooth = true, Action onComplete = null)
    {
        if (KillCameraShake != null)
        {
            KillCameraShake.Kill();
            mainCamera.transform.localPosition = Vector3.forward * -10f;
        }
        KillCameraShake = mainCamera.DOShakePosition(dur, stre, vib, randness, smooth).OnComplete(() =>
        {
            mainCamera.transform.localPosition = Vector3.forward * -10f;
            onComplete?.Invoke();
        });
    }
    public static float GetEdgeCamera(CameraEdge cameraEdge)
    {
        float orthoHeight = Camera.main.orthographicSize;
        float orthoWidth = orthoHeight * Camera.main.aspect;
        float value = 0;
        switch (cameraEdge)
        {
            case CameraEdge.Left:
                value = Camera.main.transform.position.x - orthoWidth;
                break;
            case CameraEdge.Right:
                value = Camera.main.transform.position.x + orthoWidth;
                break;
            case CameraEdge.Top:
                value = 0;
                break;
            case CameraEdge.Bottom:
                value = 1;
                break;
        }
        return value;
    }
    //private Transform target;
    //private Vector3 velocity;
    //public void FollowTo(Transform target, float duration, Action onComplete)
    //{
    //    this.target = target;
    //    //if(Vector2.Distance(mainCamera.transform.position, endValue) <= 0)
    //    SD_GameManager.Instance.Delay(duration, () =>
    //    {
    //        this.target = null;
    //        onComplete?.Invoke();
    //    });
    //}
    //private void Update()
    //{
    //    if (target != null)
    //    {
    //        Vector3 newPos = new Vector3(0, target.position.y, -10f);
    //        mainCamera.transform.position = newPos;//Vector3.SmoothDamp(mainCamera.transform.position, newPos, ref velocity, 0.5f);
    //    }
    //}

}
public enum CameraEdge
{
    Left,
    Right,
    Top,
    Bottom
}

