using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory_Data", menuName = "GameData/Inventory_Data")]
public class Inventory_Data : ScriptableObject
{
    public List<InventoryEntry> inventory = new List<InventoryEntry>(); // 인벤토리

    [System.Serializable]
    public class InventoryEntry
    {
        public Item_Data item; // 아이템 데이터
        public int count; // 아이템 개수
    }

    public void AddItem(Item_Data item)
    {
        InventoryEntry existingEntry = inventory.Find(entry => entry.item.itemCode == item.itemCode);
        if (existingEntry != null)
        {
            existingEntry.count++;
        }
        else
        {
            inventory.Add(new InventoryEntry { item = item, count = 1 });
        }
    }

    public void initInventory()
    {
        inventory.Clear();
    }
}