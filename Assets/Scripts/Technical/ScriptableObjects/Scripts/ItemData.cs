using UnityEngine;
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Objects/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private bool isActivable = true;
    [SerializeField] private Items itemClass;
    [SerializeField] private bool isTemporal = false;
    [SerializeField] private float multiplicatorValue = 0f;
    [SerializeField] private float itemSpeed = 0.5f;
}
