using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
   public static PlayerInputActions inputActions;
   public static bool duplicateBindingsAllowed = false;
   public static bool autoSave = true;
   public static event Action<InputActionMap> actionMapChange;

   public static event Action rebindComplete;
   public static event Action rebindCanceled;
   public static event Action<InputAction, int> rebindStarted;
   public static event Action<string> rebindDuplicated;
   private void Awake()
   {
      inputActions ??= new PlayerInputActions();
   }

   [ContextMenu("Toogle duplicateBindingsAllowed")]
   public void ToogleDuplicateBindingsAllowed()
   {
      duplicateBindingsAllowed = !duplicateBindingsAllowed;
      Debug.Log($"duplicateBindingsAllowed: {duplicateBindingsAllowed}");
   }

   [ContextMenu("Toogle autoSave")]
   public void ToogleAutoSave()
   {
      autoSave = !autoSave;
      Debug.Log($"duplicateBindingsAllowed: {autoSave}");
   }

   [ContextMenu("Save All")]
   public void SaveAllBindingsLocal()
   {
      SaveAllBindings();
   }
   public static void SaveAllBindings()
   {
      foreach (InputAction action in inputActions)
         SaveBindingOverride(action);

   }

   [ContextMenu("Reset All")]
   public void ResetAllBindingsLocal()
   {
      ResetAllBindings();
   }
   public static void ResetAllBindings()
   {
      foreach (InputActionMap map in inputActions.asset.actionMaps)
         map.RemoveAllBindingOverrides();
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
      string oldOverridePath = actionToRebind.bindings[bindingIndex].effectivePath;
      var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);

      rebind.OnComplete(operation =>
      {
         actionToRebind.Enable();
         operation.Dispose();

         if (CheckDuplicateBindings(actionToRebind, bindingIndex, allCompositeParts) && !duplicateBindingsAllowed)
            ToggleDuplicateActions(actionToRebind, bindingIndex, actionToRebind.bindings[bindingIndex].overridePath, oldOverridePath);

         if (allCompositeParts)
            if (bindingIndex + 1 < actionToRebind.bindings.Count && actionToRebind.bindings[bindingIndex + 1].isPartOfComposite)
               DoRebind(actionToRebind, bindingIndex + 1, statusText, allCompositeParts, excludeMouse);

         if (autoSave)
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

   public static bool CheckDuplicateBindings(string actionName, int bindingIndex)
   {
      if (actionName == null)
         return true;

      inputActions ??= new PlayerInputActions();

      InputAction action = inputActions.asset.FindAction(actionName);

      bool output = false;
      if (!action.bindings[bindingIndex].isComposite)
         output |= CheckDuplicateBindings(action, bindingIndex, true);
      else if (bindingIndex + 1 < action.bindings.Count && action.bindings[bindingIndex + 1].isPartOfComposite)
         output |= CheckDuplicateBindings(action, bindingIndex + 1, true);

      return output;
   }

   private static bool CheckDuplicateBindings(InputAction action, int bindingIndex, bool allCompositeParts = false)
   {
      InputBinding newBinding = action.bindings[bindingIndex];

      foreach (InputBinding binding in action.actionMap.bindings)
      {

         if (binding.action == newBinding.action)
         {
            if (binding.isPartOfComposite && binding.id != newBinding.id)
            {
               if (binding.effectivePath == newBinding.effectivePath)
               {
                  rebindDuplicated?.Invoke(newBinding.effectivePath);
                  return true;
               }
            }
            else
               continue;
         }

         if (binding.effectivePath == newBinding.effectivePath)
         {
            rebindDuplicated?.Invoke(newBinding.effectivePath);
            return true;
         }
      }

      if (allCompositeParts)
      {
         for (int i = 1; i < bindingIndex; i++)
         {
            if (action.bindings[i].effectivePath == newBinding.overridePath)
            {
               rebindDuplicated?.Invoke(newBinding.effectivePath);
               return true;
            }
         }
      }

      return false;
   }

   public static string GetBindingName(string actionName, int bindingIndex, InputBinding.DisplayStringOptions displayStringOptions)
   {
      inputActions ??= new PlayerInputActions();

      InputAction action = inputActions.asset.FindAction(actionName);
      InputBinding binding = action.bindings[bindingIndex];
      if (!binding.isComposite)
         return action.bindings[bindingIndex].ToDisplayString(displayStringOptions);
      return action.GetBindingDisplayString(bindingIndex,displayStringOptions);
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

      inputActions ??= new PlayerInputActions();

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

      if (autoSave)
         SaveBindingOverride(action);

      rebindComplete?.Invoke();
   }

   public static void ResetBinding(InputAction action, int bindingIndex)
   {
      ToggleDuplicateActions(action, bindingIndex, action.bindings[bindingIndex].path, action.bindings[bindingIndex].overridePath);

      action.RemoveBindingOverride(bindingIndex);

      if (autoSave)
         SaveBindingOverride(action);
   }

   public static void ToggleDuplicateActions(InputAction action, int bindingIndex, string path, string oldOverridePath)
   {
      InputBinding newBinding = action.bindings[bindingIndex];

      foreach (InputAction otherAction in action.actionMap)
      {
         for (int i = 0; i < otherAction.bindings.Count; i++)
         {
            InputBinding binding = otherAction.bindings[i];
            if (binding.effectivePath == path && binding.id != newBinding.id)
            {
               otherAction.ApplyBindingOverride(i, oldOverridePath);
               if (autoSave)
                  SaveBindingOverride(otherAction);
            }
         }
      }
   }

   public static void RetryDuplicateActions(InputAction action, int bindingIndex, TextMeshProUGUI statusText, bool allCompositeParts, bool excludeMouse, Action cleanup)
   {
      action.RemoveBindingOverride(bindingIndex);
      cleanup?.Invoke();
      DoRebind(action, bindingIndex, statusText, allCompositeParts, excludeMouse);
   }
   
   public static void ResetControlSchemeBinding(string targetControlScheme)
   {
      foreach (InputActionMap map in InputManager.inputActions.asset.actionMaps)
         foreach (InputAction action in map.actions)
            action.RemoveBindingOverride(InputBinding.MaskByGroup(targetControlScheme));
   }
}
