﻿using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[CreateAssetMenu(fileName = "NewButtonHandler")]
public class ButtonHandler : InputHandler
{
    public InputHelpers.Button button = InputHelpers.Button.None;

    public delegate void StateChange(XRController controller);
    public event StateChange OnButtonDown;
    public event StateChange OnButtonUp;

    private bool previousPress = false;

    public override void HandleState(XRController controller)
    {
        if(controller.inputDevice.IsPressed(button, out bool pressed, controller.axisToPressThreshold))
        {
            if(previousPress != pressed)
            {
                previousPress = pressed;

                if (pressed)
                {
                    //Check if null
                    OnButtonDown?.Invoke(controller);
                }
                else
                {
                    OnButtonUp?.Invoke(controller);
                }

            }
        }

    }
}