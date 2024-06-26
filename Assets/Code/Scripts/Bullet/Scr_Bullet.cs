using UnityEngine;
using QuantumRevenant.Utilities;
using QuantumRevenant.GeneralNS;
using QuantumRevenant.PixelsinTheSpace;
using QuantumRevenant.Timer;
using UnityEditor;
using MyBox;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(SpriteRenderer))]
public class Scr_Bullet : MonoBehaviour
{
    [InitializationField]
    [SerializeField] private ScO_Bullet bulletData;
    private FunctionTimer timerActive;
    [HideInInspector]
    public float inheritedAngle = 0;
    [ReadOnly]
    [SerializeField] private DamageTypes type = DamageTypes.Neutral;
    [ReadOnly]
    [SerializeField] private float damage;
    [ReadOnly]
    [SerializeField] private int pierceCounter = 0;
    public ScO_Bullet BulletData
    {
        get { return bulletData; }
        set
        {
            bulletData = value;
            UpdateProperties();
            timerActive.setPause(false);
        }
    }
    public DamageTypes Type
    {
        get { return type; }
        set { type = value; }
    }
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }



    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        timerActive = FunctionTimer.Create(Death, float.PositiveInfinity, "BulletDeath_PermanentTimer", true);
        timerActive.setPause(true);
    }
    void Start() { UpdateProperties(); }
    void FixedUpdate()
    {
        if (BulletData != null)
            Movement();
    }
    private void OnEnable() { }
    private void OnDisable() { ResetProperties(); }
    private void OnDestroy() { if (!gameObject.scene.isLoaded) return; timerActive?.NoObjectionDestroySelf(); }

    #region BulletProperties
    [ContextMenu("updateProperties")]
    private void UpdateProperties()
    {
        if (BulletData == null)
            return;

        SetProperties(
            bulletData.Sprite,
            bulletData.Color,
            bulletData.Angle + inheritedAngle,
            new Vector3(bulletData.Scale, bulletData.Scale, bulletData.Scale),
            bulletData.LifeTime, bulletData.PierceCounter);
        timerActive.setPause(false);
    }

    [ContextMenu("resetProperties")]
    public void ResetProperties()
    {
        if (IsReset())
            return;

        bulletData = null;
        CleanProperties();
    }

    [ContextMenu("cleanProperties")]
    private void CleanProperties()
    {
        if (IsClean())
            return;
        inheritedAngle = 0;
        SetProperties(
            null,
            new Color(),
            0,
            new Vector3(1, 1, 1),
            float.PositiveInfinity, Tags.NeutralTeam);
    }
    private void SetProperties(Sprite sprite, Color color, float angle, Vector3 scale, float timer, int cPierce = 0)
    {
        SetProperties(sprite, color, angle, scale, timer, gameObject.tag, cPierce);
    }
    private void SetProperties(Sprite sprite, Color color, float angle, Vector3 scale, float timer, string stag, int cPierce = 0)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
        transform.eulerAngles = new Vector3(0, 0, angle);
        transform.localScale = scale;
        timerActive.changeTime(timer);
        gameObject.tag = stag;
        pierceCounter = cPierce;
        ResizeBulletCollider2D();
    }

    public void ResizeBulletCollider2D()
    {
        float resizeFactor;

        if (tag == Tags.NeutralTeam || tag == Tags.PlayerTeam)
            resizeFactor = 1f;
        else
            resizeFactor = 0.5f;

        boxCollider2D = Utility.ResizeBoxCollider2D(spriteRenderer, boxCollider2D, transform.localScale, resizeFactor);
    }


    private bool IsReset() { return IsClean() && BulletData == null; }
    private bool IsClean()
    {
        return spriteRenderer.sprite == null
        && spriteRenderer.color == new Color()
        && transform.rotation == Quaternion.Euler(0, 0, 0)
        && transform.localScale == new Vector3(1, 1, 1)
        && timerActive.getTimer() == float.PositiveInfinity && inheritedAngle == 0;
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (bulletData == null || bulletData.GizmosData == null)
            return;
        if (bulletData.PostMortem.HasFlag(PostMortemBulletAction.Explode))
            DrawExplosionAreaGizmos();

        if (bulletData.PostMortem.HasFlag(PostMortemBulletAction.Summon))
            DrawSummonAreaGizmos();

    }
    private void DrawExplosionAreaGizmos()
    {
        float radius = bulletData.AoeRadius * bulletData.Scale / bulletData.GizmosData.FixedReductor;

        Gizmos.color = bulletData.GizmosData.ThirdColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
    private void DrawSummonAreaGizmos()
    {
        float angleOffset = bulletData.AngularOffset;
        angleOffset = Utility.NormalizeAngle(angleOffset);
        angleOffset += transform.eulerAngles.z;

        Vector3 origen = transform.position;
        Vector3 puntoCentral = origen + Quaternion.Euler(0, 0, angleOffset) * Vector3.up * bulletData.GizmosData.Radius;

        Gizmos.color = bulletData.GizmosData.SecondColor;
        Handles.color = bulletData.GizmosData.SecondColor;

        Gizmos.DrawLine(origen, puntoCentral);

        // // Asegúrate de que el ángulo esté en el rango [0, 360]
        if (Utility.NormalizeAngle(bulletData.FiringArc) == 360)
        {
            Handles.DrawWireDisc(origen, Vector3.forward, bulletData.GizmosData.Radius);
            return;
        }

        Vector3 puntoA = origen + Quaternion.Euler(0, 0, -bulletData.FiringArc / 2 + angleOffset) * Vector3.up * bulletData.GizmosData.Radius;
        Vector3 puntoB = origen + Quaternion.Euler(0, 0, bulletData.FiringArc / 2 + angleOffset) * Vector3.up * bulletData.GizmosData.Radius;

        Gizmos.DrawLine(origen, puntoA);
        Gizmos.DrawLine(origen, puntoB);
        Handles.DrawWireArc(origen, Vector3.forward, puntoA - origen, bulletData.FiringArc, bulletData.GizmosData.Radius);
    }

    private void Movement()
    {
        Vector3 translateMovement = Vector3.up * bulletData.Speed * Time.fixedDeltaTime;
        transform.Translate(translateMovement, Space.Self);
        RotateObject(bulletData.AngularSpeed);
    }

    private void RotateObject(float angularSpeed)
    {
        float angle = transform.localRotation.eulerAngles.z;
        angle += angularSpeed * Time.fixedDeltaTime;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
    private void DoDamage(Scr_Entity entity)
    {
        PostMortemBulletAction postMortem = bulletData.PostMortem;

        entity.Hurt(new Damage(damage, type));
        if (postMortem.HasFlag(PostMortemBulletAction.Alter))
            Debug.Log("Im Affecting");
    }
    private void Death()
    {
        PostMortemBulletAction postMortem = bulletData.PostMortem;

        if (postMortem.HasFlag(PostMortemBulletAction.Explode))
            Explode();
        if (postMortem.HasFlag(PostMortemBulletAction.Summon))
            Summon();

        if (pierceCounter > 0)
            pierceCounter--;
        else
            gameObject.SetActive(false);

    }
    private void Explode()
    {
        Collider2D[] inAoeArea = Physics2D.OverlapCircleAll(transform.position, bulletData.AoeRadius);

        foreach (Collider2D collision in inAoeArea)
        {
            if (collision.TryGetComponent(out Scr_Entity entity))
                DoDamage(entity);
        }
    }

    private void Summon()
    {
        int summonQuantity = bulletData.SubprojectileQuantity > 0 ? bulletData.SubprojectileQuantity : bulletData.AvailableSubprojectiles;

        for (int i = 0; i < summonQuantity; i++)
        {
            int x = i % bulletData.AvailableSubprojectiles;

            float angleArc = 0;
            float lateralOffset = 0;

            if (summonQuantity != 1)
            {
                float limit;
                float percentage;

                if (Utility.NormalizeAngle(bulletData.FiringArc) == 360)
                    limit = 180f * (summonQuantity - 1) / summonQuantity;
                else
                    limit = bulletData.FiringArc / 2;

                percentage = (float)i / (summonQuantity - 1);
                angleArc = Mathf.Lerp(limit, -limit, percentage);

                limit = bulletData.Spacing / 2;
                percentage = (float)i / (summonQuantity - 1);
                lateralOffset = Mathf.Lerp(limit, -limit, percentage);
            }

            angleArc += bulletData.AngularOffset;


            float subprojectileDamage = bulletData.InheritDamage ? damage / summonQuantity : bulletData.SubprojectileDamage;
            DamageTypes subprojectileType = bulletData.InheritType ? type : bulletData.SubprojectileType;

            Scr_BulletPool.Instance.spawnBullet(gameObject.transform.position, lateralOffset, bulletData.Subprojectile[x], transform.eulerAngles.z + angleArc, gameObject.tag, subprojectileDamage, subprojectileType);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (CompareTag(collision.tag) || !Tags.isTeamTag(collision.tag))
            return;

        if (collision.TryGetComponent(out Scr_Entity entity))
        {
            PostMortemBulletAction postMortem = bulletData.PostMortem;

            if (!postMortem.HasFlag(PostMortemBulletAction.Explode))
                DoDamage(entity);

            Death();
        }
    }
}