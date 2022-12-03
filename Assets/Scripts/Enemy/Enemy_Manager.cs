using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Manager : MonoBehaviour
{
    #region - Variables
    [Header("Life")]
    [Range(0, 10)] public float enemyLife = 100f;
    public Color enemyColor;
    public Color enemyDamagedColor;
    [SerializeField] private float durationDamageColor;
    private bool onLaser=false;
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
        enemyColor = gameObject.GetComponent<SpriteRenderer>().color;
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
                    Damage(collision.GetComponent<Bullet_Script>().bulletDamage);
                    break;
                }

            case "Explosion":
                {
                    Damage(recievedMeleeDamage);
                    break;
                }

            case "AllyLaser":
                {

                    
                    Damage(0.01f, false);
                    break;
                }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Salió");
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
            else
            {

            }
        }


    }

    IEnumerator blinking()
    {
        //bool state = true;
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = enemyDamagedColor;
        yield return new WaitForSeconds(durationDamageColor);
        spriteRenderer.color = enemyColor;
    }
    #endregion

    private void Death()
    {
        Debug.Log("Me mori :(");
    }

    public void changeSize()
    {
        float baseSize = enemySize * 10;
        float changedSize = baseSize * multiplicatorScale;
        gameObject.transform.localScale = new Vector3(changedSize, changedSize, changedSize);
        finalSize = changedSize / 10;
    }
}
