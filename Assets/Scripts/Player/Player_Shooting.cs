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
    private float reloadTimer;
    private bool isShooting = false;
    [Space(10)]

    [Header("Bullet Stats")]
    private Vector2 bullletVector;
    private Color bullletColor;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float baseDamage;
    [SerializeField] private bool isEnemy;
    [SerializeField, Range(-1, 1)] private int direction;
    [Space(10)]

    [Header("Player Modificators")]
    private float multiplicatorReload;
    private float multiplicatorDamage;
    [Space(10)]

    [Header("Bullet Modificators")]
    private float multiplicatorBulletSpeed;
    private float multiplicatorBulletScale;
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
        reloadTimer = 0;
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
        multiplicatorReload = PlayerManager.multiplicatorReload;
        multiplicatorDamage
 = PlayerManager.multiplicatorDamage;
        bullletVector = PlayerManager.bulletVector;
        multiplicatorBulletScale = PlayerManager.multiplicatorBulletScale;
        multiplicatorBulletSpeed = PlayerManager.multiplicatorBulletSpeed;
        bullletColor = PlayerManager.bulletColor;
        isEnemy = PlayerManager.bulletEnemy;
    }
    private void Shoot()
    {
        reloadTimer += -(Time.deltaTime);

        if (Reload() == true)
        {
            reloadTimer = baseReloadTime;
            Select();
        }

    }

    private bool Reload()
    {
        if (reloadTimer <= 0 && isShooting == true)
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

        BulletScript.bulletVector = bullletVector;
        BulletScript.bulletColor = bullletColor;
        BulletScript.bulletSpeed = baseSpeed * multiplicatorBulletSpeed;
        BulletScript.multiplicatorBulletScale = multiplicatorBulletScale;
        BulletScript.bulletDamage = baseDamage * multiplicatorDamage
;
        if (direction != 0) { BulletScript.bulletDirection = direction; } else { BulletScript.bulletDirection = 1; }
        BulletScript.bulletEnemy=isEnemy;
        BulletScript.multiplicatorBulletScale=multiplicatorBulletScale;

    }
}

