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
    public ShootingType shootingType = ShootingType.Simple;
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
    [SerializeField] private ShootingData shootData;
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
        shootingType = PlayerManager.shootingType;
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
        }
        else if (shootPressed == 0)
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
        if (shootingType == ShootingType.Laser)
        {
            LaserChange(true);
            return;
        }
        LaserChange(false);

        switch (shootingType)
        {
            default:
                ReadShootGroup(shootData);
                break;
        }

    }
    private void ReadShootGroup(ShootingData shootingData)
    {
        int size = shootingData.bulletData.Length;

        Debug.Log("Piu " + size);
        for (int i = 0; i < size; i++)
            Generate(shootingData.bulletData[i]);
    }
    private void Generate(BulletData bulletData)
    {
        GameObject inst = Instantiate(SimpleBullet, transform.position, Quaternion.identity);
        Bullet_Script bulletScript = inst.GetComponent<Bullet_Script>();

        bulletScript.bulletData = bulletData;
        bulletScript.multiplicatorBulletScale = multiplicatorBulletScale;
        bulletScript.bulletDamage = playerData.BaseDamage * multiplicatorDamage;
        bulletScript.multiplicatorBulletScale = multiplicatorBulletScale;

    }
}