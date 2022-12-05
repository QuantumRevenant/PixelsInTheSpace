using UnityEngine;
[CreateAssetMenu(fileName ="New Enemy Data", menuName ="Scriptable Objects/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Characteristics")]
    [SerializeField] private string enemyName="Enemy";
    [SerializeField] private Color enemyColor=Color.white;
    [SerializeField] private Color enemyDamagedColor=new Color(1f,0f,0.3f,1);
    [SerializeField] private float durationDamageColor;
    [SerializeField] private bool invulnerableMelee;
    [SerializeField] private float recievedMeleeDamage;
    [SerializeField] private float enemySize;
    [SerializeField] private float meleeDamage;
    [SerializeField] private Vector2 enemyBoundaries;
    [Space(10)]
    /////////////////////////////////////////////////////
    [Header("Enemy Movement")]
    [SerializeField] private float baseSpeed;
    [SerializeField] private Vector2 baseVector;
    [Space(10)]
    /////////////////////////////////////////////////////
    [Header("Enemy Shooting")]
    [SerializeField] private float baseReloadTime;
    [SerializeField] private float baseDamage;
    [Space(10)]
    /////////////////////////////////////////////////////
    [Header("Bullet Characteristics")]
    [SerializeField] private Color bulletColor;
    [SerializeField] private float baseBulletSpeed;
    [SerializeField] private bool isEnemy;

    public string EnemyName { get {return enemyName; } }
    public Color EnemyColor { get { return enemyColor; } }
    public Color EnemyDamagedColor { get { return enemyDamagedColor; } }
    public float DurationDamageColor { get { return durationDamageColor; } }
    public bool InvulnerableMelee { get { return invulnerableMelee; } }
    public float RecievedMeleeDamage { get { return recievedMeleeDamage; } }
    public float EnemySize { get { return enemySize; } }
    public Vector2 EnemyBoundaries { get { return enemyBoundaries; } }
    public float BaseSpeed { get { return baseSpeed; } }
    public Vector2 BaseVector { get { return baseVector; } }
    public float BaseReloadTime { get { return baseReloadTime; } }
    public float BaseDamage { get { return baseDamage; } }
    public Color BulletColor { get { return bulletColor; } }
    public float BaseBulletSpeed { get { return baseBulletSpeed; } }
    public bool IsEnemy { get { return isEnemy; } }
}
