using UnityEngine;
[CreateAssetMenu(fileName ="New Enemy Data", menuName ="Scriptable Objects/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Characteristics")]
    [SerializeField] private string enemyName="Enemy";
    [SerializeField] private float enemyBaseLife=100;
    [SerializeField] private Color enemyColor=Color.white;
    [SerializeField] private Color enemyDamagedColor=Color.red;
    [SerializeField] private float durationDamageColor=0.125f;
    [SerializeField] private bool invulnerableMelee=false;
    [SerializeField] private float recievedMeleeDamage=250;
    [SerializeField] private float enemySize=0.1f;
    [SerializeField] private float meleeDamage=1;
    [SerializeField] private Vector2 enemyBoundaries=new Vector2(1.125f,1);
    [Space(10)]
    /////////////////////////////////////////////////////
    [Header("Enemy Movement")]
    [SerializeField] private float baseSpeed=0.5f;
    [SerializeField] private Vector2 baseVector=new Vector2(1,0.125f);
    [Space(10)]
    /////////////////////////////////////////////////////
    [Header("Enemy Shooting")]
    [SerializeField] private float baseReloadTime=1;
    [SerializeField] private float baseDamage=1;
    [Space(10)]
    /////////////////////////////////////////////////////
    [Header("Bullet Characteristics")]
    [SerializeField] private Color bulletColor=new Color(1f,0f,0.3f,1);
    [SerializeField] private float baseBulletSpeed=1;
    [SerializeField] private float baseBulletScale=2.5f;
    [SerializeField] private bool isEnemy=true;

    public string EnemyName { get { return enemyName; } }
    public float EnemyBaseLife { get { return enemyBaseLife; } }
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
    public Vector3 BaseBulletScale { get { return new Vector3(baseBulletScale,baseBulletScale,baseBulletScale); } }
    public bool IsEnemy { get { return isEnemy; } }
}
