using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] public Button DeleteButton;
    [SerializeField] Image ItemIcon;
    [SerializeField] Text Amount;
    public Inventory parentInventory;
    public int Id;

    public void UpdateSlot(InventoryItem item, Sprite icon, float iconScale, int amount)
    {
        if (amount <= 0)
        {
            HideEverything();
            return;
        }
        DeleteButton.gameObject.SetActive(true);
        ItemIcon.sprite = icon;
        ItemIcon.transform.localScale = new Vector3 (iconScale, iconScale, iconScale);
        ItemIcon.gameObject.SetActive(true);
        if (amount == 1)
            Amount.gameObject.SetActive(false);
        else
        {
            Amount.gameObject.SetActive(true);
            Amount.text = amount.ToString();
        }
    }

    private void HideEverything()
    {
        DeleteButton.gameObject.SetActive(false);
        ItemIcon.gameObject.SetActive(false);
        Amount.gameObject.SetActive(false);
    }

    public void Delete()
    {
        parentInventory.DeleteItem(Id);
    }
}
