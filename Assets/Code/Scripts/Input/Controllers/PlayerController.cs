using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private InputAction movement;

    private void OnEnable() {
        InputManager.ToggleActionMap(InputManager.inputActions.Player);
        movement=InputManager.inputActions.Player.Movement;

        InputManager.inputActions.Player.Shoot.performed+=DoShoot; 

        InputManager.inputActions.Player.PowerDropRelease.performed+=DoOne;    
        InputManager.inputActions.Player.PowerDropRelease.canceled+=DoTwo;   


        InputManager.inputActions.Player.MenuOpenClose.performed+=exitPlayer;
    }

    private void OnDisable() {
        
        InputManager.inputActions.Player.Shoot.performed-=DoShoot; 

        InputManager.inputActions.Player.PowerDropRelease.performed-=DoOne;    
        InputManager.inputActions.Player.PowerDropRelease.canceled-=DoTwo;  

        InputManager.inputActions.Player.MenuOpenClose.performed-=exitPlayer;
    }

    private void exitPlayer(InputAction.CallbackContext obj)
    {
        InputManager.ToggleActionMap(InputManager.inputActions.UI);
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
        Debug.Log("Movement Values: "+movement.ReadValue<Vector2>());
    }
}
