using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using QuantumRevenant.Utilities;
using QuantumRevenant.GeneralNS;
using QuantumRevenant.PixelsinTheSpace;
using System;
[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(BoxCollider2D)), RequireComponent(typeof(SpriteRenderer))]
public class Scr_Bullet : MonoBehaviour
{
    [SerializeField] private ScO_Bullet bulletData;
    [SerializeField] private float timerActive = float.PositiveInfinity;
    /*[HideInInspector]*/
    public float inheritedAngle = 0;
    private int pierceCounter = 0;
    public ScO_Bullet BulletData
    {
        get { return bulletData; }
        set
        {
            bulletData = value;
            UpdateProperties();
        }
    }

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }
    void Start() { UpdateProperties(); }
    void FixedUpdate()
    {
        timerDeactivate();
        if (BulletData != null)
            Movement();
    }
    private void OnEnable() { }
    private void OnDisable() { resetProperties(); }


    #region BulletProperties
    [ContextMenu("updateProperties")]
    private void UpdateProperties()
    {
        if (BulletData == null)
            return;

        setProperties(
            bulletData.Sprite,
            bulletData.Color,
            bulletData.Angle + inheritedAngle,
            new Vector3(bulletData.Scale, bulletData.Scale, bulletData.Scale),
            bulletData.LifeTime, bulletData.PierceCounter);
    }

    [ContextMenu("resetProperties")]
    public void resetProperties()
    {
        if (isReset())
            return;

        bulletData = null;
        cleanProperties();
    }

    [ContextMenu("cleanProperties")]
    private void cleanProperties()
    {
        if (isClean())
            return;
        inheritedAngle = 0;
        setProperties(
            null,
            new Color(),
            0,
            new Vector3(1, 1, 1),
            float.PositiveInfinity, Tags.NeutralTeam);
    }
    private void setProperties(Sprite sprite, Color color, float angle, Vector3 scale, float timer, int cPierce = 0)
    {
        setProperties(sprite, color, angle, scale, timer, gameObject.tag);
    }
    private void setProperties(Sprite sprite, Color color, float angle, Vector3 scale, float timer, string stag, int cPierce = 0)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
        transform.eulerAngles = new Vector3(0, 0, angle);
        transform.localScale = scale;
        timerActive = timer;
        gameObject.tag = tag;
        resizeBulletCollider2D();
    }

    public void resizeBulletCollider2D()
    {
        float resizeFactor;

        if (tag == Tags.NeutralTeam || tag == Tags.PlayerTeam)
            resizeFactor = 1f;
        else
            resizeFactor = 0.5f;

        boxCollider2D = Utility.ResizeBoxCollider2D(spriteRenderer, boxCollider2D, transform.localScale, resizeFactor);
    }


    [ContextMenu("isReset")]
    private bool isReset() { return isClean() && BulletData == null; }

    [ContextMenu("isClean")]
    private bool isClean()
    {
        return spriteRenderer.sprite == null
        && spriteRenderer.color == new Color()
        && transform.rotation == Quaternion.Euler(0, 0, 0)
        && transform.localScale == new Vector3(1, 1, 1)
        && timerActive == float.PositiveInfinity && inheritedAngle == 0;
    }
    #endregion

    private void timerDeactivate()
    {
        timerActive -= Time.fixedDeltaTime;
        if (timerActive <= 0)
        {
            death();
        }
    }

    private void OnDrawGizmos()
    {
        float fixedReductor=100f; // With this reducer, when the Explosion Radius is 1, it is the standard size of the spherical bullet

        float radius = 0;
        if (bulletData != null)
            radius = bulletData.AoeRadius*bulletData.Scale/fixedReductor;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
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
    private void doDamage(Scr_Entity entity, Damage damage)
    {
        PostMortemBulletAction postMortem = bulletData.PostMortem;

        entity.hurt(damage);
        if (postMortem.HasFlag(PostMortemBulletAction.Alter))
            Debug.Log("Im Affecting");
    }
    private void death()
    {
        PostMortemBulletAction postMortem = bulletData.PostMortem;

        if (postMortem.HasFlag(PostMortemBulletAction.Explode))
            explode();
        if (postMortem.HasFlag(PostMortemBulletAction.Summon))
            summon();

        if (pierceCounter > 0)
            pierceCounter--;
        else
            gameObject.SetActive(false);

    }
    private void explode()
    {
        Collider2D[] inAoeArea = Physics2D.OverlapCircleAll(transform.position, bulletData.AoeRadius);

        foreach (Collider2D collision in inAoeArea)
        {
            if (collision.TryGetComponent(out Scr_Entity entity))
            {
                doDamage(entity, new Damage(bulletData.Damage, DamageTypes.Neutral));
            }
        }
    }

    private void summon()
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

            Scr_BulletPool.Instance.spawnBullet(gameObject.transform.position, lateralOffset, bulletData.Subprojectile[x], transform.eulerAngles.z + angleArc, gameObject.tag);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == tag || !Tags.isTeamTag(collision.tag))
            return;

        if (collision.TryGetComponent(out Scr_Entity entity))
        {
            PostMortemBulletAction postMortem = bulletData.PostMortem;

            if (!postMortem.HasFlag(PostMortemBulletAction.Explode))
                doDamage(entity, new Damage(bulletData.Damage, DamageTypes.Neutral));

            death();
        }
    }

    
}
