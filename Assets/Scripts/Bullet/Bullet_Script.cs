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
    public float bulletDamage;
    [Range(-1, 1)] public int bulletDirection;
    public bool bulletEnemy;
    public float orientation;
    private Vector3 baseScale;
    [Space(10)]

    [Header("Modificators")]
    public float multiplicatorBulletScale;
    [Space(10)]

    [Header("Boundaries")]
    [SerializeField] private Vector2 boundaries;
    [SerializeField] private float boundariesOffset;
    [SerializeField] private float spriteSize;

    
    #endregion

    #region - Awake/Start
    private void Awake()
    {
        baseScale = transform.localScale;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        Movement();
        ChangeSize();
        ChangeColor();
        Boundaries();
    }

    private void Movement()
    {
        switch (bulletEnemy)
        {
            case true:
                {
                    gameObject.tag = "EnemyProjectile";
                    break;
                }
            case false:
                {
                    gameObject.tag = "AllyProjectile";
                    break;
                }
        }
        Vector2 direction = bulletVector.normalized;
        Vector3 VectorInicial =new Vector3(direction.x, direction.y*bulletDirection, 0);
        Vector3 vectorFinal = VectorInicial.normalized * bulletSpeed * Time.deltaTime;
        transform.position += vectorFinal;
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
        if (actualPosition.x > boundaries.x + boundariesOffset||actualPosition.x<-(boundaries.x + boundariesOffset))
        {
            Destroy(gameObject);
        }
        if (actualPosition.y> boundaries.y + boundariesOffset||actualPosition.y<-(boundaries.y + boundariesOffset))
        {
            Destroy(gameObject);
        }
    }
}
