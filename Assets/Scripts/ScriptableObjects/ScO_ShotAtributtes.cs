using UnityEngine;
[CreateAssetMenu(fileName = "New ShotAtributtes Data", menuName = "Scriptable Objects/ShotAtributtes_Data")]
public class ScO_ShotAtributtes : ScriptableObject
{
    [SerializeField][Range(0,360)] private float firingArc; 
    [SerializeField] private int roundsFired;
    [SerializeField] private float offset;
    [SerializeField] private float offsetSpeed; 
    [SerializeField] private ScO_Cartridge cartridge;

    public float FiringArc { get => firingArc; set => firingArc = value; }
    public int RoundsFired { get => roundsFired; set => roundsFired = value; }
    public float Offset { get => offset; set => offset = value; }
    public float OffsetSpeed { get => offsetSpeed; set => offsetSpeed = value; }
    public ScO_Cartridge Cartridge { get => cartridge; set => cartridge = value; }
}