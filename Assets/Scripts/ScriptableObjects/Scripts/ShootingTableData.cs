using UnityEngine;
[CreateAssetMenu(fileName = "New Shooting Table Data", menuName = "Scriptable Objects/Shooting Table Data")]
public class ShootingTableData : ScriptableObject
{
    [SerializeField]private ShootingData[] _shootingData=new ShootingData[1];
    public ShootingData[] shootingData { get { return _shootingData; } }
}