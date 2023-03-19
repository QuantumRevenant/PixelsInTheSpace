using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class Player_Scr : MonoBehaviour
{
    [Header("Life")]
    [Range(0, 10)] private float _healthPoints = 5f;
    private float _invulnerableTimer;
    private bool _isInvulnerable = false, _isShielded = false;
    private GameObject _shield;
    [Space(10)]

    [Header("Shooting")]
    private float _reloadTimer;
    private ShootingTypes _shootingType = ShootingTypes.Simple;
    private GameObject _laser;
    // [Space(10)]

    // [Header("Motion")]

    // [Space(10)]

    // [Header("Management")]
    private int score = 10;
    public int Score { get { return score; } }

    [Header("Inventory")]
    [Range(1, 5)] private int _inventoryPosition = 1;
    private int _inventorySize = 5;
    private List<Items> _inventory;
    private bool _canScroll;
    private const int _BaseInvSize = 5;
    private const float _ScrollCooldown = 0.5f;


    [SerializeField] private GameObject SimpleBullet;
    [Header("Data Tables")]
    [SerializeField] private ShootingTableData shootData;
    [SerializeField] private PlayerData playerData;
    private Rigidbody2D rb;
    [HideInInspector] public PlayerInput playerInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        _shield = GetChildWithName(gameObject, "Shield");
        InitializeInventory();
    }
    void Start()
    {
    }
    private void Update()
    {
        Shoot(playerInput.actions["Shoot"].ReadValue<float>());
    }
    private void FixedUpdate()
    {
        Move(playerInput.actions["Move"].ReadValue<Vector2>());
    }

    #region Management
    public GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
            return childTrans.gameObject;
        else
            return null;
    }
    public void AddPoints(int points)
    {
        score += points;
    }
    #endregion

    #region Life
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Enemy":
                Damage(1); // Colocar Variable "Impact Damage"
                break;
            case "EnemyProjectile":
                Damage(collision.GetComponent<Bullet_Script>().bulletDamage);
                break;
            case "Explosion":
                Damage(1);
                break;
            case "EnemyLaser":
                Damage(1);
                break;
            case "PickupItem":
                TouchedItem(collision);
                break;
        }
    }
    public void Damage(float damage, bool invulnerableCheck = true)
    {
        if (_isInvulnerable)
            return;
        if (_isShielded)
        {
            ShieldChange(false);
            return;
        }
        _healthPoints += -damage;
        _healthPoints = Mathf.Clamp(_healthPoints, 0, 10);
        if (invulnerableCheck)
        {
            _isInvulnerable = true;
            _invulnerableTimer = playerData.InvulnerableTime;
            StartCoroutine("blinking");
        }
        if (_healthPoints <= 0)
            Death();
    }
    private void Death()
    {
        Debug.Log("GG, GAME OVER!");
        gameObject.SetActive(false);
    }
    private void ShieldChange(bool TurnOn)
    {
        _shield.SetActive(TurnOn);
        if (TurnOn)
            GetComponent<BoxCollider2D>().size = new Vector2(0.15f, 0.15f);
        else
        {
            GetComponent<BoxCollider2D>().size = new Vector2(0.0875f, 0.0875f);
            _isInvulnerable = true;
            _invulnerableTimer = playerData.InvulnerableTime;
            StartCoroutine("blinking");
        }
    }
    #endregion

    #region Shooting
    private void Shoot(float shootPressed)
    {
        _reloadTimer += -(Time.deltaTime);

        if (_reloadTimer <= 0 && shootPressed > 0)
        {
            _reloadTimer = playerData.BaseReloadTime;
        }
        else if (shootPressed == 0)
        {
            _laser.SetActive(false);
        }

    }
    private void Select(ShootingTableData shootingTable)
    {
        if (_shootingType == ShootingTypes.Laser)
        {
            _laser.SetActive(true);
            return;
        }
        _laser.SetActive(false);
        for (int i = 0; i < shootingTable.shootingData.Length; i++)
        {
            if (shootingTable.shootingData[i].shootingType == _shootingType)
            {
                ReadShootGroup(shootingTable.shootingData[i], _shootingType);
                i = shootingTable.shootingData.Length;
            }
        }
    }
    private void ReadShootGroup(ShootingData shootingData, ShootingTypes shootingType)
    {
        switch (shootingType)
        {
            case ShootingTypes.Simple:
                Generate(shootingData.bulletData[0], 0, Vector2.zero);
                break;
            case ShootingTypes.Lateral:
                ShootLateral(shootingData.bulletData[0], shootingData.BulletSeparation, shootingData.BulletNumber);
                break;
            case ShootingTypes.Arch:
                ShootArch(shootingData.bulletData[0], shootingData.AngleArch, shootingData.BulletNumber);
                break;
            case ShootingTypes.Wave:
                ShootWave(shootingData);
                break;
            default:
                Debug.Log("Player_Shooting.cs, ReadShootGroup, shooting type no coincidente");
                break;
        }
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
    private void ShootWave(ShootingData shootingData)
    {
        int value = shootingData.BulletNumber < shootingData.bulletData.Length ? shootingData.BulletNumber : shootingData.bulletData.Length;
        for (int i = 0; i < value; i++)
            Generate(shootingData.bulletData[i], 0, Vector2.zero);
    }
    private void Generate(BulletData bulletData, float angle, Vector2 offsetPos)
    {
        Vector3 vec3OffsetPos = offsetPos;
        GameObject inst = Instantiate(SimpleBullet, transform.position + vec3OffsetPos, Quaternion.Euler(0f, 0f, angle));
        Bullet_Script bulletScript = inst.GetComponent<Bullet_Script>();
        bulletScript.initialAngle = angle;
        bulletScript.bulletData = bulletData;
        bulletScript.multiplicatorBulletScale = 1;              //add multiplicators
        bulletScript.bulletDamage = playerData.BaseDamage * 1;  //add multiplicators
        bulletScript.multiplicatorBulletScale = 1;              //add multiplicators
    }
    #endregion

    #region Motion
    private void Boundaries()
    {
        Vector3 PosAndSize;
        PosAndSize.x = transform.position.x;
        PosAndSize.y = transform.position.y;
        PosAndSize.z = playerData.PlayerSize;

        PosAndSize.x = Mathf.Clamp(PosAndSize.x, -playerData.PlayerBoundaries.x + (PosAndSize.z / 2), playerData.PlayerBoundaries.x - (PosAndSize.z / 2));
        PosAndSize.y = Mathf.Clamp(PosAndSize.y, -playerData.PlayerBoundaries.y, playerData.PlayerBoundaries.y - (PosAndSize.z / 2));

        PosAndSize.z = transform.position.z;
        transform.position = PosAndSize;
    }
    public void Move(Vector2 inputVector)
    {
        Vector3 vectorInicial = new Vector3(inputVector.x * playerData.BaseVector.x, inputVector.y * playerData.BaseVector.y, 0);
        Vector3 vectorFinal = vectorInicial * playerData.BaseSpeed * Time.deltaTime;
        transform.Translate(vectorFinal);
        Boundaries();
    }
    #endregion

    #region Inventory
    private void InitializeInventory()
    {
        for (int i = 0; i < _BaseInvSize; i++)
        {
            if (i < _inventorySize)
                _inventory.Add(Items.Nothing);
            else
                _inventory.Add(Items.BlockedSpace);
        }
    }
    private void TouchedItem(Collider2D collision)
    {
        Items itemClass = collision.GetComponent<Item_Script>().itemData.ItemClass;
        if (itemClass == Items.Nothing || itemClass == Items.BlockedSpace)
        {
            Debug.Log("Error, El jugador tocó un " + itemClass);
            return;
        }
        if (itemClass == Items.ExtraLife && _healthPoints < 10)
        {
            _healthPoints++;
            _healthPoints = Mathf.Clamp(_healthPoints, 0, 10);
            Destroy(collision.gameObject);
            return;
        }
        for (int i = 0; i < _inventorySize; i++)
        {
            if (_inventory[i] == Items.Nothing)
            {
                _inventory[i] = itemClass;
                i = _inventorySize;
                Destroy(collision.gameObject);
            }
        }
    }
    private void UseItem(int pos, float value)
    {
        if (value < 1)
            return;
        if (_inventory[pos - 1] == Items.Nothing || _inventory[pos - 1] == Items.BlockedSpace)
            return;
        switch (_inventory[pos - 1])
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
                if (_healthPoints < 10)
                    _healthPoints++;
                _healthPoints = Mathf.Clamp(_healthPoints, 0, 10);
                break;
        }
        _inventory[pos - 1] = Items.Nothing;
    }
    private void InventorySelectPosition(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.ReadValue<int>() <= 0 || callbackContext.ReadValue<int>() > _inventorySize)
            return;
        if (callbackContext.performed)
            _inventoryPosition = callbackContext.ReadValue<int>();
    }
    private void InventoryScrollPosition(float value)
    {
        if (value == 0 || !_canScroll)
            return;
        StartCoroutine("ScrollInvCooldown");
        if (value > 0)
            if (_inventoryPosition >= _inventorySize) _inventoryPosition = 1; else _inventoryPosition++;
        else
            if (_inventoryPosition <= 1) _inventoryPosition = _inventorySize; else _inventoryPosition--;
    }
    IEnumerator ScrollInvCooldown()
    {
        _canScroll = false;
        yield return new WaitForSeconds(_ScrollCooldown);
        _canScroll = true;
    }
    #endregion

    #region Variables
    public void ChangeScale()
    {
        float baseSize = playerData.PlayerSize * 10;
        float changedSize = baseSize * 1;//add multiplicators
        gameObject.transform.localScale = new Vector3(changedSize, changedSize, changedSize);
    }
    #endregion
}