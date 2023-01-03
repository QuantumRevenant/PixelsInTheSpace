using UnityEngine;
[CreateAssetMenu(fileName = "New Loot Table", menuName = "Scriptable Objects/Loot Table")]
public class LootTableData : ScriptableObject
{
    [System.Serializable]
    public struct Loot
    {
        [SerializeField] private int weight;
        [SerializeField] private Items itemClass;
        [SerializeField] private ItemData itemData;
        public int Weight { get { return weight; } }
        Items ItemClass { get { return itemClass; } }
        public ItemData _ItemData { get { return itemData; } }
    }

    public Loot[] Objetos;



}
