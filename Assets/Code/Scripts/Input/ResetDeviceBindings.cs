using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResetDeviceBindings : MonoBehaviour
{   
    [SerializeField] private bool resetAllBindings=true;
    [SerializeField] private string targetControlScheme;

    public void ResetBindings()
    {
        if(resetAllBindings)
        {
            InputManager.ResetAllBindings();
            return;
        }
        InputManager.ResetControlSchemeBinding(targetControlScheme);
    }
}
