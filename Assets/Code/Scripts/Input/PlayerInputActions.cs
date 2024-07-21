//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Code/Scripts/Input/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerActions"",
            ""id"": ""4ec1e714-2674-4675-82ff-dd40e2973482"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""48ea9928-c064-43b7-9e03-5898b2e6daa9"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""TypeSelection"",
                    ""type"": ""Button"",
                    ""id"": ""5e6309b2-6985-47ce-9992-5568cfa1da6d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""045eca1a-1723-4f19-b622-4c2061378a0a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""PowerActivate"",
                    ""type"": ""Button"",
                    ""id"": ""d3f8def3-84df-4a4a-b3dd-d660b8548f5c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""5dcb6d5f-140e-4cc4-bd38-ddb799c6947c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InstantDash"",
                    ""type"": ""Value"",
                    ""id"": ""35c1a4a2-664c-419c-ab1b-015cdf529f9a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""PowerDropRelease"",
                    ""type"": ""Button"",
                    ""id"": ""0e73ec47-d383-4f18-acf9-be5a7df95d2f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MenuOpenClose"",
                    ""type"": ""Button"",
                    ""id"": ""aa7543f4-3ab5-4078-99cb-9cb111ecb63d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3c76acb9-328f-4414-b4da-5f69e58359d7"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PowerDropRelease"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""04d745c3-aa2b-45cc-ba00-f9d2f5082ff5"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PowerDropRelease"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f0cb93f0-5e08-4561-b5a0-d32e620980af"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PowerDropRelease"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""222e82e1-3a78-4771-a967-bf0167c4d053"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PowerDropRelease"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""968a4c6f-65eb-4b65-9fb4-2cbd61be3388"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MenuOpenClose"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6376fdf1-9d57-48e0-b0f8-86ae8c3d8d42"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MenuOpenClose"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""dcd60e8f-52d0-42b0-a0d7-a7efaadd5bee"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""64c31788-ebb9-48ab-af89-47fec2d5069e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""b00d7fcd-057d-4ad8-9813-b08fe8b378d6"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3795fe07-cb45-478b-9453-69f94faa43c0"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""ad0841f9-7f5a-45be-8afd-1499bd52a8d8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""cfe1fef0-1a90-449d-bb21-d7651df48fc5"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8d0e2e65-14fe-481f-8b95-54344f322643"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""97fab78f-8daf-408c-ad23-2a372504693e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TypeSelection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""13f10980-79fa-41d4-b134-f3b0f96bd982"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TypeSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""fc673a8c-dac6-4cd4-90cc-d69764e287ff"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TypeSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""73bd4db4-e6ae-4d1f-bbc2-277783bbb460"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TypeSelection"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""dea1ebb4-e806-4d4b-8986-4259283de629"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TypeSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""ed12453b-4dba-4d58-8399-9e4ad2890f2e"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""TypeSelection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""bc2cc1bd-68a7-46ba-8ebb-b32f00ab2e19"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f9545d60-8fab-4e2a-8eee-fad3241a845c"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f99837a3-64ae-4a10-ba9a-2081ee4b3ef7"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b1515fcb-5569-493e-90f6-d3c8290d719c"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f15cd2e7-126d-4258-b0cf-0575ac095852"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""InstantDash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""606f83b5-c9b1-490a-b09a-715417b03856"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43a574f2-cb06-4890-b0ea-d940931692b8"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e9c2440f-48bb-4b81-97e2-6a86d64ed615"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PowerActivate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9d00f6c2-13ca-4f25-a69b-129ef64ea6fb"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""PowerActivate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""df1585e1-cd75-4de1-a35a-9cafb38bd33c"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PowerActivate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82c4ffad-3eda-47c9-828e-2ec8cab3e079"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PowerActivate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UIActions"",
            ""id"": ""f54774e3-0d76-4e8e-8b27-85f1fcd72ffe"",
            ""actions"": [
                {
                    ""name"": ""MenuOpenClose"",
                    ""type"": ""Button"",
                    ""id"": ""75916f30-67db-4fde-ba25-cffa3f34a630"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Trigger"",
                    ""type"": ""Button"",
                    ""id"": ""d1d314aa-b74f-4ed6-b337-a31fa12dfb5d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3b6a17d2-cd47-4017-bb46-6d47d42881c5"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuOpenClose"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e50efd87-f3e7-49ab-9170-d5434dfc80d2"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuOpenClose"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3f57561-89ef-4955-8f1c-9ca26b071342"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Trigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerActions
        m_PlayerActions = asset.FindActionMap("PlayerActions", throwIfNotFound: true);
        m_PlayerActions_Movement = m_PlayerActions.FindAction("Movement", throwIfNotFound: true);
        m_PlayerActions_TypeSelection = m_PlayerActions.FindAction("TypeSelection", throwIfNotFound: true);
        m_PlayerActions_Shoot = m_PlayerActions.FindAction("Shoot", throwIfNotFound: true);
        m_PlayerActions_PowerActivate = m_PlayerActions.FindAction("PowerActivate", throwIfNotFound: true);
        m_PlayerActions_Dash = m_PlayerActions.FindAction("Dash", throwIfNotFound: true);
        m_PlayerActions_InstantDash = m_PlayerActions.FindAction("InstantDash", throwIfNotFound: true);
        m_PlayerActions_PowerDropRelease = m_PlayerActions.FindAction("PowerDropRelease", throwIfNotFound: true);
        m_PlayerActions_MenuOpenClose = m_PlayerActions.FindAction("MenuOpenClose", throwIfNotFound: true);
        // UIActions
        m_UIActions = asset.FindActionMap("UIActions", throwIfNotFound: true);
        m_UIActions_MenuOpenClose = m_UIActions.FindAction("MenuOpenClose", throwIfNotFound: true);
        m_UIActions_Trigger = m_UIActions.FindAction("Trigger", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerActions
    private readonly InputActionMap m_PlayerActions;
    private IPlayerActionsActions m_PlayerActionsActionsCallbackInterface;
    private readonly InputAction m_PlayerActions_Movement;
    private readonly InputAction m_PlayerActions_TypeSelection;
    private readonly InputAction m_PlayerActions_Shoot;
    private readonly InputAction m_PlayerActions_PowerActivate;
    private readonly InputAction m_PlayerActions_Dash;
    private readonly InputAction m_PlayerActions_InstantDash;
    private readonly InputAction m_PlayerActions_PowerDropRelease;
    private readonly InputAction m_PlayerActions_MenuOpenClose;
    public struct PlayerActionsActions
    {
        private @PlayerInputActions m_Wrapper;
        public PlayerActionsActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerActions_Movement;
        public InputAction @TypeSelection => m_Wrapper.m_PlayerActions_TypeSelection;
        public InputAction @Shoot => m_Wrapper.m_PlayerActions_Shoot;
        public InputAction @PowerActivate => m_Wrapper.m_PlayerActions_PowerActivate;
        public InputAction @Dash => m_Wrapper.m_PlayerActions_Dash;
        public InputAction @InstantDash => m_Wrapper.m_PlayerActions_InstantDash;
        public InputAction @PowerDropRelease => m_Wrapper.m_PlayerActions_PowerDropRelease;
        public InputAction @MenuOpenClose => m_Wrapper.m_PlayerActions_MenuOpenClose;
        public InputActionMap Get() { return m_Wrapper.m_PlayerActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActionsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActionsActions instance)
        {
            if (m_Wrapper.m_PlayerActionsActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnMovement;
                @TypeSelection.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnTypeSelection;
                @TypeSelection.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnTypeSelection;
                @TypeSelection.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnTypeSelection;
                @Shoot.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnShoot;
                @PowerActivate.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnPowerActivate;
                @PowerActivate.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnPowerActivate;
                @PowerActivate.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnPowerActivate;
                @Dash.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnDash;
                @InstantDash.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnInstantDash;
                @InstantDash.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnInstantDash;
                @InstantDash.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnInstantDash;
                @PowerDropRelease.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnPowerDropRelease;
                @PowerDropRelease.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnPowerDropRelease;
                @PowerDropRelease.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnPowerDropRelease;
                @MenuOpenClose.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnMenuOpenClose;
                @MenuOpenClose.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnMenuOpenClose;
                @MenuOpenClose.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnMenuOpenClose;
            }
            m_Wrapper.m_PlayerActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @TypeSelection.started += instance.OnTypeSelection;
                @TypeSelection.performed += instance.OnTypeSelection;
                @TypeSelection.canceled += instance.OnTypeSelection;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @PowerActivate.started += instance.OnPowerActivate;
                @PowerActivate.performed += instance.OnPowerActivate;
                @PowerActivate.canceled += instance.OnPowerActivate;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @InstantDash.started += instance.OnInstantDash;
                @InstantDash.performed += instance.OnInstantDash;
                @InstantDash.canceled += instance.OnInstantDash;
                @PowerDropRelease.started += instance.OnPowerDropRelease;
                @PowerDropRelease.performed += instance.OnPowerDropRelease;
                @PowerDropRelease.canceled += instance.OnPowerDropRelease;
                @MenuOpenClose.started += instance.OnMenuOpenClose;
                @MenuOpenClose.performed += instance.OnMenuOpenClose;
                @MenuOpenClose.canceled += instance.OnMenuOpenClose;
            }
        }
    }
    public PlayerActionsActions @PlayerActions => new PlayerActionsActions(this);

    // UIActions
    private readonly InputActionMap m_UIActions;
    private IUIActionsActions m_UIActionsActionsCallbackInterface;
    private readonly InputAction m_UIActions_MenuOpenClose;
    private readonly InputAction m_UIActions_Trigger;
    public struct UIActionsActions
    {
        private @PlayerInputActions m_Wrapper;
        public UIActionsActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MenuOpenClose => m_Wrapper.m_UIActions_MenuOpenClose;
        public InputAction @Trigger => m_Wrapper.m_UIActions_Trigger;
        public InputActionMap Get() { return m_Wrapper.m_UIActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActionsActions set) { return set.Get(); }
        public void SetCallbacks(IUIActionsActions instance)
        {
            if (m_Wrapper.m_UIActionsActionsCallbackInterface != null)
            {
                @MenuOpenClose.started -= m_Wrapper.m_UIActionsActionsCallbackInterface.OnMenuOpenClose;
                @MenuOpenClose.performed -= m_Wrapper.m_UIActionsActionsCallbackInterface.OnMenuOpenClose;
                @MenuOpenClose.canceled -= m_Wrapper.m_UIActionsActionsCallbackInterface.OnMenuOpenClose;
                @Trigger.started -= m_Wrapper.m_UIActionsActionsCallbackInterface.OnTrigger;
                @Trigger.performed -= m_Wrapper.m_UIActionsActionsCallbackInterface.OnTrigger;
                @Trigger.canceled -= m_Wrapper.m_UIActionsActionsCallbackInterface.OnTrigger;
            }
            m_Wrapper.m_UIActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MenuOpenClose.started += instance.OnMenuOpenClose;
                @MenuOpenClose.performed += instance.OnMenuOpenClose;
                @MenuOpenClose.canceled += instance.OnMenuOpenClose;
                @Trigger.started += instance.OnTrigger;
                @Trigger.performed += instance.OnTrigger;
                @Trigger.canceled += instance.OnTrigger;
            }
        }
    }
    public UIActionsActions @UIActions => new UIActionsActions(this);
    private int m_KeyboardSchemeIndex = -1;
    public InputControlScheme KeyboardScheme
    {
        get
        {
            if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
            return asset.controlSchemes[m_KeyboardSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IPlayerActionsActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnTypeSelection(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnPowerActivate(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnInstantDash(InputAction.CallbackContext context);
        void OnPowerDropRelease(InputAction.CallbackContext context);
        void OnMenuOpenClose(InputAction.CallbackContext context);
    }
    public interface IUIActionsActions
    {
        void OnMenuOpenClose(InputAction.CallbackContext context);
        void OnTrigger(InputAction.CallbackContext context);
    }
}
