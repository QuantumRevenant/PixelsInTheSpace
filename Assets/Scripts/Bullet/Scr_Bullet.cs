using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using QuantumRevenant.GeneralNS;
using System;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using QuantumRevenant.Utilities;
[System.Flags] public enum PostMortemBulletAction { Nothing = 0, Explode = 1, Summon = 2, All = -1 }
public class Scr_Bullet : MonoBehaviour
{
    [SerializeField] private ScO_Bullet bulletData;
    [SerializeField] private float timerActive = float.PositiveInfinity;
    /*[HideInInspector]*/ public float inheritedAngle=0;
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

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    void Start() { UpdateProperties(); }
    void FixedUpdate()
    {
        timerDeactivate();
        if (BulletData != null)
            Movement();
    }
    private void OnEnable() {}
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
            bulletData.Angle+inheritedAngle,
            new Vector3(bulletData.Scale, bulletData.Scale, bulletData.Scale),
            bulletData.LifeTime);
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
        inheritedAngle=0;
        setProperties(
            null,
            new Color(),
            0,
            new Vector3(1, 1, 1),
            float.PositiveInfinity, Tags.NeutralTeam);
    }
    private void setProperties(Sprite sprite, Color color,  float angle, Vector3 scale, float timer)
    {
        setProperties(sprite, color, angle, scale, timer, gameObject.tag);
    }
    private void setProperties(Sprite sprite, Color color, float angle, Vector3 scale, float timer, string tag)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
        transform.eulerAngles=new Vector3(0,0,angle);
        transform.localScale = scale;
        timerActive = timer;
        gameObject.tag = tag;
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
        && timerActive == float.PositiveInfinity&&inheritedAngle==0;
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
        float radius = 0;
        if (bulletData != null)
            radius = bulletData.AoeRadius;

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
    private void death()
    {
        PostMortemBulletAction postMortem = bulletData.PostMortem;

        if (postMortem.HasFlag(PostMortemBulletAction.Explode))
            explode();
        if (postMortem.HasFlag(PostMortemBulletAction.Summon))
            summon();

        gameObject.SetActive(false);
    
    }
    private void explode()
    {
        Debug.Log("I exploded!",this);
    }

    private void summon()
    {
        int summonQuantity=bulletData.SubprojectileQuantity>0?bulletData.SubprojectileQuantity:bulletData.AvailableSubprojectiles;

        for (int i = 0; i < summonQuantity; i++)
        {
            int x=i%bulletData.AvailableSubprojectiles;

            float angleArc = 0;
            float lateralOffset = 0;

            if (summonQuantity != 1)
            {
                float limit=0;
                float percentage=0;

                if(Utility.NormalizeAngle(bulletData.FiringArc)==360)
                    limit=180f*(summonQuantity-1)/summonQuantity;
                else
                    limit =bulletData.FiringArc / 2;

                percentage = (float)i / (summonQuantity - 1);
                angleArc = Mathf.Lerp(limit, -limit, percentage);

                limit = bulletData.Spacing / 2;
                percentage = (float)i / (summonQuantity - 1);
                lateralOffset = Mathf.Lerp(limit, -limit, percentage);
            }

            angleArc += bulletData.AngularOffset;

            Scr_BulletPool.Instance.spawnBullet(gameObject.transform.position, lateralOffset,bulletData.Subprojectile[x],transform.eulerAngles.z+angleArc,gameObject.tag);
        }
    }
}
