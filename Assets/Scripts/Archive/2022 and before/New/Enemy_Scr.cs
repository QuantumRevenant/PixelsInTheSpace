using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Scr : MonoBehaviour
{
        [Header("Life")]
    [SerializeField][Range(0, 10)] private float _healthPoints = 5f;
    private bool _isInvulnerable = false, _isShielded = false;
    private GameObject _shield;
    [Space(10)]

    [Header("Shooting")]
    [SerializeField]private float _reloadTimer;
    [SerializeField]private ShootingTypes _shootingType = ShootingTypes.Simple;
    private GameObject _laser;
    [Space(10)]

    [Header("Motion")]

    [Space(10)]

    [Header("Management")]
    [SerializeField]private int score = 0;
    public int Score { get { return score; } }

    [Header("Inventory")]
    [SerializeField][Range(1, 5)] private int _inventoryPosition = 1;
    private int _inventorySize = 5;
    [SerializeField]private List<Items> _inventory;
    private bool _canScroll=true;
    private const int _BaseInvSize = 5;
    private const float _ScrollCooldown = 0.25f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Management
    #endregion
    #region Life
    #endregion
    #region Shooting
    #endregion
    #region Motion
    #endregion
    #region Inventory
    #endregion
    #region Variables
    #endregion
}