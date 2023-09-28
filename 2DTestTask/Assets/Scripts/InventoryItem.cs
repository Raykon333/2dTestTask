using UnityEngine;

public class InventoryItem
{
    public string Name;

    public override bool Equals(object obj)
    {
        var toItem = (InventoryItem)obj;
        if (toItem == null)
            return false;
        return toItem.Name == Name;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
