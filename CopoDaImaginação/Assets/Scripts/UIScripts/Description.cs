using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Description : MonoBehaviour
{
    public Image currentImage;
    public Text ItemName,ItemWeight,ItemHeight,ItemDescription;
    public GameObject WeightHolder, HeightHolder, DescriptionHolder;

    void Start()
    {
        Inventory.itemSelected += changeCurrentImage;
        currentImage.enabled = false;
        ItemName.enabled = false;
        DisableHolders();
    }

    void changeCurrentImage(Item newItem)
    {
        if (newItem != null)
        {
            currentImage.enabled = true;
            currentImage.sprite = newItem.sprite;
            EnableHolders();
            ItemName.enabled = true;
            ItemName.text = newItem.name;
            if (newItem.GetType() == typeof(Peixe))
            {
                ItemHeight.enabled = ItemWeight.enabled = ItemDescription.enabled = true;
                Peixe newPeixe = newItem as Peixe;
                ItemWeight.text = newPeixe.weight.ToString();
                ItemHeight.text = newPeixe.height.ToString();
                ItemDescription.text = newPeixe.characteristics;
            }
        }
        else
        {
            currentImage.sprite = null;
            currentImage.enabled = false;
            ItemName.enabled = false;
            DisableHolders();
            ItemHeight.enabled = ItemWeight.enabled = ItemDescription.enabled = false;
        }
    }

    private void DisableHolders()
    {
        WeightHolder.SetActive(false);
        HeightHolder.SetActive(false);
        DescriptionHolder.SetActive(false);
    }
    private void EnableHolders()
    {
        WeightHolder.SetActive(true);
        HeightHolder.SetActive(true);
        DescriptionHolder.SetActive(true);
    }
}
