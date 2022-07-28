using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InventoryUI : MonoBehaviour
{
    public Transform items;
    Inventory inventory;
    InventorySlot[] itemSlots;

    public TMPro.TextMeshProUGUI descriptionName;
    public TMPro.TextMeshProUGUI description;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        itemSlots = items.GetComponentsInChildren<InventorySlot>();
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI()
    {
        for (int i=0; i < itemSlots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                itemSlots[i].AddItem(inventory.items[i]);
            }
            else
            {
                itemSlots[i].ClearSlot();
            }
        }
    }

    public void updateDescription(Item item)
    {
        descriptionName.text = item.name;
        description.text = item.description;
    }
}
