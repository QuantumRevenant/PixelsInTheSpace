using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Script : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Relation 0.1-1.8 VerticalSpeed-AngularSpeed")]
    [HideInInspector] public float initialAngle = 0.0f;
    private bool firstSaveAngle = true;
    private float bulletTotalDesviation = 0.0f;
    private int bulletDirection;
    public float bulletDamage;
    public bool bulletEnemy = false;
    [Space(10)]
    [Header("Modificators")]
    public float multiplicatorBulletScale;
    [Space(10)]
    public BulletData bulletData;
    [Header("SinusMove")]
    private Vector3 startPos;
    private float startTime = 0f;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("InstantedObject");
    }
    void Start()
    {
        startPos = transform.position;
        startTime = Time.time;
    }
    void Update()
    {
        if (bulletData == null)
            return;
        Movement();
        ChangeProperties();
        Boundaries();
    }
    private void Movement()
    {
        if (bulletEnemy)
        {
            gameObject.tag = "EnemyProjectile";
            gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
            bulletDirection = -1;
        }
        else
        {
            gameObject.tag = "AllyProjectile";
            gameObject.layer = LayerMask.NameToLayer("AllyProjectile");
            bulletDirection = 1;
        }
        float bulletAngle = GetAngle(bulletData.BaseVector.normalized, bulletData.BaseAngle, bulletData.UseAngle);
        bulletTotalDesviation = ChangeOrientation(bulletTotalDesviation, bulletData.BaseDesviation);
        bulletAngle += bulletTotalDesviation;

        if (bulletData.IsSinusoidal)
        {
            float newXPos = bulletData.SinusDirection * bulletData.Amplitude * Mathf.Sin((Time.time - startTime) * bulletData.Frecuency) + startPos.x;
            float xStep = newXPos - transform.position.x;
            transform.Translate(new Vector3(xStep, 0), Space.Self);
            bulletAngle = 0;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, initialAngle + bulletAngle);
        Vector3 vectorFinal = Vector3.up * bulletDirection * bulletData.BaseSpeed * Time.deltaTime;
        transform.Translate(vectorFinal, Space.Self);
    }
    private float GetAngle(Vector2 vector, float angle, bool useAngle)
    {
        if (useAngle)
            return -angle;
        Vector2 vec = Vector2.right;
        float outputAngle = Mathf.Atan2(-vector.x, vector.y) * 180 / Mathf.PI;
        return outputAngle;
    }
    private float ChangeOrientation(float input, float angularSpeed)
    {
        float Output;
        Output = input + (angularSpeed * Time.deltaTime);
        Output = Output >= 360 ? Output - 360 : Output < 0 ? Output + 360 : Output;
        return Output;
    }
    private void ChangeProperties()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = bulletData.BulletSprite;
        transform.localScale = bulletData.BaseScale * multiplicatorBulletScale;
        gameObject.GetComponent<SpriteRenderer>().color = bulletData.BulletColor;
    }
    private void Boundaries()
    {
        Vector3 actualPosition = transform.position;
        Vector2 boundaries = bulletData.Boundaries;
        float boundariesOffset = bulletData.BoundariesOffset;

        if (actualPosition.x > boundaries.x + boundariesOffset || actualPosition.x < -(boundaries.x + boundariesOffset))
            Destroy(gameObject);
        if (actualPosition.y > boundaries.y + boundariesOffset || actualPosition.y < -(boundaries.y + boundariesOffset))
            Destroy(gameObject);
    }
}
