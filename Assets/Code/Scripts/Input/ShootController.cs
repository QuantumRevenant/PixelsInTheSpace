using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootController : MonoBehaviour
{
   private PlayerController gameActions;
   private void OnEnable()
   {
    gameActions=InputManager.inputActions;
    gameActions.InGame.Shoot.started+=DoShoot;
    gameActions.InGame.Enable();
   }

   private void DoShoot(InputAction.CallbackContext obj)
   {
        Debug.Log("pew");
   }
}
