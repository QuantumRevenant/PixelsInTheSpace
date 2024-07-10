using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ResetDeviceBindings : MonoBehaviour
{
    [SerializeField] private InputActionAsset _inputActions;
    [SerializeField] private string _targetControlScheme;

    public void ResetAllBidings()
    {
        foreach (InputActionMap map in _inputActions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
    }

    public void ResetControlSchemeBinding()
    {
        foreach (InputActionMap map in _inputActions.actionMaps)
        {
            foreach (InputAction actions in map.actions)
            {
                actions.RemoveBindingOverride(InputBinding.MaskByGroup(_targetControlScheme));
            }
        }
    }
}
