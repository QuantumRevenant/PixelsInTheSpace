using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Player_Manager : MonoBehaviour
{
    #region - Variables
    [Header("Life")]
    [Range(0,10)] public float playerLife = 5f;
    public Color playerColor;
    public Color playerDamagedColor;
    [SerializeField] private bool isInvulnerable;
    [SerializeField] private float invulnerableTime;
    [SerializeField] private float blinkTime;
    [SerializeField] private bool trigger;
    [SerializeField] private float invulnerableTimer;
    [Space(10)]

    [Header("Player Multiplicators")]
    public float multiplicatorPlayerSpeed;
    public float multiplicatorPlayerScale;
    public float multiplicatorPlayerDamage;
    public float multiplicatorReload;
    [Space(10)]

    [Header("Bullet Multiplicators")]
    public float multiplicatorPlayerBulletSpeed;
    public float multiplicatorPlayerBulletScale;
    public Color playerBulletColor;
    public Vector2 playerBulletVector;
    public bool bEnemy = false;
    [Space(10)]

    [Header("Player Data")]
    public float playerSize;
    [HideInInspector] public float finalSize;
    public Vector2 playerBoundaries;
    //[Space(10)]
    #endregion

    private void Awake()
    {
        playerColor = gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (trigger == true)
        {
            trigger = false;
            Damage(1);            
        }
        changeSize();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                {
                    Damage(1); // Colocar Variable "Impact Damage"
                    break;
                }

            case "EnemyProjectile":
                {
                    Damage(collision.GetComponent<Bullet_Script>().bulletDamage);
                    break;
                }

            case "Explosion":
                {
                    Damage(1);
                    break;
                }

            case "EnemyLaser":
                {
                    Damage(0.01f,false);
                    break;
                }
        }
    }

    #region Damage
    public void Damage(float damage, bool invCheck=true)
    {
        if (isInvulnerable == false)
        {            
            playerLife += -damage;
            if (playerLife <= 0)
            {
                Death();
            }
            else
            {
                if (invCheck==true) 
                {
                    isInvulnerable = true;
                    invulnerableTimer = invulnerableTime;
                    StartCoroutine("blinking");
                }
            }
        }
        
    }

    IEnumerator blinking()
    {
        //bool state = true;
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        while (invulnerableTimer > 0)
        {
            spriteRenderer.color = playerDamagedColor;
            yield return new WaitForSeconds(blinkTime);
            spriteRenderer.color = playerColor;
            yield return new WaitForSeconds(blinkTime);
            invulnerableTimer += (-blinkTime * 2);            
        }
        isInvulnerable = false;
    }
    #endregion

    private void Death()
    {
        Debug.Log("Me mori :(");
    }

    public void changeSize()
    {
        float baseSize = playerSize * 10;
        float changedSize = baseSize * multiplicatorPlayerScale;
        gameObject.transform.localScale = new Vector3(changedSize, changedSize, changedSize);
        finalSize = changedSize/10;
    }
}
