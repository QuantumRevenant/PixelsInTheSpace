using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    public static UserInput instance;

    public Vector2 MoveInput { get; private set; }
    public bool DropReleaseJustPressed { get; private set; }
    public bool DropReleaseBeingHeld { get; private set; }
    public bool DropReleaseReleased { get; private set; }
    public bool ActivateInput { get; private set; }

    public bool ShootInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool MenuOpenCloseInput { get; private set; }

    private PlayerInput _playerInput;

    private InputAction _moveAction;
    private InputAction _dropReleaseAction;
    private InputAction _activateAction;
    private InputAction _shootAction;
    private InputAction _dashAction;
    private InputAction _menuOpenCloseAction;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        _playerInput = GetComponent<PlayerInput>();

        setupInputActions();
    }

    private void Update()
    {
        UpdateInputs();
    }

    private void setupInputActions()
    {
        _moveAction=_playerInput.actions["Move"];
        _dropReleaseAction=_playerInput.actions["PowerDropRelease"];
        _activateAction=_playerInput.actions["PowerActivate"];
        _shootAction=_playerInput.actions["Shoot"];
        _dashAction=_playerInput.actions["Dash"];
        _menuOpenCloseAction=_playerInput.actions["MenuOpenClose"];
    }

    private void UpdateInputs()
    {
        MoveInput=_moveAction.ReadValue<Vector2>();
    DropReleaseJustPressed=_dropReleaseAction.WasPressedThisFrame();
    DropReleaseBeingHeld=_dropReleaseAction.IsPressed();
    DropReleaseReleased=_dropReleaseAction.WasReleasedThisFrame();
    ActivateInput=_activateAction.WasPressedThisFrame();

    ShootInput=_shootAction.WasPressedThisFrame();
    DashInput=_dashAction.WasPressedThisFrame();
    MenuOpenCloseInput=_menuOpenCloseAction.WasPressedThisFrame();
    }
}
