using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public string itemName;
    public Item item;
    public Button button;
    public TMPro.TextMeshProUGUI text;

    void Start()
    {
        button = gameObject.GetComponent<Button>();
        text = gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    public void AddItem (Item newItem)
    {
        item = newItem;
        itemName = newItem.name;
        button.interactable = true;
        text.text = itemName;
    }

    public void ClearSlot()
    {
        item = null;
        itemName = null;

        button.interactable = false;
        text.text = "";
    }
}
