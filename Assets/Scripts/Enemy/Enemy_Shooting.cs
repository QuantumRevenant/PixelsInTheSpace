using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shooting : MonoBehaviour
{   
    #region - Variables
    [Header("Enemy Stats")]
    [SerializeField] private float baseReloadTime;
    private float modifiedReloadTime=0.1f;
    [SerializeField] private float reloadTimer;
    [SerializeField, Range(0,1)] private float maxReloadOffsetPercent;
    [SerializeField] private float reloadOffset;
    [Space(10)]

    [Header("Bullet Stats")]
    private Vector2 bulletVector;
    private Color bulletColor;
    [SerializeField] private float baseSpeed;
    [SerializeField] private float baseDamage;
    [SerializeField] private bool isEnemy;
    [SerializeField, Range(-1, 1)] private int direction;
    [Space(10)]

    [Header("Player Modificators")]
    private float multiplicatorReload=1;
    private float multiplicatorDamage=1;
    [Space(10)]

    [Header("Bullet Modificators")]
    private float multiplicatorBulletSpeed=1;
    private float multiplicatorBulletScale=1;
    [Space(10)]

    [Header("Bullets GameObject")]
    [SerializeField] private GameObject SimpleBullet;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        reloadOffset=(float)System.Math.Round(Random.Range(0f,baseReloadTime*0.1f),2);
        RecalculateTimer();
        reloadTimer=modifiedReloadTime;
    }

    // Update is called once per frame
    void Update()
    {
        GetVariables();
        if(ValidateShoot())
            Select();
    }

    void RecalculateTimer()
    {
        modifiedReloadTime=(baseReloadTime-reloadOffset)*multiplicatorReload;
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
        bulletColor = EnemyManager.bulletColor;
        isEnemy = EnemyManager.bulletEnemy;

        if(isEnemy)
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

        bulletScript.bulletVector = bulletVector;
        bulletScript.bulletColor = bulletColor;
        bulletScript.bulletSpeed = baseSpeed * multiplicatorBulletSpeed;
        bulletScript.multiplicatorBulletScale = multiplicatorBulletScale;
        bulletScript.bulletDamage = baseDamage * multiplicatorDamage
;
        if (direction != 0)
            bulletScript.bulletDirection = direction;
        else
            bulletScript.bulletDirection = 1;
            
        bulletScript.bulletEnemy=isEnemy;
        bulletScript.multiplicatorBulletScale=multiplicatorBulletScale;
        enemyManager.isShooting=false;
    }



}
