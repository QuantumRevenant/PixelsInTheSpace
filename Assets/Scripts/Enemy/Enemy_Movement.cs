using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float baseSpeed;
    [SerializeField] private Vector2 baseVector;

    [Header("Modificators")]
    [SerializeField] private float multiplicatorSpeed
;
    [SerializeField] private float size;
    [SerializeField] private float multiplicatorScale
;
    [SerializeField] private Vector2 boundaries;
    [SerializeField] private Vector2 boundOffset;

    [Header("Functionality")]
    private Vector2 boundDirection = new Vector2(1,1);

    // Start is called before the first frame update
    void Start()
    {
        boundOffset+=new Vector2(size*multiplicatorScale, size*multiplicatorScale);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        Vector3 pos = transform.position;
        
        Vector2 resultado=new Vector2(0,0);

        if (pos.x >= (boundaries.x-boundOffset.x))
        {
            boundDirection.x = -1;
        } else if (pos.x <= -(boundaries.x-boundOffset.x))
        {
            boundDirection.x = 1;
        }

        if (pos.y >= (boundaries.y-boundOffset.y))
        {
            boundDirection.y = -1;
        }
        else if (pos.y <= -(boundaries.y-boundOffset.y))
        {
            boundDirection.y = 1;
        }

        resultado.x = baseSpeed * baseVector.x * multiplicatorSpeed * boundDirection.x;
        resultado.y = baseSpeed * baseVector.y * multiplicatorSpeed * boundDirection.y;
        resultado=resultado*Time.deltaTime;
        Vector3 movementResultado = new Vector3(resultado.x, resultado.y, 0);
        transform.position += movementResultado;
    }
}
