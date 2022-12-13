using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Enemy_Manager))]
public class Enemy_Shooting : MonoBehaviour
{   
    #region - Variables
    [Header("Enemy Stats")]
    private float modifiedReloadTime=0.1f;
    private float reloadTimer;
    [SerializeField, Range(0,1)] private float ReloadOffsetPercent;
    private float reloadOffset;
    [Space(10)]

    [Header("Bullet Stats")]
    private Vector2 bulletVector;
    private int direction;
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
    [SerializeField] private EnemyData enemyData;
    #endregion

    void Awake()
    {
        enemyData=gameObject.GetComponent<Enemy_Manager>().enemyData;
    }
    // Start is called before the first frame update
    void Start()
    {
        reloadOffset=(float)System.Math.Round(Random.Range(-enemyData.BaseReloadTime*ReloadOffsetPercent,enemyData.BaseReloadTime*ReloadOffsetPercent),2);
        RecalculateTimer();
        reloadTimer=modifiedReloadTime;
    }

    // Update is called once per frame
    void Update()
    {
        RecalculateTimer();
        GetVariables();
        if(ValidateShoot())
            Select();
    }

    void RecalculateTimer()
    {
        modifiedReloadTime=(enemyData.BaseReloadTime-reloadOffset)*multiplicatorReload;
    }

    private void GetVariables()
    {
        Enemy_Manager EnemyManager;
        EnemyManager = gameObject.GetComponent<Enemy_Manager>();
        multiplicatorReload = EnemyManager.multiplicatorReload;
        multiplicatorDamage = EnemyManager.multiplicatorDamage;
        bulletVector = EnemyManager.bulletVector;
        multiplicatorBulletScale = EnemyManager.multiplicatorBulletScale;
        multiplicatorBulletSpeed = EnemyManager.multiplicatorBulletSpeed;

        if(enemyData.IsEnemy)
            direction=-1;
        else
            direction=1;
    }

    bool ValidateShoot()
    {
        bool output=false;
        
        if(reloadTimer<=0)
        {
            output=true;
            reloadTimer=modifiedReloadTime;
        }
        else
            reloadTimer+=-Time.deltaTime;

        return output;
    }

    void Select()
    {
        Generate();
    }

    void Generate()
    {
        Enemy_Manager enemyManager=gameObject.GetComponent<Enemy_Manager>();
        enemyManager.isShooting=true;
        GameObject inst=Instantiate(SimpleBullet,transform.position,Quaternion.identity);
        Bullet_Script bulletScript= inst.GetComponent<Bullet_Script>();

        bulletScript.baseScale=enemyData.BaseBulletScale;
        bulletScript.bulletVector = bulletVector;
        bulletScript.bulletColor = enemyData.BulletColor;
        bulletScript.bulletSpeed = enemyData.BaseSpeed * multiplicatorBulletSpeed;
        bulletScript.multiplicatorBulletScale = multiplicatorBulletScale;
        bulletScript.bulletDamage = enemyData.BaseDamage * multiplicatorDamage;
        if (direction != 0)
            bulletScript.bulletDirection = direction;
        else
            bulletScript.bulletDirection = 1;
            
        bulletScript.bulletEnemy=enemyData.IsEnemy;
        bulletScript.multiplicatorBulletScale=multiplicatorBulletScale;
        enemyManager.isShooting=false;
    }



}
