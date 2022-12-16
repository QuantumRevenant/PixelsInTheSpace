using UnityEngine;
[CreateAssetMenu(fileName = "New Shooting Data", menuName = "Scriptable Objects/Shooting Data")]
public class ShootingData : ScriptableObject
{
    [SerializeField]private ShootingType _shootingtype;
    public BulletData[] bulletData;
    public ShootingType shootingType { get { return _shootingtype; } }   
}
