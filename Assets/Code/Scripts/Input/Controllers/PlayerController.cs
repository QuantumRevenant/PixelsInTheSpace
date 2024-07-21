using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction movement;

    private void OnEnable() {
        InputManager.ToggleActionMap(InputManager.inputActions.PlayerActions);
        movement=InputManager.inputActions.PlayerActions.Movement;

        InputManager.inputActions.PlayerActions.Shoot.performed+=DoShoot; 

        InputManager.inputActions.PlayerActions.PowerDropRelease.performed+=DoOne;    
        InputManager.inputActions.PlayerActions.PowerDropRelease.canceled+=DoTwo;   

        InputManager.inputActions.PlayerActions.MenuOpenClose.performed+=exitPlayer;

        InputManager.rebindDuplicated+=DuplicateBinding;
    }

    private void OnDisable() {
        
        InputManager.inputActions.PlayerActions.Shoot.performed-=DoShoot; 

        InputManager.inputActions.PlayerActions.PowerDropRelease.performed-=DoOne;    
        InputManager.inputActions.PlayerActions.PowerDropRelease.canceled-=DoTwo;  

        InputManager.inputActions.PlayerActions.MenuOpenClose.performed-=exitPlayer;

        InputManager.rebindDuplicated-=DuplicateBinding;
    }

    private void exitPlayer(InputAction.CallbackContext obj)
    {
        InputManager.ToggleActionMap(InputManager.inputActions.UIActions);
    }
    private void DoShoot(InputAction.CallbackContext obj)
    {
        Debug.Log("Shoot");
    }
    private void DoOne(InputAction.CallbackContext obj)
    {
        Debug.Log("performed");
    }
    private void DoTwo(InputAction.CallbackContext obj)
    {
        Debug.Log("canceled");
    }
    

    private void FixedUpdate() {
        // Debug.Log("Movement Values: "+movement.ReadValue<Vector2>());
    }

    
    private void DuplicateBinding(string binding)
    {
        Debug.Log("Duplicate binding found: " + binding);
    }
}
