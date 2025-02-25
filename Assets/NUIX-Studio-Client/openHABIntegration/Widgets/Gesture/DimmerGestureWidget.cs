﻿
using System;
using Tsinghua.HCI.IoThingsLab;
using UnityEngine;

public class DimmerGestureWidget : ItemWidget
{
    public GestureThumbsUpRotated _Gesture;

    int counter = 0;
    /// <summary>
    /// Initialize ItemController
    /// </summary>
    public override void Start()
    {
        // OnUpdate is not needed for Gestures only OnSetItem
        base.Start();
        InitWidget();
    }

    /// <summary>
    /// For public field initialization etc. This is to be able to use
    /// a generic start function for all widgets. This function is called for
    /// at end of Start()
    /// </summary>
    private void InitWidget()
    {
        if (_Gesture == null) _Gesture = GetComponent<GestureThumbsUpRotated>();
    }

    void Update()
    {
        counter = (counter + 1) % 5;
        if (counter == 1) OnSetItem();
    }


    /// <summary>
    /// When an item updates from server. This function is
    /// called from ItemController when Item is Updated on server.
    /// Begin with a check if Item and UI state is equal. Otherwise we
    /// might get flickering as the state event is sent after update from
    /// UI. This will Sync as long as Event Stream is online.
    /// </summary>
    public override void OnUpdate()
    {
    }

    /// <summary>
    /// Update item from UI. Call itemcontroller and update Item on server.
    /// If update is true, an event will be recieved. If state is equal no
    /// new UI update is necesarry. If not equal the PUT has failed and we need
    /// to revert UI state to server state.
    /// </summary>
    public void OnSetItem()
    {
        if (_Gesture.TryGetNormalizedValue(out uint value))
        {
            ConnectedItemController.SetItemStateAsDimmer((int)value);
        }
    }

    /// <summary>
    /// Stop event listening from controller
    /// </summary>
    void OnDisable()
    {
        ConnectedItemController.updateItem -= OnUpdate;
    }
}