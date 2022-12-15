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
    private float reloadTimer = 0;
    private bool isShooting = false;
    public ShootingType shootingType=ShootingType.Simple;
    private GameObject Laser;
    [Space(10)]

    [Header("Bullet Stats")]
    private Vector2 bulletVector;
    private int direction;
    [Space(10)]

    [Header("Player Modificators")]
    private float multiplicatorReload = 1f;
    private float multiplicatorDamage = 1f;
    [Space(10)]

    [Header("Bullet Modificators")]
    private float multiplicatorBulletSpeed = 1f;
    private float multiplicatorBulletScale = 1f;
    [Space(10)]

    [Header("Bullets GameObject")]
    [SerializeField] private GameObject SimpleBullet;
    [HideInInspector] public PlayerInput playerInput;

    //Functionality
    private PlayerController playerController;
    [SerializeField] private PlayerData playerData;
    #endregion
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerData = gameObject.GetComponent<Player_Manager>().playerData;
    }
    void Start()
    {
        Laser = gameObject.GetComponent<Player_Manager>().GetChildWithName(gameObject, "Laser");
    }
    void Update()
    {
        GetVariables();
        Shoot(playerInput.actions["Shoot"].ReadValue<float>());
    }

    private void GetVariables()
    {
        Player_Manager PlayerManager;
        PlayerManager = gameObject.GetComponent<Player_Manager>();
        shootingType=PlayerManager.shootingType;
        multiplicatorReload = PlayerManager.multiplicatorReload;
        multiplicatorDamage = PlayerManager.multiplicatorDamage;
        bulletVector = PlayerManager.bulletVector;
        multiplicatorBulletScale = PlayerManager.multiplicatorBulletScale;
        multiplicatorBulletSpeed = PlayerManager.multiplicatorBulletSpeed;

        if (playerData.IsEnemy)
            direction = -1;
        else
            direction = 1;
    }
    private void Shoot(float shootPressed)
    {
        reloadTimer += -(Time.deltaTime);

        if (reloadTimer <= 0 && shootPressed > 0)
        {
            reloadTimer = playerData.BaseReloadTime;
            Select();
        }else if(shootPressed==0)
        {
            LaserChange(false);
        }

    }
    private void LaserChange(bool TurnOn)
    {
        Laser.SetActive(TurnOn);
    }
    private void Select()
    {
        if(shootingType==ShootingType.Laser)
            LaserChange(true);
        else
        {
        LaserChange(false);
        Generate();
        }
    }
    private void Generate()
    {
        GameObject inst = Instantiate(SimpleBullet, transform.position, Quaternion.identity);
        Bullet_Script bulletScript = inst.GetComponent<Bullet_Script>();

        bulletScript.baseScale = playerData.BaseBulletScale;
        bulletScript.bulletDesviation = playerData.BaseBulletDesviation;
        bulletScript.bulletVector = bulletVector;
        bulletScript.bulletColor = playerData.BulletColor;
        bulletScript.bulletSpeed = playerData.BaseBulletSpeed * multiplicatorBulletSpeed;
        bulletScript.multiplicatorBulletScale = multiplicatorBulletScale;
        bulletScript.bulletDamage = playerData.BaseDamage * multiplicatorDamage;
        if (direction != 0)
            bulletScript.bulletDirection = direction;
        else
            bulletScript.bulletDirection = 1;

        bulletScript.bulletEnemy = playerData.IsEnemy;
        bulletScript.multiplicatorBulletScale = multiplicatorBulletScale;

    }
}