using Unity.Mathematics;
using UnityEngine;
[CreateAssetMenu(fileName = "New Bullet Data", menuName = "Scriptable Objects/Bullet_Data")]
public class ScO_Bullet : ScriptableObject
{
    [SerializeField] private Team team=Team.Neutral;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Color color = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private float speed;
    [SerializeField] private float angularSpeed;
    [SerializeField][Range(0, 360)] private float angle;
    [SerializeField] private float scale;
    [SerializeField] private float lifeTime;
    [SerializeField] private float damage;
    [SerializeField] private float aoeDamage;
    [SerializeField] private float aoeRadius;
    public Sprite Sprite { get => sprite; set => sprite = value; }
    public float Speed { get => speed; set => speed = value; }
    public float Scale { get => scale; set => scale = value; }
    public float AngularSpeed { get => angularSpeed; set => angularSpeed = value; }
    public float Angle { get => angle; set => angle = value; }
    public float LifeTime { get => lifeTime; set => lifeTime = value; }
    public Color Color { get => color; set => color = value; }
    public float Damage { get => damage; set => damage = value; }
    public float AoeDamage { get => aoeDamage; set => aoeDamage = value; }
    public float AoeRadius { get => aoeRadius; set => aoeRadius = value; }
}