using UnityEngine;
[CreateAssetMenu(fileName = "New ShotAtributtes Data", menuName = "Scriptable Objects/ShotAtributtes_Data")]
public class ScO_ShotAtributtes : ScriptableObject
{
    [SerializeField][Range(0,360)] private float firingArc; 
    [SerializeField] private int projectileQuantity;
    [SerializeField] private float angularOffset;
    [SerializeField] private float offsetSpeed; 
    [SerializeField] private ScO_Bullet bullet;

    public float FiringArc { get => firingArc; set => firingArc = value; }
    public int ProjectileQuantity { get => projectileQuantity; set => projectileQuantity = value; }
    public float AngularOffset { get => angularOffset; set => angularOffset = value; }
    public float OffsetSpeed { get => offsetSpeed; set => offsetSpeed = value; }
    public ScO_Bullet Bullet { get => bullet; set => bullet = value; }
}