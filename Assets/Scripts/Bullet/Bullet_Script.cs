using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Script : MonoBehaviour
{
    #region - Variables

    [Header("Stats")]
    public Vector2 bulletVector;
    public Color bulletColor;
    public float bulletSpeed;
    [Tooltip("Relation 0.1-1.8 VerticalSpeed-AngularSpeed")]
    public float bulletDesviation = 0f;
    [SerializeField] private float bulletTotalDesviation = 0.0f;
    public float bulletDamage;
    [Range(-1, 1)] public int bulletDirection;
    public bool bulletEnemy;
    public float orientation;
    [HideInInspector] public Vector3 baseScale;
    [Space(10)]

    [Header("Modificators")]
    public float multiplicatorBulletScale;
    [Space(10)]

    [Header("Boundaries")]
    [SerializeField] private Vector2 boundaries;
    [SerializeField] private float boundariesOffset;
    [SerializeField] private float spriteSize;
    #endregion
    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("InstantedObject");
        baseScale = transform.localScale;
    }
    void Start()
    {

    }
    void Update()
    {
        Movement();
        ChangeSize();
        ChangeColor();
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

        float bulletAngle = Vector3.Angle(new Vector3(0.0f, 1.0f, 0.0f), new Vector3(bulletVector.x, bulletVector.y, 0.0f));
        bulletTotalDesviation = ChangeOrientation(bulletTotalDesviation, bulletDesviation);
        if (bulletVector.x < 0.0f)
        {
            bulletAngle = -bulletAngle;
            bulletAngle = bulletAngle + 360;
        }
        bulletAngle += bulletTotalDesviation;

        transform.rotation = Quaternion.Euler(0f, 0f, bulletAngle);
        Vector3 vectorFinal = transform.up * bulletDirection * bulletSpeed * Time.deltaTime;
        transform.Translate(vectorFinal, Space.Self);
    }
    private float ChangeOrientation(float input, float angularSpeed)
    {
        float Output;
        Output = input + (angularSpeed * Time.deltaTime);
        if (Output >= 360)
            Output -= 360;
        else if (Output < 0)
            Output += 360;
        return Output;
    }
    private void ChangeSize()
    {
        transform.localScale = baseScale * multiplicatorBulletScale;
    }

    private void ChangeColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = bulletColor;
    }

    private void Boundaries()
    {
        Vector3 actualPosition = transform.position;
        if (actualPosition.x > boundaries.x + boundariesOffset || actualPosition.x < -(boundaries.x + boundariesOffset))
        {
            Destroy(gameObject);
        }
        if (actualPosition.y > boundaries.y + boundariesOffset || actualPosition.y < -(boundaries.y + boundariesOffset))
        {
            Destroy(gameObject);
        }
    }
}
