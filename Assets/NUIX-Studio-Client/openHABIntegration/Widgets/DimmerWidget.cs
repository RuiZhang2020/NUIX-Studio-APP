﻿using Microsoft.MixedReality.Toolkit.UI;
using UnityEngine;
using UnityEngine.UI;

public class DimmerWidget : ItemWidget
{
    
    //[Header("Widget Setup")]
    //public Slider _Slider;
    public PinchSlider _PinchSlider;
    //public SliderEventData _PinchSliderData;

    private int sentValue = -10;

    void Start()
    {
        if (connectedToServer) ConnectedItemController.updateItem += OnUpdate;
        InitWidget();
    }


    /// <summary>
    /// For public field initialization etc. This is to be able to use
    /// a generic start function for all widgets. This function is called for
    /// at end of Start()
    /// </summary>
    private void InitWidget()
    {
        //if (_Slider == null) _Slider = GetComponent<Slider>();
        // Assume dimmer is 0-100 percentage so make slider no 0-1 float but 0-100 int.
        //_Slider.wholeNumbers = true;
        //_Slider.minValue = 0;
        //_Slider.maxValue = 100;


        if (_PinchSlider == null) _PinchSlider = GetComponent<PinchSlider>();
        ConnectedItemController.updateItem?.Invoke();


        //if (_PinchSliderData == null) _PinchSliderData = GetComponent<SliderEventData>();
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
        float value = ConnectedItemController.GetItemStateAsDimmer();
        if (!Mathf.Approximately(value, sentValue))
        {
            if (Mathf.Abs(value / 100f - _PinchSlider.SliderValue) <= 0.01f) return;
            //Debug.Log("OnUpdate recieved state: " + value);
            // Failed to parse the dimmer
            if (value == -1 || value > 100)
            {
                _PinchSlider.SliderValue = 0f;
            }
            else
            {
                _PinchSlider.SliderValue = value / 100f;
            }
        }

    }

    /// <summary>
    /// Update item from UI. Call itemcontroller and update Item on server.
    /// If update is true, an event will be recieved. If state is equal no
    /// new UI update is necesarry. If not equal the PUT has failed and we need
    /// to revert UI state to server state.
    /// </summary>
    public void OnSetItem()
    {
        sentValue = (int)(_PinchSlider.SliderValue * 100f);
        ConnectedItemController.SetItemStateAsDimmer(sentValue);
    }

    /// <summary>
    /// Stop event listening from controller
    /// </summary>
    void OnDisable()
    {
        ConnectedItemController.updateItem -= OnUpdate;
    }
}
