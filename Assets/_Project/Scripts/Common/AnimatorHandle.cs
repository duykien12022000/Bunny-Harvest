using System.Collections.Generic;
using UnityEngine;
public class AnimatorHandle : MonoBehaviour
{
    public Animator animator
    {
        get
        {
            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }
    private Animator _animator;

    public event System.Action<string> OnEventAnimation;
    private Dictionary<string, int> layers = new Dictionary<string, int>();
    public event System.Action<Vector3> OnAnimatorUpdate;
    public void Rebind()
    {
        animator.Rebind();
    }
    public virtual void ResetAnimator()
    {
        ResumeAnimator();
    }
    public virtual void PauseAnimator()
    {

        animator.speed = 0;
    }
    public virtual void ResumeAnimator()
    {
        animator.speed = 1;
    }
    public void SetFloat(string parameter, float value)
    {
        animator.SetFloat(parameter, value);
    }
    public void SetFloat(string parameter, float value, float speedAnimation)
    {
        animator.SetFloat(parameter, value);
        animator.speed = speedAnimation;
    }
    public void SetBool(string parameter, bool status)
    {
        animator.SetBool(parameter, status);
    }
    public bool GetBool(string param)
    {
        return animator.GetBool(param);
    }
    public void PlayAnimation(string stateName, float normalizedTransitionDuration, int layer)
    {
        animator.CrossFade(stateName, normalizedTransitionDuration, layer);
    }
    public void PlayAnimation(string stateName, float normalizedTransitionDuration, int layer, bool isInteracting)
    {
        animator.CrossFade(stateName, normalizedTransitionDuration, layer);
        animator.SetBool("IsInteracting", isInteracting);
    }

    public void PlayAnimation(string stateName, float normalizedTransitionDuration, int layer, bool isInteracting, float speedAnimation)
    {
        animator.CrossFade(stateName, normalizedTransitionDuration, layer);
        animator.SetBool("IsInteracting", isInteracting);
        animator.speed = speedAnimation;
    }
    public void PlayAnimation(string stateName, float normalizedTransitionDuration, string layerName, bool isInteracting, bool isApplyRootMotion)
    {
        animator.CrossFade(stateName, normalizedTransitionDuration, GetLayer(layerName));
        animator.SetBool("IsInteracting", isInteracting);
        animator.SetBool("IsApplyRootMotion", isInteracting);
    }
    public void PlayAnimation(string stateName, float normalizedTransitionDuration, int layer, bool isInteracting, bool isApplyRootMotion)
    {
        animator.CrossFade(stateName, normalizedTransitionDuration, layer);
        animator.SetBool("IsInteracting", isInteracting);
        animator.SetBool("IsApplyRootMotion", isApplyRootMotion);
    }

    //public void PlayStunAnimation(float normalizedTransitionDuration, int layer, bool isInteracting, float stunTime)
    //{
    //    animator.SetFloat("StunTime", stunTime);
    //    animator.CrossFade("Hit", normalizedTransitionDuration, layer);
    //    animator.SetBool("IsInteracting", isInteracting);
    //    if (animator.GetCurrentAnimatorStateInfo(layer).normalizedTime >= 1)
    //    {
    //        animator.SetFloat("StunTime", 1);
    //    }
    //}

    public int GetLayer(string layerName)
    {
        int layer = 0;
        if (layers.ContainsKey(layerName))
        {
            layer = layers[layerName];
        }
        else
        {
            layer = animator.GetLayerIndex(layerName);
            layers.Add(layerName, layer);
        }
        return layer;
    }
    public void SendEvent(string eventName)
    {
        OnEventAnimation?.Invoke(eventName);
    }
    public virtual void DeactiveCharacter()
    {
        gameObject.SetActive(false);
    }
    public Transform GetBone(HumanBodyBones bone)
    {
        return animator.GetBoneTransform(bone);
    }
    private void OnAnimatorMove()
    {
        OnAnimatorUpdate?.Invoke(animator.deltaPosition);
    }

}

