using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
   public static PlayerInputActions inputActions=new PlayerInputActions();
   public static event Action<InputActionMap> actionMapChange;

   private void Start() {

   }

   public static void ToggleActionMap(InputActionMap actionMap)
   {
    if(actionMap.enabled)
        return;
        inputActions.Disable();
        actionMapChange?.Invoke(actionMap);
        actionMap.Enable();
   }

   
}
