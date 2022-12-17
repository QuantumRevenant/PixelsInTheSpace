using UnityEngine;
[CreateAssetMenu(fileName = "New Shooting Data", menuName = "Scriptable Objects/Shooting Data")]
public class ShootingData : ScriptableObject
{
    [SerializeField]private ShootingType _shootingtype;
    [SerializeField]private BulletData[] _bulletData=new BulletData[1];
    [SerializeField]private float angleArch=0;
    [SerializeField]private int bulletNumber=1;
    [SerializeField]private string identifier;
    public ShootingType shootingType { get { return _shootingtype; } }
    public BulletData[] bulletData { get { return _bulletData;} }
    public float AngleArch { get { return angleArch; } }
    public int BulletNumber { get { return bulletNumber; } }
    
    
}
