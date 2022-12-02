using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Player_Shooting : MonoBehaviour
{
    #region - Variables
    [Header("Player Stats")]
    [SerializeField] private float baseReloadTime;
    private float _reloadTimer;
    private bool isShooting = false;
    [Space(10)]

    [Header("Bullet Stats")]
    private Vector2 bVector;
    private Color bColor;
    [SerializeField] private float baseBulletSpeed;
    [SerializeField] private float baseBulletDamage;
    [SerializeField] private bool enemy;
    [SerializeField, Range(-1, 1)] private int direction;
    [Space(10)]

    [Header("Player Modificators")]
    private float mReload;
    private float mDamage;
    [Space(10)]

    [Header("Bullet Modificators")]
    private float mBSpeed;
    private float mBScale;
    [Space(10)]

    [Header("Bullets GameObject")]
    [SerializeField] private GameObject SimpleBullet;

    //Functionality
    private PlayerController playerController;

    #endregion

    #region - Action Voids

    private void shootPressed()
    {
        isShooting = true;
    }

    private void shootReleased()
    {
        isShooting = false;
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

        playerController.Player.DispararIn.performed += x => shootPressed();
        playerController.Player.DispararOut.performed += x => shootReleased();
    }
    // Start is called before the first frame update
    void Start()
    {
        _reloadTimer = 0;
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        GetVariables();
        Shoot();        
    }

    private void GetVariables()
    {
        Player_Manager PlayerManager;
        PlayerManager = gameObject.GetComponent<Player_Manager>();
        mReload = PlayerManager.multiplicatorReload;
        mDamage = PlayerManager.multiplicatorPlayerDamage;
        bVector = PlayerManager.playerBulletVector;
        mBScale = PlayerManager.multiplicatorPlayerBulletScale;
        mBSpeed = PlayerManager.multiplicatorPlayerBulletSpeed;
        bColor = PlayerManager.playerBulletColor;
        enemy = PlayerManager.bEnemy;
    }
    private void Shoot()
    {
        _reloadTimer += -(Time.deltaTime);

        if (Reload() == true)
        {
            _reloadTimer = baseReloadTime;
            Select();
        }

    }

    private bool Reload()
    {
        if (_reloadTimer <= 0 && isShooting == true)
        {            
            return true;
        }
        else { return false; }
        
    }

    private void Select()
    {
        Generate();
    }
    private void Generate()
    {   
        GameObject inst=Instantiate(SimpleBullet,transform.position,Quaternion.identity);
        Bullet_Script BulletScript= inst.GetComponent<Bullet_Script>();

        BulletScript.bulletVector = bVector;
        BulletScript.bulletColor = bColor;
        BulletScript.bulletSpeed = baseBulletSpeed * mBSpeed;
        BulletScript.multiplicatorBulletScale = mBScale;
        BulletScript.bulletDamage = baseBulletDamage * mDamage;
        if (direction != 0) { BulletScript.bulletDirection = direction; } else { BulletScript.bulletDirection = 1; }
        BulletScript.bulletEnemy=enemy;
        BulletScript.multiplicatorBulletScale=mBScale;

    }
}

