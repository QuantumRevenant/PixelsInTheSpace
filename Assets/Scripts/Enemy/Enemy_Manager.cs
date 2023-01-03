using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Enemy_Manager : MonoBehaviour
{
    [Header("Life")]
    public float enemyLife = 100f;
    private bool onLaser = false;
    [HideInInspector] public bool isShooting = true;
    private float damageLaser;
    [SerializeField] private bool trigger;
    [Space(10)]
    [Header("Enemy Multiplicators")]
    [HideInInspector] public float multiplicatorSpeed = 1f;
    [HideInInspector] public float multiplicatorScale = 1f;
    [HideInInspector] public float multiplicatorDamage = 1f;
    [HideInInspector] public float multiplicatorReload = 1f;
    [Space(10)]
    [Header("Enemy Multiplicators")]
    [HideInInspector] public float multiplicatorBulletSpeed = 1f;
    [HideInInspector] public float multiplicatorBulletScale = 1f;
    [HideInInspector] public Vector2 bulletVector = new Vector2(0, 1f);
    [Space(10)]
    [HideInInspector] public float finalSize;
    public EnemyData enemyData;

    private void Awake()
    {
        enemyLife = enemyData.EnemyBaseLife;
    }
    void Update()
    {
        if (trigger)
        {
            trigger = false;
            Damage(250);
        }
        ChangeSize();
        if (onLaser)
        {
            gameObject.GetComponent<SpriteRenderer>().color = enemyData.EnemyDamagedColor;
            Damage(damageLaser, false);
        }
        else
            gameObject.GetComponent<SpriteRenderer>().color = enemyData.EnemyColor;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                {
                    if (!enemyData.InvulnerableMelee)
                        Damage(enemyData.RecievedMeleeDamage); // Colocar Variable "Impact Damage"
                    break;
                }
            case "AllyProjectile":
                {
                    if (!isShooting)
                    {
                        Damage(collision.GetComponent<Bullet_Script>().bulletDamage);
                        Debug.Log("Ouch, No me dispares PF");
                    }
                    break;
                }
            case "Explosion":
                {
                    Damage(enemyData.RecievedMeleeDamage);
                    break;
                }
            case "AllyLaser":
                {
                    damageLaser = 1f;
                    // damageLaser=collision.GetComponent<>().laserDamage;
                    damageLaser = damageLaser * Time.deltaTime;
                    onLaser = true;
                    break;
                }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        onLaser = false;
    }
    
    public void Damage(float damage, bool ticking = true)
    {
        enemyLife += -damage;
        if (enemyLife <= 0)
            Death();
        else
        {
            if (ticking)
                StartCoroutine("blinking");
        }
    }
    private void Death()
    {
        // Debug.Log("Me mori :( - Enemigo " + System.DateTime.Now.Year.ToString());
        Destroy(this.gameObject);
    }
    public void ChangeSize()
    {
        float baseSize = enemyData.EnemySize * 10;
        float changedSize = baseSize * multiplicatorScale;
        gameObject.transform.localScale = new Vector3(changedSize, changedSize, changedSize);
        finalSize = changedSize / 10;
    }
    IEnumerator blinking()
    {
        //bool state = true;
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material.color = enemyData.EnemyDamagedColor;
        yield return new WaitForSeconds(enemyData.DurationDamageColor);
        spriteRenderer.material.color = enemyData.EnemyColor;
    }
}
