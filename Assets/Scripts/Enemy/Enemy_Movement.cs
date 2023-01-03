using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Enemy_Manager))]
public class Enemy_Movement : MonoBehaviour
{
    [Header("Modificators")]
    private float multiplicatorSpeed;
    private float multiplicatorScale;
    [SerializeField] private Vector2 boundOffset;
    private Vector2 finalOffset;
    [Header("Functionality")]
    private Vector2 boundDirection = new Vector2(1, 1);
    [SerializeField] private EnemyData enemyData;

    void Awake()
    {
        enemyData = gameObject.GetComponent<Enemy_Manager>().enemyData;
    }
    void Update()
    {
        GetVariables();
        ActualizarOffset();
        Movement();
    }
    
    private void GetVariables()
    {
        Enemy_Manager EnemyManager;
        EnemyManager = gameObject.GetComponent<Enemy_Manager>();
        multiplicatorScale = EnemyManager.multiplicatorScale;
        multiplicatorSpeed = EnemyManager.multiplicatorSpeed;
    }
    private void ActualizarOffset()
    {
        finalOffset = boundOffset + new Vector2(enemyData.EnemySize * multiplicatorScale, enemyData.EnemySize * multiplicatorScale);
    }
    private void Movement()
    {
        Vector3 pos = transform.position;
        Vector2 resultado = new Vector2(0, 0);

        if (pos.x >= (enemyData.EnemyBoundaries.x - finalOffset.x))
            boundDirection.x = -1;
        else if (pos.x <= -(enemyData.EnemyBoundaries.x - finalOffset.x))
            boundDirection.x = 1;

        if (pos.y >= (enemyData.EnemyBoundaries.y - finalOffset.y))
            boundDirection.y = -1;
        else if (pos.y <= -(enemyData.EnemyBoundaries.y - finalOffset.y))
            boundDirection.y = 1;

        resultado.x = enemyData.BaseSpeed * enemyData.BaseVector.x * multiplicatorSpeed * boundDirection.x;
        resultado.y = enemyData.BaseSpeed * enemyData.BaseVector.y * multiplicatorSpeed * boundDirection.y;
        resultado = resultado * Time.deltaTime;
        Vector3 movementResultado = new Vector3(resultado.x, resultado.y, 0);
        transform.position += movementResultado;
    }
}
