using MyBox;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ReBindUI : MonoBehaviour
{
    [SerializeField]
    private InputActionReference inputActionReference; //this reference is in the SO
    [SerializeField]
    private bool excludeMouse = true;
    [Range(0, 10)]
    [SerializeField]
    private int selectedBinding;
    [SerializeField]
    private InputBinding.DisplayStringOptions displayStringOptions;

    [Header("Binding Info - DO NOT EDIT")]
    [ReadOnly][SerializeField] private InputBinding inputBinding;
    [ReadOnly][SerializeField] private int bindingIndex;
    [ReadOnly][SerializeField] private string actionName;

    [Header("UI Fields")]
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private Button rebindButton;
    [SerializeField] private TextMeshProUGUI rebindText;
    [SerializeField] private Button resetButton;

    private void OnEnable()
    {
        rebindButton.onClick.AddListener(() => DoRebind());
        resetButton.onClick.AddListener(() => ResetBinding());

        if (inputActionReference != null)
        {
            InputManager.LoadBindingOverride(actionName);
            GetBindingInfo();
            UpdateUI();
        }

        InputManager.rebindComplete += UpdateUI;
        InputManager.rebindCanceled += UpdateUI;
    }

    private void OnDisable()
    {
        InputManager.rebindComplete -= UpdateUI;
        InputManager.rebindCanceled -= UpdateUI;
    }
    private void OnValidate()
    {
        if (inputActionReference == null)
            return;
        GetBindingInfo();
        UpdateUI();
    }

    private void GetBindingInfo()
    {
        if (inputActionReference.action != null)
            actionName = inputActionReference.action.name;

        if (inputActionReference.action.bindings.Count > selectedBinding)
        {
            inputBinding = inputActionReference.action.bindings[selectedBinding];
            bindingIndex = selectedBinding;
        }
    }

    private void UpdateUI()
    {
        if (actionText != null)
            actionText.text = actionName;

        if (rebindText != null)
        {
            if (Application.isPlaying)
            {
                rebindText.text = InputManager.GetBindingName(actionName, bindingIndex);
                if(InputManager.CheckDuplicateBindings(actionName,bindingIndex))
                    rebindText.color=Color.red;
                else
                    rebindText.color=new Color32(50,50,50,255);
            }
            else
                rebindText.text = inputActionReference.action.GetBindingDisplayString(bindingIndex);
        }
    }

    private void DoRebind()
    {
        InputManager.StartRebind(actionName, bindingIndex, rebindText,excludeMouse);
    }

    private void ResetBinding()
    {
        InputManager.ResetBinding(actionName,bindingIndex);
        UpdateUI();
    }
}
