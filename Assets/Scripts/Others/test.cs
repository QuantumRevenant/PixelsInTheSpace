using UnityEngine;
using UnityEngine.PlayerLoop;

public class test : MonoBehaviour
{
    [SerializeField]
    private float _angulo;
    [SerializeField]
    private Vector2 _direccion;

    public float Angulo
    {
        get { return _angulo; }
        set
        {
            _angulo = value;
            _direccion = new Vector2(Mathf.Cos(_angulo * Mathf.Deg2Rad), Mathf.Sin(_angulo * Mathf.Deg2Rad));
        }
    }

    public Vector2 Direccion
    {
        get { return _direccion; }
        set
        {
            _direccion = value;
            _angulo = Mathf.Atan2(_direccion.y, _direccion.x) * Mathf.Rad2Deg;
        }
    }

    void Awake()
    {
        Angulo=50;
    }
    void Update()
    {
        
    }
}