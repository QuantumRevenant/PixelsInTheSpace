using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float baseEnemySpeed;
    [SerializeField] private Vector2 baseEnemyVector;

    [Header("Modificators")]
    [SerializeField] private float multiplicatorSpeed
;
    [SerializeField] private float size;
    [SerializeField] private float multiplicatorScale
;
    [SerializeField] private Vector2 boundaries;
    [SerializeField] private Vector2 boundOffset;

    [Header("Functionality")]
    [SerializeField] private string test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Movement()
    {
        Vector3 actualPos = transform.position;
        Vector2 boundDirection = new Vector2(1,1);

        if (actualPos.x >= boundaries.x)
        {
            boundDirection.x = -1;
        } else if (actualPos.x <= -boundaries.x)
        {
            boundDirection.x = 1;
        }

        if (actualPos.y >= boundaries.y)
        {
            boundDirection.y = -1;
        }
        else if (actualPos.y <= -boundaries.y)
        {
            boundDirection.y = 1;
        }

        float xResultado = baseEnemySpeed * baseEnemyVector.x * multiplicatorSpeed
 * boundDirection.x;
        float yResultado = baseEnemySpeed * baseEnemyVector.y * multiplicatorSpeed
 * boundDirection.y;
        Vector3 movementResultado = new Vector3(xResultado, yResultado, 0);
        transform.position += movementResultado;
    }
}
