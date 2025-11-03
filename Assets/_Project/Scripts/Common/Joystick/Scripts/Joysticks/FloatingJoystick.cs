using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    private Vector2 startPos;
    //private Animator animator;
    private float timer;
    private int counter;
    protected override void Start()
    {
        base.Start();
        startPos = Vector2.zero;
        //RPGFantasy.RequireDisplay.OnInteract += Cancel;
        //background.anchoredPosition = ScreenPointToAnchoredPosition(startPos);
        //animator = GetComponent<Animator>();
        counter = 0;
        timer = 5f;
    }
    private void Update()
    {
        //timer -= Time.deltaTime;
        //animator.enabled = timer <= 0;
    }
    private void Cancel()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        OnPointerUp(eventData);
    }
    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        base.HandleInput(magnitude, normalised, radius, cam);
        timer = 10f;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.anchoredPosition = Vector2.zero;
        base.OnPointerUp(eventData);
    }
}