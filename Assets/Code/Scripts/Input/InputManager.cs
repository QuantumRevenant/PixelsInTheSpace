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
   public static event Action<string> rebindDuplicate;

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

         if (CheckDuplicateBindings(actionToRebind, bindingIndex, allCompositeParts)) //If you try to duplicate an assignment, ignore the request and restart the process
         {
            // rebind.Cancel();
            // return;
            BlockDuplicateActions(actionToRebind, bindingIndex, statusText, allCompositeParts, excludeMouse, CleanUp);
            return;
            // ToggleDuplicateActions(actionToRebind,bindingIndex);
         }

         if (allCompositeParts)
         {
            var nextBindingIndex = bindingIndex + 1;
            if (nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isPartOfComposite)
            {
               DoRebind(actionToRebind, nextBindingIndex, statusText, allCompositeParts, excludeMouse);
            }
         }

         SaveBindingOverride(actionToRebind);
         rebindComplete?.Invoke();
      });

      rebind.OnCancel(operation =>
      {
         actionToRebind.Enable();
         operation.Dispose();
         LoadBindingOverride(actionToRebind.name);
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
      Debug.Log(bindingIndex);

      // foreach (InputBinding binding in action.actionMap.bindings)
      // {
      //    if (binding.action == newBinding.action)
      //       continue;
      //    if (binding.effectivePath == newBinding.effectivePath)
      //    {
      //       rebindDuplicate.Invoke(newBinding.effectivePath);
      //       return true;
      //    }
      // }

      // if (allCompositeParts)
      // {
      //    for (int i = 1; i < bindingIndex; i++)
      //    {
      //       if (action.bindings[i].effectivePath == newBinding.overridePath)
      //       {
      //          rebindDuplicate.Invoke(newBinding.effectivePath);
      //          return true;
      //       }
      //    }
      // }

      // return false;
      var allBindings = action.actionMap.bindings;

      for (int i = 0; i < allBindings.Count; i++)
      {
         Debug.Log(i);

         var binding=allBindings[i];

         if (binding.action == newBinding.action)
         {
            if (binding.isPartOfComposite && i != bindingIndex)
            {
               if (binding.effectivePath == newBinding.effectivePath)
               {
                  rebindDuplicate.Invoke(newBinding.effectivePath);
                  return true;
               }
            }
            else
               continue;
         }

         if (allBindings[i].effectivePath == newBinding.effectivePath)
         {
            rebindDuplicate.Invoke(newBinding.effectivePath);
            return true;
         }
      }

      if (allCompositeParts)
      {
         for (int i = 1; i < bindingIndex; i++)
         {
            if (action.bindings[i].effectivePath == newBinding.overridePath)
            {
               rebindDuplicate.Invoke(newBinding.effectivePath);
               return true;
            }
         }
      }

      return false;


      int currentIndex = -1;

      foreach (InputBinding binding in action.actionMap.bindings)
      {
         currentIndex++;
         Debug.Log("IF 1");

         if (binding.action == newBinding.action)
         {
            Debug.Log("IF 1A");
            if (binding.isComposite || currentIndex == bindingIndex)
            {
               continue;
            }
            else
            {
               Debug.Log("IF 1B");
               if (binding.effectivePath == newBinding.effectivePath)
               {
                  rebindDuplicate.Invoke(newBinding.effectivePath);
                  return true;
               }
            }
         }
         Debug.Log("IF 2");

         if (binding.effectivePath == newBinding.effectivePath)
         {
            rebindDuplicate.Invoke(newBinding.effectivePath);
            return true;
         }

      }
      Debug.Log("IF 3");
      if (allCompositeParts)
      {
         for (int i = 1; i < bindingIndex; i++)
         {
            if (action.bindings[i].effectivePath == newBinding.overridePath)
            {
               rebindDuplicate.Invoke(newBinding.effectivePath);
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
      ToggleDuplicateActions(action, bindingIndex);

      action.RemoveBindingOverride(bindingIndex);

      SaveBindingOverride(action);
   }

   public static void ToggleDuplicateActions(InputAction action, int bindingIndex)
   {
      InputBinding newBinding = action.bindings[bindingIndex];
      string oldOverridePath = newBinding.overridePath;

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
   }

   public static void BlockDuplicateActions(InputAction action, int bindingIndex, TextMeshProUGUI statusText, bool allCompositeParts, bool excludeMouse, Action cleanup)
   {
      action.RemoveBindingOverride(bindingIndex);
      cleanup.Invoke();
      DoRebind(action, bindingIndex, statusText, allCompositeParts, excludeMouse);
   }

   public static void ResetAllBindings()
   {
      foreach (InputActionMap map in InputManager.inputActions.asset.actionMaps)
         map.RemoveAllBindingOverrides();
   }

   public static void ResetControlSchemeBinding(string targetControlScheme)
   {
      foreach (InputActionMap map in InputManager.inputActions.asset.actionMaps)
         foreach (InputAction action in map.actions)
            action.RemoveBindingOverride(InputBinding.MaskByGroup(targetControlScheme));
   }
}
