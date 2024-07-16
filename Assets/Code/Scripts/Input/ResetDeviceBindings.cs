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

    public void ResetAllBindings()
    {
        foreach (InputActionMap map in InputManager.inputActions.asset.actionMaps)
            map.RemoveAllBindingOverrides();
    }

    public void ResetControlSchemeBinding()
    {
        if(resetAllBindings)
        {
            ResetAllBindings();
            return;
        }

        foreach (InputActionMap map in InputManager.inputActions.asset.actionMaps)
            foreach (InputAction action in map.actions)
                action.RemoveBindingOverride(InputBinding.MaskByGroup(targetControlScheme));


    }

}
