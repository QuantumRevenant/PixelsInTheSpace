using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using GeneralNS;
[System.Flags] public enum PostMortemBulletAction { Nothing = 0, Explode = 1, Summon = 2, All = -1 }
public class Scr_Bullet : MonoBehaviour
{
    [SerializeField] private ScO_Bullet bulletData;
    [SerializeField] private float timerActive = float.PositiveInfinity;
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
    void Update()
    {
        timerDeactivate();
        if (BulletData != null)
            Movement();
    }
    private void OnEnable() { UpdateProperties(); }
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
            Quaternion.Euler(0, 0, bulletData.Angle),
            new Vector3(bulletData.Scale, bulletData.Scale, bulletData.Scale),
            bulletData.LifeTime);
    }

    [ContextMenu("resetProperties")]
    private void resetProperties()
    {
        if(isReset())
            return;

        bulletData = null;
        cleanProperties();
    }

    [ContextMenu("cleanProperties")]
    private void cleanProperties()
    {
        if(isClean())
            return;

        setProperties(
            null,
            new Color(),
            Quaternion.Euler(0, 0, 0),
            new Vector3(1, 1, 1),
            float.PositiveInfinity,Tags.NeutralTeam);
    }
    private void setProperties(Sprite sprite, Color color, Quaternion euler, Vector3 scale, float timer)
    {
        setProperties(sprite,color,euler,scale,timer,gameObject.tag);
    }
    private void setProperties(Sprite sprite, Color color, Quaternion euler, Vector3 scale, float timer,string tag)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
        transform.rotation = euler;
        transform.localScale = scale;
        timerActive = timer;
        gameObject.tag=tag;
    }
    [ContextMenu("isReset")]
    private bool isReset() { return isClean() && BulletData == null;}

    [ContextMenu("isClean")]
    private bool isClean()
    {
        bool output = true;
        output &= spriteRenderer.sprite == null
        && spriteRenderer.color == new Color()
        && transform.rotation == Quaternion.Euler(0, 0, 0)
        && transform.localScale == new Vector3(1, 1, 1)
        && timerActive == float.PositiveInfinity;
        Debug.Log(output);
        return output;
    }
    #endregion
    
    private void timerDeactivate()
    {
        timerActive -= Time.deltaTime;
        if (timerActive <= 0)
            gameObject.SetActive(false);
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
        Vector3 translateMovement = Vector3.up * bulletData.Speed * Time.deltaTime;
        transform.Translate(translateMovement, Space.Self);
        RotateObject(bulletData.AngularSpeed);
    }

    private void RotateObject(float angularSpeed)
    {
        float angle = transform.localRotation.eulerAngles.z;
        angle += angularSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}
