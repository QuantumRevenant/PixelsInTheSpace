using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
[RequireComponent(typeof(Player_Manager))]
public class Player_Shooting : MonoBehaviour
{
    [Header("Player Stats")]
    private float reloadTimer = 0;
    public ShootingType shootType = ShootingType.Simple;
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
    [SerializeField] private ShootingTableData shootData;
    [HideInInspector] public PlayerInput playerInput;
    [SerializeField] private PlayerData playerData;

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
        shootType = PlayerManager.shootingType;
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
            Select(shootData);
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
    private void Select(ShootingTableData shootingTable)
    {
        if (shootType == ShootingType.Laser)
        {
            LaserChange(true);
            return;
        }
        LaserChange(false);
        for (int i = 0; i < shootingTable.shootingData.Length; i++)
        {
            if (shootingTable.shootingData[i].shootingType == shootType)
            {
                ReadShootGroup(shootingTable.shootingData[i], shootType);
                i = shootingTable.shootingData.Length;
            }
        }
    }
    private void ReadShootGroup(ShootingData shootingData, ShootingType shootingType)
    {
        switch(shootingType)
        {
            case ShootingType.Simple:
                Generate(shootingData.bulletData[0], 0, Vector2.zero);
                break;
            case ShootingType.Lateral:
                ShootLateral(shootingData.bulletData[0], shootingData.BulletSeparation, shootingData.BulletNumber);
                break;
            case ShootingType.Arch:
                ShootArch(shootingData.bulletData[0], shootingData.AngleArch, shootingData.BulletNumber);
                break;
            case ShootingType.Wave: 
                ShootWave(shootingData);
                break;
            default:
                Debug.Log("Player_Shooting.cs, ReadShootGroup, shooting type no coincidente");
                break;
        }
    }
    private void ShootWave(ShootingData shootingData)
    {
        int value = shootingData.BulletNumber < shootingData.bulletData.Length ? shootingData.BulletNumber : shootingData.bulletData.Length;
        for (int i = 0; i < value; i++)
            Generate(shootingData.bulletData[i], 0, Vector2.zero);
    }
    private void ShootLateral(BulletData bData, float bulletSeparation, int bulletNumber)
    {
        float resultingDistance = 0f;
        float value = bulletNumber;

        for (int i = 0; i < bulletNumber; i++)
        {
            if (bulletNumber <= 1)
                value = 2;
            resultingDistance = Mathf.Lerp(-bulletSeparation / 2, bulletSeparation / 2, (float)i / (value - 1));
            Generate(bData, 0, new Vector2(resultingDistance, 0));
        }
    }
    private void ShootArch(BulletData bData, float angleArch, int bulletNumber)
    {
        float resultingAngle = 0f;
        float value = bulletNumber;

        if (angleArch >= 360)
        {
            for (int i = 0; i < bulletNumber; i++)
            {
                if (bulletNumber <= 1)
                    value = 2;
                resultingAngle = Mathf.Lerp(0, angleArch, (float)i / value);
                Generate(bData, resultingAngle, Vector2.zero);
            }
        }
        else
        {
            for (int i = 0; i < bulletNumber; i++)
            {
                if (bulletNumber <= 1)
                    value = 2;
                resultingAngle = Mathf.Lerp(-angleArch / 2, angleArch / 2, (float)i / (value - 1));
                Generate(bData, resultingAngle, Vector2.zero);
            }
        }
    }
    private void Generate(BulletData bulletData, float angle, Vector2 offsetPos)
    {
        Vector3 vec3OffsetPos = offsetPos;
        GameObject inst = Instantiate(SimpleBullet, transform.position + vec3OffsetPos, Quaternion.Euler(0f, 0f, angle));
        Bullet_Script bulletScript = inst.GetComponent<Bullet_Script>();
        bulletScript.initialAngle = angle;
        bulletScript.bulletData = bulletData;
        bulletScript.multiplicatorBulletScale = multiplicatorBulletScale;
        bulletScript.bulletDamage = playerData.BaseDamage * multiplicatorDamage;
        bulletScript.multiplicatorBulletScale = multiplicatorBulletScale;

    }
}