using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    #region - Variables
    [Header("Life")]
    public float enemyLife = 100f;
    public Color enemyColor;
    public Color enemyDamagedColor;
    [SerializeField] private float durationDamageColor;
    private bool onLaser=false;
    public bool isShooting=true;
    private float damageLaser;
    [SerializeField] private bool invulnerableMelee;
    [SerializeField] private float recievedMeleeDamage = 250f;
    [SerializeField] private bool trigger;
    [Space(10)]

    [Header("Enemy Multiplicators")]
    public float multiplicatorSpeed;
    public float multiplicatorScale;
    public float multiplicatorDamage;
    public float multiplicatorReload;
    [Space(10)]

    [Header("Enemy Multiplicators")]
    public float multiplicatorBulletSpeed;
    public float multiplicatorBulletScale;
    public Color bulletColor;
    public Vector2 bulletVector;
    public bool bulletEnemy = true;
    [Space(10)]

    [Header("Enemy Data")]
    public float enemySize;

    public float meleeDamage;
    [HideInInspector] public float finalSize;
    public Vector2 enemyBoundaries;
    //[Space(10)]
    #endregion


    private void Awake()
    {
        enemyColor = gameObject.GetComponent<SpriteRenderer>().material.color;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (trigger)
        {
            trigger = false;
            Damage(250);
        }
        changeSize();

        if(onLaser)
        {
            gameObject.GetComponent<SpriteRenderer>().color=enemyDamagedColor;
            Damage(damageLaser,false);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color=enemyColor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                {
                    if (!invulnerableMelee)
                        Damage(recievedMeleeDamage); // Colocar Variable "Impact Damage"
                    break;
                }

            case "AllyProjectile":
                {
                    if(!isShooting)
                    {
                    Damage(collision.GetComponent<Bullet_Script>().bulletDamage);
                    Debug.Log("Ouch, No me dispares PF");
                    }
                    break;     
                }

            case "Explosion":
                {
                    Damage(recievedMeleeDamage);
                    break;
                }

            case "AllyLaser":
                {
                    damageLaser=1f;
                    // damageLaser=collision.GetComponent<>().laserDamage;
                    damageLaser=damageLaser*Time.deltaTime;
                    onLaser=true;
                    break;
                }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        onLaser=false;
    }

    #region Damage
    public void Damage(float damage, bool ticking = true)
    {

        enemyLife += -damage;
        if (enemyLife <= 0)
        {
            Death();
        }
        else
        {
            if (ticking)
            {
                StartCoroutine("blinking");
            }
        }


    }

    IEnumerator blinking()
    {
        //bool state = true;
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.material.color = enemyDamagedColor;
        yield return new WaitForSeconds(durationDamageColor);
        spriteRenderer.material.color = enemyColor;
    }
    #endregion

    private void Death()
    {
        Debug.Log("Me mori :( - Enemigo "+ System.DateTime.Now.Year.ToString() );
        Destroy(this.gameObject);
    }

    public void changeSize()
    {
        float baseSize = enemySize * 10;
        float changedSize = baseSize * multiplicatorScale;
        gameObject.transform.localScale = new Vector3(changedSize, changedSize, changedSize);
        finalSize = changedSize / 10;
    }
}
