using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryButton : MonoBehaviour, ISelectHandler
{
    public GameObject canvas;

    public void OnSelect(BaseEventData eventData)//An item button has been selected
    {
        //Debug.Log(gameObject.GetComponent<InventorySlot>() == null);
        canvas.GetComponent<InventoryUI>().updateDescription(gameObject.GetComponent<InventorySlot>().item);
    }
}
