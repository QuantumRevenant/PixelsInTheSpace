using UnityEngine;
[CreateAssetMenu(fileName = "New Catridge Data", menuName = "Scriptable Objects/Cartridge_Data")]
public class ScO_Cartridge : ScriptableObject
{
    private string identifier;
    private ScO_Bullet[] bullets;
    private float separation;
    public string Identifier { get => identifier; set => identifier = value; }
    public ScO_Bullet[] Bullets { get => bullets; set => bullets = value; }
    public int bulletsQuantity {get => bullets.Length;}
    public float Separation { get => separation; set => separation = value; }
}
