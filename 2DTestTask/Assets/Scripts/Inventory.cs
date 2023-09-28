using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] InventorySlot SlotPrefab;
    [SerializeField] RectTransform Container;
    [SerializeField] int Size;
    public InventoryItem[] items;
    public int[] amounts;
    private InventorySlot[] Slots;

    [SerializeField] Sprite BulletIcon;
    [SerializeField] Sprite CoinIcon;


    // Start is called before the first frame update
    void Awake()
    {
        items = new InventoryItem[Size];
        amounts = new int[Size];
        Slots = new InventorySlot[Size];
        
        for (int i = 0; i < Size; i++)
        {
            Slots[i] = Instantiate(SlotPrefab, Container);
            Slots[i].Id = i;
            Slots[i].parentInventory = this;
            Slots[i].UpdateSlot(null, null, 0, 0);
        }
    }

    public int GetItemAmount(string name)
    {
        for (int i = 0; i < Size; i++)
        {
            if (items[i].Name == name)
                return amounts[i];
        }
        return 0;
    }

    public bool TryAddItem(InventoryItem item, int amount)
    {
        int i = 0;
        while(i < Size)
        {
            if (item.Equals(items[i]) || amounts[i] == 0)
                break;
            i++;
        }

        if (i == Size)
            return false;

        items[i] = item;
        amounts[i] += amount;
        var icon = IconAndScaleFromName(item);
        Slots[i].UpdateSlot(items[i], icon.icon, icon.iconScale, amounts[i]);
        return true;
    }

    public void RemoveItem(string name, int amount)
    {
        int i;
        for (i = 0; i < Size; i++)
        {
            if (items[i].Name == name)
            {
                if (amounts[i] < amount)
                    throw new ArgumentException();
                else
                    amounts[i] -= amount;
                var icon = IconAndScaleFromName(items[i]);
                Slots[i].UpdateSlot(items[i], icon.icon, icon.iconScale, amounts[i]);
                break;
            }
        }
        if (i == Size)
            throw new ArgumentException();
    }

    public void DeleteItem(int slotId)
    {
        items[slotId] = null;
        amounts[slotId] = 0;
        Slots[slotId].UpdateSlot(items[slotId], null, 0, amounts[slotId]);
    }

    public void DeleteAll()
    {
        for (int i = 0; i < Size; i++)
            DeleteItem(i);
    }

    public void UpdateAllSlots()
    {
        for (int i = 0; i < Size; i++)
        {
            var icon = IconAndScaleFromName(items[i]);
            Slots[i].UpdateSlot(items[i], icon.icon, icon.iconScale, amounts[i]);
        }
    }

    private (Sprite icon, float iconScale) IconAndScaleFromName(InventoryItem item)
    {
        if (item == null)
            return (null, 0);

        Sprite icon = default;
        float iconScale = 0;
        if (item.Name == "bullet")
        {
            icon = BulletIcon;
            iconScale = 0.7f;
        }
        else if (item.Name == "coin")
        {
            icon = CoinIcon;
            iconScale = 0.4f;
        }
        return (icon, iconScale);
    }
}
