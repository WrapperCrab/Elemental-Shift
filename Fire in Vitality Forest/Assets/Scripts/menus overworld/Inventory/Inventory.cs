using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //something called a singleton to make searching for this way faster

    #region Singleton

    public static Inventory instance;
    void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of inventory found!");
            return;
        }
        instance = this;
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int itemSpace = 20;

    public List<Item> items = new List<Item>();

    public Dialogue noSpace;

    public bool Add (Item item)
    {
        if (!item.isDefaultItem)
        {
            if (items.Count >= itemSpace)
            {
                Debug.Log("not enough room");
                return false;//failed to pick up item
            }
            items.Add(item);
            if (onItemChangedCallback != null)
            {
                onItemChangedCallback.Invoke();
            }
        }
        return true;
    }

    public void Remove (Item item)
    {
        items.Remove(item);
        if (onItemChangedCallback != null)
        {
            onItemChangedCallback.Invoke();
        }
    }

    public Dialogue getDialogue()
    {
        return noSpace;
    }
}
