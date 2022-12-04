using UnityEngine;
[CreateAssetMenu(fileName ="New Player Data",menuName ="Player Data")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private Color playerColor;


    public Color PlayerColor { get { return playerColor; } }
}