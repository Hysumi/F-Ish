using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    public GameObject HUD;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GetComponent<Dialog>()._isOpened = false;
            this.gameObject.SetActive(false);
        }
    }
    void OnDisable()
    {
        HUD.SetActive(true);
    }
}
