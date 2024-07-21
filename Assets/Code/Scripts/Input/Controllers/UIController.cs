using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    private void OnEnable() {
        InputManager.inputActions.UIActions.Trigger.performed+=DoOne;

        InputManager.inputActions.UIActions.MenuOpenClose.performed+=exitUI;

    }

    private void OnDisable() {
        InputManager.inputActions.UIActions.Trigger.performed-=DoOne; 

        InputManager.inputActions.UIActions.MenuOpenClose.performed-=exitUI;
    }

    private void exitUI(InputAction.CallbackContext obj)
    {
        InputManager.ToggleActionMap(InputManager.inputActions.UIActions);
    }

    private void DoOne(InputAction.CallbackContext obj)
    {
        Debug.Log("A");
    }
}
