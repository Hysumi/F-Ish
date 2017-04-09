using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinMenu : MonoBehaviour
{
    public GameObject MainMenu;

    public void CallMenu()
    {
        StartCoroutine(ClosingForMenu());
    }

    IEnumerator ClosingForMenu()
    {
        GetComponent<Dialog>().Close();
        yield return new WaitForSeconds(0.25f);
        OpenMenu();
        yield break;
    }

    void OpenMenu()
    {
        MainMenu.SetActive(true);
        MainMenu.GetComponent<Dialog>().Open();
    }

    public void StartGame()
    {
        MenuController.instance.StartLoading("teste", true);
    }

}
