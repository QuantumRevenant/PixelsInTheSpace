using UnityEngine;
[CreateAssetMenu(fileName = "New Shooting Data", menuName = "Scriptable Objects/Shooting Data")]
public class ShootingData : ScriptableObject
{
    [SerializeField]private ShootingTypes _shootingtype;
    [SerializeField]private BulletData[] _bulletData=new BulletData[1];
    [SerializeField]private float angleArch=0;
    [SerializeField]private float bulletSeparation=0;
    [SerializeField]private int bulletNumber=1;
    [SerializeField]private string identifier;

    public ShootingTypes shootingType { get { return _shootingtype; } }
    public BulletData[] bulletData { get { return _bulletData;} }
    public float AngleArch { get { return angleArch; } }
    public float BulletSeparation { get { return bulletSeparation; } }
    public int BulletNumber { get { return bulletNumber; } }
    public string Identifier { get { return identifier; } }
}
