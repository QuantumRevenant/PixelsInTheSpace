using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
public class Player_Movement : MonoBehaviour
{
    #region - Variables

    [Header("Player Stats")]
    [SerializeField] private float baseSpeed;
    [SerializeField] private Vector2 baseVector;

    [Header("Modificators")]
    private float multiplicatorSpeed;
    private float multiplicatorScale;
    private float size;
    private Vector2 boundaries;
    
    [Header("Functionality")]
    [SerializeField] private Vector3 inputVector;
    private PlayerController playerController;
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
        size = PlayerManager.playerSize;
        boundaries = PlayerManager.playerBoundaries;
    }
    private void Movement()
    {
        Vector3 vectorInicial = new Vector3(inputVector.x * baseVector.x, inputVector.y * baseVector.y, inputVector.z);
        Vector3 vectorFinal = vectorInicial * baseSpeed * multiplicatorSpeed * Time.deltaTime;
        transform.position += vectorFinal;
    }
    private void Boundaries()
    {
        float xActual = transform.position.x;
        float yActual = transform.position.y;

        xActual=Mathf.Clamp(xActual, -boundaries.x+(size/ 2), boundaries.x - (size/ 2));
        yActual=Mathf.Clamp(yActual, -boundaries.y, boundaries.y - (size / 2));

        transform.position = new Vector3(xActual, yActual, 0);
    }  
}
