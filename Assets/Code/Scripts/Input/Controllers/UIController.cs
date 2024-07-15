using UnityEngine;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    private void OnEnable() {
        InputManager.inputActions.UI.Trigger.performed+=DoOne;

        InputManager.inputActions.UI.MenuOpenClose.performed+=exitUI;

    }

    private void OnDisable() {
        InputManager.inputActions.UI.Trigger.performed-=DoOne; 

        InputManager.inputActions.UI.MenuOpenClose.performed-=exitUI;
    }

    private void exitUI(InputAction.CallbackContext obj)
    {
        InputManager.ToggleActionMap(InputManager.inputActions.UI);
    }

    private void DoOne(InputAction.CallbackContext obj)
    {
        Debug.Log("A");
    }
}
