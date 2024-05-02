using UnityEngine;
using QuantumRevenant.PixelsinTheSpace;
using MyBox;

[CreateAssetMenu(fileName = "New Bullet Data", menuName = "Scriptable Objects/Bullet_Data")]
public class ScO_Bullet : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private Color color = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private float speed;
    [SerializeField] private float angularSpeed;
    [SerializeField][Range(0, 360)] private float angle;
    [SerializeField] private float scale;
    [SerializeField] private float lifeTime;
    [SerializeField] private ScO_Gizmos gizmosData;

    [Header("Post Mortem")]
    [SerializeField] private PostMortemBulletAction postMortem;

    [ConditionalField(true, nameof(isExplosive))]
    [SerializeField] private float aoeRadius;
    [ConditionalField(true, nameof(isPiercing))]
    [SerializeField] private int pierceCounter;
    [ConditionalField(true, nameof(isAlterer))]
    [SerializeField] private string stats;

    private bool isSummoner() => postMortem.HasFlag(PostMortemBulletAction.Summon);
    private bool isExplosive() => postMortem.HasFlag(PostMortemBulletAction.Explode);
    private bool isPiercing() => postMortem.HasFlag(PostMortemBulletAction.Pierce);
    private bool isAlterer() => postMortem.HasFlag(PostMortemBulletAction.Alter);

    //Cartridge Integration

    [ConditionalField(true, nameof(isSummoner))]
    [SerializeField] private CollectionWrapper<ScO_Bullet> subprojectile;
    [ConditionalField(true, nameof(isSummoner))]
    [SerializeField] private float spacing;
    [ConditionalField(true, nameof(isSummoner))]
    [SerializeField][Range(0, 360)] private float firingArc;
    [ConditionalField(true, nameof(isSummoner))]
    [SerializeField] private int subprojectileQuantity;
    [ConditionalField(true, nameof(isSummoner))]
    [SerializeField] private float angularOffset;
    [ConditionalField(true, nameof(isSummoner))]
    [SerializeField] private bool inheritType;
    [ConditionalField(true, nameof(isSummoner))]
    [SerializeField] private bool inheritDamage;

    private bool inheritTypeBool() => isSummoner() && inheritType;
    private bool notInheritTypeBool() => isSummoner() && !inheritType;
    private bool notInheritDamageBool() => isSummoner() && !inheritDamage;

    [ConditionalField(true, nameof(inheritTypeBool))]
    [SerializeField] private bool inheritAllDamage;
    [ConditionalField(true, nameof(notInheritTypeBool))]
    [SerializeField] private DamageTypes subprojectileType;
    [ConditionalField(true, nameof(notInheritDamageBool))]
    [SerializeField] private float subprojectileDamage;

    public PostMortemBulletAction PostMortem { get => postMortem; set => postMortem = value; }
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public float Speed { get => speed; set => speed = value; }
    public float Scale { get => scale; set => scale = value; }
    public float AngularSpeed { get => angularSpeed; set => angularSpeed = value; }
    public float Angle { get => angle; set => angle = value; }
    public float LifeTime { get => lifeTime; set => lifeTime = value; }
    public Color Color { get => color; set => color = value; }
    public float AoeRadius { get => aoeRadius; set => aoeRadius = value; }

    public ScO_Bullet[] Subprojectile { get => subprojectile.Value; set => subprojectile.Value = value; }
    public int AvailableSubprojectiles { get => subprojectile.Value.Length; }
    public float Spacing { get => spacing; set => spacing = value; }

    public float FiringArc { get => firingArc; set => firingArc = value; }
    public int SubprojectileQuantity { get => subprojectileQuantity; set => subprojectileQuantity = value; }
    public float AngularOffset { get => angularOffset; set => angularOffset = value; }
    public int PierceCounter { get => pierceCounter; set => pierceCounter = value; }
    public bool InheritDamage { get => inheritDamage; set => inheritDamage = value; }
    public bool InheritType { get => inheritType; set => inheritType = value; }
    public bool InheritAllDamage { get => inheritAllDamage; set => inheritAllDamage = value; }
    public DamageTypes SubprojectileType { get => subprojectileType; set => subprojectileType = value; }
    public float SubprojectileDamage { get => subprojectileDamage; set => subprojectileDamage = value; }
    public ScO_Gizmos GizmosData { get => gizmosData; set => gizmosData = value; }
}