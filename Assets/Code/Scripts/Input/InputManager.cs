using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
   public static PlayerInputActions inputActions;
   public static event Action<InputActionMap> actionMapChange;

   public static event Action rebindComplete;
   public static event Action rebindCanceled;
   public static event Action<InputAction, int> rebindStarted;

   private void Awake()
   {
      if (inputActions == null)
         inputActions = new PlayerInputActions();
   }

   public static void ToggleActionMap(InputActionMap actionMap)
   {
      if (actionMap.enabled)
         return;
      inputActions.Disable();
      actionMapChange?.Invoke(actionMap);
      actionMap.Enable();
   }

   public static void StartRebind(string actionName, int bindingIndex, TextMeshProUGUI statusText, bool excludeMouse)
   {
      InputAction action = inputActions.asset.FindAction(actionName);
      if (action == null || action.bindings.Count <= bindingIndex)
      {
         Debug.Log("Couldn't find action or binding");
         return;
      }

      if (action.bindings[bindingIndex].isComposite)
      {
         var firstPartIndex = bindingIndex + 1;
         if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
            DoRebind(action, firstPartIndex, statusText, true, excludeMouse);
      }
      else
         DoRebind(action, bindingIndex, statusText, false, excludeMouse);
   }

   private static void DoRebind(InputAction actionToRebind, int bindingIndex, TextMeshProUGUI statusText, bool allCompositeParts, bool excludeMouse)
   {
      if (actionToRebind == null || bindingIndex < 0)
         return;

      statusText.text = $"Press a {actionToRebind.expectedControlType}";

      actionToRebind.Disable();

      var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);

      void CleanUp()
      {
         rebind.Dispose();
         rebind = null;
      }

      rebind.OnComplete(operation =>
      {
         actionToRebind.Enable();
         operation.Dispose();

         if (allCompositeParts)
         {
            var nextBindingIndex = bindingIndex + 1;
            if (nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isPartOfComposite)
            {
               DoRebind(actionToRebind, nextBindingIndex, statusText, allCompositeParts, excludeMouse);
            }
         }

         if (CheckDuplicateBindings(actionToRebind, bindingIndex, allCompositeParts)) //If you try to duplicate an assignment, ignore the request and restart the process
         {
            actionToRebind.RemoveBindingOverride(bindingIndex);
            CleanUp();
            DoRebind(actionToRebind, bindingIndex, statusText, allCompositeParts, excludeMouse);
            return;
         }


         SaveBindingOverride(actionToRebind);
         rebindComplete?.Invoke();
      });

      rebind.OnCancel(operation =>
      {
         actionToRebind.Enable();
         operation.Dispose();

         rebindCanceled?.Invoke();
      });

      rebind.WithCancelingThrough("<Keyboard>/escape");

      if (excludeMouse)
         rebind.WithControlsExcluding("Mouse");

      rebindStarted?.Invoke(actionToRebind, bindingIndex);
      rebind.Start(); //actually starts the rebinding process
   }

   private static bool CheckDuplicateBindings(InputAction action, int bindingIndex, bool allCompositeParts = false)
   {
      InputBinding newBinding = action.bindings[bindingIndex];

      foreach (InputBinding binding in action.actionMap.bindings)
      {
         if (binding.action == newBinding.action)
            continue;
         if (binding.effectivePath == newBinding.effectivePath)
         {
            Debug.Log("Duplicate binding found: " + newBinding.effectivePath);
            return true;
         }
      }

      if (allCompositeParts)
      {
         for (int i = 1; i < bindingIndex; i++)
         {
            if (action.bindings[i].effectivePath == newBinding.overridePath)
            {
               Debug.Log("Duplicate binding found: " + newBinding.effectivePath);
               return true;
            }
         }
      }

      return false;
   }

   public static string GetBindingName(string actionName, int bindingIndex)
   {
      if (inputActions == null)
         inputActions = new PlayerInputActions();

      InputAction action = inputActions.asset.FindAction(actionName);
      return action.GetBindingDisplayString(bindingIndex);
   }

   private static void SaveBindingOverride(InputAction action)
   {
      for (int i = 0; i < action.bindings.Count; i++)
      {
         PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
      }
   }

   public static void LoadBindingOverride(string actionName)
   {
      if (actionName == null)
         return;

      if (inputActions == null)
         inputActions = new PlayerInputActions();

      InputAction action = inputActions.asset.FindAction(actionName);

      for (int i = 0; i < action.bindings.Count; i++)
      {
         if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + i)))
            action.ApplyBindingOverride(i, PlayerPrefs.GetString(action.actionMap + action.name + i));
      }
   }

   public static void ResetBinding(string actionName, int bindingIndex)
   {
      InputAction action = inputActions.asset.FindAction(actionName);

      if (action == null || action.bindings.Count <= bindingIndex)
      {
         Debug.Log("Could not find action or binding");
         return;
      }

      if (action.bindings[bindingIndex].isComposite)
      {
         for (int i = bindingIndex + 1; i < action.bindings.Count && action.bindings[i].isPartOfComposite; i++)
            ResetBinding(action, i);
      }
      else
         ResetBinding(action, bindingIndex);
      
      rebindComplete.Invoke();
      SaveBindingOverride(action);
   }

   public static void ResetBinding(InputAction action, int bindingIndex)
   {
      InputBinding newBinding = action.bindings[bindingIndex];
      string oldOverridePath = newBinding.overridePath;

      action.RemoveBindingOverride(bindingIndex);

      foreach (InputAction otherAction in action.actionMap)
      {
         if (otherAction == action)
            continue;

         for (int i = 0; i < otherAction.bindings.Count; i++)
         {
            InputBinding binding = otherAction.bindings[i];
            if (binding.overridePath == newBinding.path)
            {
               otherAction.ApplyBindingOverride(i, oldOverridePath);
               SaveBindingOverride(otherAction);
            }
         }
      }
      SaveBindingOverride(action);
   }
}
