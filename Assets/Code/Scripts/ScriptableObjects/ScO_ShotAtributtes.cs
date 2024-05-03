using QuantumRevenant.CustomEditors;
using QuantumRevenant.PixelsinTheSpace;
using UnityEngine;
[CreateAssetMenu(fileName = "New ShotAtributtes Data", menuName = "Scriptable Objects/ShotAtributtes_Data")]
public class ScO_ShotAtributtes : ScriptableObject
{
    [SerializeField] private string idName;
    [SerializeField][Range(0, 360)] private float firingArc;
    [SerializeField] private int projectileQuantity;
    [SerializeField] private float angularOffset;
    [SerializeField] private float angularOffsetSpeed;
    [ExposedScriptableObject]
    [SerializeField] private ScO_Bullet bullet;
    [SerializeField] private float damage;
    [SerializeField] private DamageTypes type;
    [SerializeField] private float reloadTime;

    //Cartridge Integration
    [SerializeField] private float spacing;

    public string IdName { get => idName; set => idName = value; }
    public float FiringArc { get => firingArc; set => firingArc = value; }
    public int ProjectileQuantity { get => projectileQuantity; set => projectileQuantity = value; }
    public float AngularOffset { get => angularOffset; set => angularOffset = value; }
    public float AngularOffsetSpeed { get => angularOffsetSpeed; set => angularOffsetSpeed = value; }
    public ScO_Bullet Bullet { get => bullet; set => bullet = value; }
    public float Spacing { get => spacing; set => spacing = value; }
    public float Damage { get => damage; set => damage = value; }
    public DamageTypes Type { get => type; set => type = value; }
    public float ReloadTime { get => reloadTime; set => reloadTime = value; }
}