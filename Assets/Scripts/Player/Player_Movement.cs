using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
public class Player_Movement : MonoBehaviour
{
    #region - Variables
    [Header("Modificators")]
    private float multiplicatorSpeed;
    private float multiplicatorScale;
    [Space(10)]

    [Header("Functionality")]
    // [SerializeField] 
    private Vector3 inputVector;
    private PlayerController playerController;
    [Space(10)]

    [SerializeField] PlayerData playerData;
    #endregion

    #region - Action Voids
    private void OnMovimiento(InputValue valor)
    {
        Vector2 inputMovimiento = valor.Get<Vector2>();
        inputVector = new Vector3(inputMovimiento.x, inputMovimiento.y, 0);
    }
    #endregion


    #region - Enable/Disable
    private void OnEnable()
    {
        playerController.Enable();
    }

    private void OnDisable()
    {
        playerController.Disable();
    }

    #endregion


    #region - Awake/Start
    private void Awake()
    {
        playerController = new PlayerController();
    }

    // Start is called before the first frame update
    void Start()
    {

    }
    #endregion


    // Update is called once per frame
    void Update()
    {
        GetVariables();
        Movement();
        Boundaries();
    }

    private void GetVariables()
    {   Player_Manager PlayerManager;
        PlayerManager = gameObject.GetComponent<Player_Manager>();
        multiplicatorSpeed = PlayerManager.multiplicatorSpeed;
    }
    private void Movement()
    {
        Vector3 vectorInicial = new Vector3(inputVector.x * playerData.BaseVector.x, inputVector.y * playerData.BaseVector.y, inputVector.z);
        Vector3 vectorFinal = vectorInicial * playerData.BaseSpeed * multiplicatorSpeed * Time.deltaTime;
        transform.position += vectorFinal;
    }
    private void Boundaries()
    {
        Vector3 PosAndSize;
        PosAndSize.x = transform.position.x;
        PosAndSize.y = transform.position.y;
        PosAndSize.z=playerData.PlayerSize*multiplicatorScale;

        PosAndSize.x=Mathf.Clamp(PosAndSize.x, -playerData.PlayerBoundaries.x+(PosAndSize.z/ 2), playerData.PlayerBoundaries.x - (PosAndSize.z/ 2));
        PosAndSize.y=Mathf.Clamp(PosAndSize.y, -playerData.PlayerBoundaries.y, playerData.PlayerBoundaries.y - (PosAndSize.z/ 2));

        PosAndSize.z=transform.position.z;

        transform.position = PosAndSize;
    }  
}
