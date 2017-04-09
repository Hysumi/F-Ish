using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Image[] itemImages = new Image[numItemslots];
    public Item[] items = new Item[numItemslots];

    public const int numItemslots = 12;

    public delegate void ItemToggle(Item currentItem);
    public static event ItemToggle itemSelected;
    public void AddItem(Item itemToAdd)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i]==null)
            {
                items[i] = itemToAdd;
                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;
                itemImages[i].gameObject.transform.Rotate(new Vector3(0, 0, 45));
                itemImages[i].gameObject.transform.localScale = new Vector3(.4f, .48f, 1f);
                return;
            }
        }
    }
    public void RemoveItem(Item itemToRemove)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == itemToRemove)
            {
                items[i] = null;
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
                itemImages[i].gameObject.transform.Rotate(new Vector3(0, 0, -45));
                itemImages[i].gameObject.transform.localScale = new Vector3(1, 1, 1);
                return;
            }
        }
    }

    public void OnSelect(int slotNumber)
    {
        itemSelected(items[slotNumber]);
    }

}
