using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
[RequireComponent(typeof(Player_Manager))]
public class Player_Shooting : MonoBehaviour
{
    #region - Variables
    [Header("Player Stats")]
    private float reloadTimer;
    private bool isShooting = false;
    [Space(10)]

    [Header("Bullet Stats")]
    private Vector2 bulletVector;
    private int direction;
    [Space(10)]

    [Header("Player Modificators")]
    private float multiplicatorReload=1f;
    private float multiplicatorDamage=1f;
    [Space(10)]

    [Header("Bullet Modificators")]
    private float multiplicatorBulletSpeed=1f;
    private float multiplicatorBulletScale=1f;
    [Space(10)]

    [Header("Bullets GameObject")]
    [SerializeField] private GameObject SimpleBullet;

    //Functionality
    private PlayerController playerController;
    [SerializeField] private PlayerData playerData;
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
        playerData=gameObject.GetComponent<Player_Manager>().playerData;
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
        multiplicatorDamage = PlayerManager.multiplicatorDamage;
        bulletVector = PlayerManager.bulletVector;
        multiplicatorBulletScale = PlayerManager.multiplicatorBulletScale;
        multiplicatorBulletSpeed = PlayerManager.multiplicatorBulletSpeed;

        if(playerData.IsEnemy)
            direction=-1;
        else
            direction=1;
    }
    private void Shoot()
    {
        reloadTimer += -(Time.deltaTime);

        if (Reload())
        {
            reloadTimer = playerData.BaseReloadTime;
            Select();
        }

    }

    private bool Reload()
    {
        if (reloadTimer <= 0 && isShooting == true)         
            return true;
        else
            return false;
        
    }

    private void Select()
    {
        Generate();
    }
    private void Generate()
    {   
        GameObject inst=Instantiate(SimpleBullet,transform.position,Quaternion.identity);
        Bullet_Script bulletScript= inst.GetComponent<Bullet_Script>();

        bulletScript.bulletVector = bulletVector;
        bulletScript.bulletColor = playerData.BulletColor;
        bulletScript.bulletSpeed = playerData.BaseBulletSpeed * multiplicatorBulletSpeed;
        bulletScript.multiplicatorBulletScale = multiplicatorBulletScale;
        bulletScript.bulletDamage = playerData.BaseDamage * multiplicatorDamage
;
        if (direction != 0)
            bulletScript.bulletDirection = direction;
        else
            bulletScript.bulletDirection = 1;

        bulletScript.bulletEnemy=playerData.IsEnemy;
        bulletScript.multiplicatorBulletScale=multiplicatorBulletScale;

    }
}

