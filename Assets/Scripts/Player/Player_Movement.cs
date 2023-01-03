using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
[RequireComponent(typeof(Player_Manager))]
public class Player_Movement : MonoBehaviour
{
    [Header("Modificators")]
    private float multiplicatorSpeed;
    private float multiplicatorScale;
    [Space(10)]
    [Header("Functionality")]
    [HideInInspector] public PlayerInput playerInput;
    [Space(10)]
    [SerializeField] private PlayerData playerData;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerData = gameObject.GetComponent<Player_Manager>().playerData;
    }
    void Update()
    {
        GetVariables();
        Movement(playerInput.actions["Move"].ReadValue<Vector2>());
    }
    
    private void GetVariables()
    {
        Player_Manager PlayerManager;
        PlayerManager = gameObject.GetComponent<Player_Manager>();
        multiplicatorScale = PlayerManager.multiplicatorScale;
        multiplicatorSpeed = PlayerManager.multiplicatorSpeed;
    }
    private void Movement(Vector2 inputVector)
    {
        Vector3 vectorInicial = new Vector3(inputVector.x * playerData.BaseVector.x, inputVector.y * playerData.BaseVector.y, 0);
        Vector3 vectorFinal = vectorInicial * playerData.BaseSpeed * multiplicatorSpeed * Time.deltaTime;
        transform.Translate(vectorFinal);
        Boundaries();
    }
    private void Boundaries()
    {
        Vector3 PosAndSize;
        PosAndSize.x = transform.position.x;
        PosAndSize.y = transform.position.y;
        PosAndSize.z = playerData.PlayerSize * multiplicatorScale;

        PosAndSize.x = Mathf.Clamp(PosAndSize.x, -playerData.PlayerBoundaries.x + (PosAndSize.z / 2), playerData.PlayerBoundaries.x - (PosAndSize.z / 2));
        PosAndSize.y = Mathf.Clamp(PosAndSize.y, -playerData.PlayerBoundaries.y, playerData.PlayerBoundaries.y - (PosAndSize.z / 2));

        PosAndSize.z = transform.position.z;
        transform.position = PosAndSize;
    }
}
