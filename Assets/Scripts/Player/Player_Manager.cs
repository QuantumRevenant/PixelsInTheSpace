using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

//Items Enumerator
public enum Items { Nothing, BlockedSpace, Shield, Laser, Bomb, Misil, Torpedo, DoubleShot, TripleShot, WaveShoot, Dron, Bengal, ExtraLife };
public enum MultiplicatorType { Speed, Scale, Damage, Reload, BulletSpeed, BulletScale }

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Player_Manager : MonoBehaviour
{
    #region - Variables
    [Header("Life")]
    [Range(0, 10)] public float playerLife = 5f;
    private bool isInvulnerable;
    private bool isShielded = false;
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
    public PlayerData playerData;
    [Space(10)]

    [Header("Inventory")]
    [SerializeField, Range(1, 5)] private int inventoryPosition = 1;
    [SerializeField] private int inventorySize = 5;
    [SerializeField] private List<Items> inventory;
    private bool isAvailableScrollInv = true;
    private const int inventoryStandardSize = 5;
    private const float scrollCooldownTimeInventory = 0.5f;

    [HideInInspector] public float finalSize;
    [HideInInspector] public PlayerInput playerInput;

    #endregion


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        InitializeInventory();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (trigger)
            Trigger();
        ChangeSize();
        ScrollActivable(playerInput.actions["Hotbar Scroll"].ReadValue<float>());
        ScrollSelect(playerInput.actions["Hotbar Select"].ReadValue<float>());
        UseItem(inventoryPosition,playerInput.actions["Trigger"].ReadValue<float>());
    }
    private void Trigger()
    {
        trigger = false;
        Damage(1);
    }
    #region TriggerEnter
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
            case "PickupItem":
                {
                    TouchedItem(collision);
                    break;
                }
        }
    }
    private void TouchedItem(Collider2D collision)
    {
        Items itemClass = collision.GetComponent<Item_Script>().itemData.ItemClass;
        if (itemClass == Items.Nothing || itemClass == Items.BlockedSpace)
        {
            Debug.Log("Error, El jugador Tocó un " + itemClass);
            return;
        }

        if (itemClass != Items.ExtraLife)
            SaveItem(itemClass, collision);
        else if (playerLife < 10)
        {
            playerLife++;
            Destroy(collision.gameObject);
        }

    }
    #endregion
    #region Damage
    [ContextMenu("Change Invulnerable Status")]
    public void ToggleInvulnerable()
    {
        isInvulnerable = !isInvulnerable;
        Debug.Log("Cambiamos el valor de Invulnerabilidad a" + isInvulnerable);
    }
    public void Damage(float damage, bool invulnerableCheck = true)
    {
        if (!isInvulnerable && !isShielded)
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
        if (isShielded)
        {
            ShieldChange(false);
        }

    }
    private void ShieldChange(bool TurnOn)
    {
        transform.GetChild(0).gameObject.SetActive(TurnOn);
        isShielded=TurnOn;
        if (TurnOn)
        {
            GetComponent<BoxCollider2D>().size = new Vector2(0.15f, 0.15f);
        }
        else
        {
            GetComponent<BoxCollider2D>().size = new Vector2(0.0875f, 0.0875f);
            isInvulnerable = true;
            invulnerableTimer = playerData.InvulnerableTime;
            StartCoroutine("blinking");
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
    private void Death()
    {
        gameObject.SetActive(false);
    }
    #endregion
    #region Inventory
    private void InitializeInventory()
    {
        for (int i = 0; i < inventoryStandardSize; i++)
        {
            if (i < inventorySize)
                inventory.Add(Items.Nothing);
            else
                inventory.Add(Items.BlockedSpace);
        }
    }
    IEnumerator cooldownScrollActivable()
    {
        isAvailableScrollInv = false;
        yield return new WaitForSeconds(scrollCooldownTimeInventory);
        isAvailableScrollInv = true;
    }
    private void ScrollSelect(float value)
    {
        if (value <= 0 || value > inventorySize)
            return;

        inventoryPosition = (int)value;

    }
    private void ScrollActivable(float value)
    {
        if (value == 0 || !isAvailableScrollInv)
            return;
        //Debug.Log(value);
        StartCoroutine("cooldownScrollActivable");
        if (value > 0)
        {
            if (inventoryPosition >= inventorySize)
                inventoryPosition = 1;
            else
                inventoryPosition += 1;
        }
        else
        {
            if (inventoryPosition <= 1)
                inventoryPosition = inventorySize;
            else
                inventoryPosition += -1;
        }
    }
    private void SaveItem(Items itemClass, Collider2D colission)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            if (inventory[i] == Items.Nothing)
            {
                inventory[i] = itemClass;
                i = inventorySize;
                Destroy(colission.gameObject);
            }
        }
    }
    private void UseItem(int pos, float value)
    {
        if (value < 1)
            return;
        if (inventory[pos - 1] == Items.Nothing || inventory[pos - 1] == Items.BlockedSpace)
            return;
        switch (inventory[pos - 1])
        {
            case Items.Shield:
                ShieldChange(true);
                break;
            case Items.Laser:
                //Chage Value to Laser
                break;
            case Items.Bomb:
                //Instantiate Bomb
                break;
            case Items.Misil:
                //Instantiate Misil
                break;
            case Items.Torpedo:
                //Instantiate Torpedo
                break;
            case Items.DoubleShot:
                //Change Value to Double Shot
                break;
            case Items.TripleShot:
                //Change Valuer to Triple Shot
                break;
            case Items.WaveShoot:
                //Change Value to Wave Shot
                break;
            case Items.Dron:
                //Generate Dron
                break;
            case Items.Bengal:
                //Instantiate Bengals
                break;
            case Items.ExtraLife:
                if (playerLife < 10)
                    playerLife++;
                break;
        }
        inventory[pos-1]=Items.Nothing;
    }
    #endregion

    public void ChangeSize()
    {
        float baseSize = playerData.PlayerSize * 10;
        float changedSize = baseSize * multiplicatorScale;
        gameObject.transform.localScale = new Vector3(changedSize, changedSize, changedSize);
        finalSize = changedSize / 10;
    }

}
