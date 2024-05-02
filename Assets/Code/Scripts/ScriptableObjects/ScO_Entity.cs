using UnityEngine;

[CreateAssetMenu(fileName = "New Entity Data", menuName = "Scriptable Objects/Entity_Data", order = 0)]
public class ScO_Entity : ScriptableObject
{

    [Header("Armor")]
    [SerializeField] private int maxArmor = 1;

    [Header("Health")]
    [SerializeField] private float maxHealth = 10;

    [Header("Invulnerability")]
    [SerializeField] private float invulnerabilityTime = 0;
    
    [Header("Movement")]
    [SerializeField] private Vector2 speedMovement = Vector2.zero;
    [SerializeField] private Vector2 boundaries;
    [SerializeField] private float waitTimeWaypoints=0;



    public int MaxArmor { get => maxArmor; set => maxArmor = value; }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    public float InvulnerabilityTime { get => invulnerabilityTime; set => invulnerabilityTime = value; }
    public Vector2 SpeedMovement { get => speedMovement; set => speedMovement = value; }
    public Vector2 Boundaries { get => boundaries; set => boundaries = value; }
    public float WaitTimeWaypoints { get => waitTimeWaypoints; set => waitTimeWaypoints = value; }
}