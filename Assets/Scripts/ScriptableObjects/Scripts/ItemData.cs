using UnityEngine;
[CreateAssetMenu(fileName = "New Item Data", menuName = "Scriptable Objects/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private Sprite itemSprite;
    [SerializeField] private bool isActivable = true;
    [SerializeField] private Items itemClass;
    [SerializeField] private bool isTemporal = false;
    [SerializeField] private float multiplicatorValue = 0f;
    [SerializeField] private MultiplicatorType multiplicatorType;
    [SerializeField] private float itemSpeed = 0.5f;

    public Sprite ItemSprite { get { return itemSprite; } }
    public bool IsActivable { get { return isActivable; } }
    public Items ItemClass { get { return itemClass; } }
    public bool IsTemporal { get { return isTemporal; } }
    public float MultiplicatorValue { get { return multiplicatorValue; } }
    public MultiplicatorType _multiplicatorType { get { return multiplicatorType; } }
    public float ItemSpeed { get { return itemSpeed; } }
}