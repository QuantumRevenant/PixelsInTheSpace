using UnityEngine;
[CreateAssetMenu(fileName = "New Player Data", menuName = "Scriptable Objects/Player Data")]
public class PlayerData : ScriptableObject
{
    [Header("Player Characteristics")]
    [SerializeField] private Color playerColor=Color.white;
    [SerializeField] private Color playerDamagedColor=new Color(0.5f,0.5f,0.5f,0.5f);
    [SerializeField] private float invulnerableTime=0.6f;
    [SerializeField] private float blinkTime=0.25f;
    [SerializeField] private float playerSize=0.1f;
    [SerializeField] private Vector2 playerBoundaries;
    [Space(10)]
    /////////////////////////////////////////////////////
    [Header("Player Movement")]
    [SerializeField] private float baseSpeed;
    [SerializeField] private Vector2 baseVector;
    [Space(10)]
    /////////////////////////////////////////////////////
    [Header("Player Shooting")]
    [SerializeField] private float baseReloadTime;
    [SerializeField] private float baseDamage;
    [Space(10)]
    /////////////////////////////////////////////////////
    [Header("Bullet Characteristics")]
    [SerializeField] private Color bulletColor;
    [SerializeField] private float baseBulletSpeed;
    [SerializeField] private bool isEnemy;


    public Color PlayerColor { get { return playerColor; } }
    public Color PlayerDamagedColor { get { return playerDamagedColor; } }
    public float InvulnerableTime { get { return invulnerableTime; } }
    public float BlinkTime { get { return blinkTime; } }
    public float PlayerSize { get { return playerSize; } }
    public Vector2 PlayerBoundaries { get { return playerBoundaries; } }
    public float BaseSpeed { get { return baseSpeed; } }
    public Vector2 BaseVector { get { return baseVector; } }
    public float BaseReloadTime { get { return baseReloadTime; } }
    public float BaseDamage { get { return baseDamage; } }
    public Color BulletColor { get { return bulletColor; } }
    public float BaseBulletSpeed { get { return baseBulletSpeed; } }
    public bool IsEnemy { get { return isEnemy; } }

}