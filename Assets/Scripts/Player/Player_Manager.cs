using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Items Enumerator
enum Items{Nothing,Shield,Laser,Bomb};

[RequireComponent(typeof(Rigidbody2D),typeof(BoxCollider2D))]
public class Player_Manager : MonoBehaviour
{
    #region - Variables
    [Header("Life")]
    [Range(0, 10)] public float playerLife = 5f;
    private bool isInvulnerable;
    [SerializeField] private bool trigger;
    private float invulnerableTimer;
    [Space(10)]

    [Header("Player Multiplicators")]
    [HideInInspector] public float multiplicatorSpeed = 1f;
    [HideInInspector] public float multiplicatorScale = 1f;
    [HideInInspector] public float multiplicatorDamage = 1f;
    [HideInInspector] public float multiplicatorReload = 1f;
    [Space(10)]

    [Header("Bullet Multiplicators")]
    [HideInInspector] public float multiplicatorBulletSpeed = 1f;
    [HideInInspector] public float multiplicatorBulletScale = 1f;
    [HideInInspector] public Vector2 bulletVector = new Vector2(0f, 1f);
    [Space(10)]

    [Header("Inventory")]

    [HideInInspector] public float finalSize;
    public PlayerData playerData;
    #endregion


    private void Awake()
    {
        //playerColor = gameObject.GetComponent<SpriteRenderer>().color;
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
                    Damage(1);
                    break;
                }
        }
    }

    #region Damage
    [ContextMenu("Change Invulnerable Status")]
    public void ToggleInvulnerable()
    {
        isInvulnerable=!isInvulnerable;
        Debug.Log("Cambiamos el valor de Invulnerabilidad a" + isInvulnerable);
    }
    public void Damage(float damage, bool invulnerableCheck = true)
    {
        if (!isInvulnerable)
        {
            playerLife += -damage;
            if (playerLife <= 0)
            {
                Death();
            }
            else
            {
                if (invulnerableCheck)
                {
                    isInvulnerable = true;
                    invulnerableTimer = playerData.InvulnerableTime;
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
            spriteRenderer.color = playerData.PlayerDamagedColor;
            yield return new WaitForSeconds(playerData.BlinkTime);
            spriteRenderer.color = playerData.PlayerColor;
            yield return new WaitForSeconds(playerData.BlinkTime);
            invulnerableTimer += (-playerData.BlinkTime * 2);
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
        float baseSize = playerData.PlayerSize * 10;
        float changedSize = baseSize * multiplicatorScale;
        gameObject.transform.localScale = new Vector3(changedSize, changedSize, changedSize);
        finalSize = changedSize / 10;
    }
}
