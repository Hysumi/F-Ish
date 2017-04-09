using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingMenu : MonoBehaviour
{
    public GameObject MainMenu;
    public void CallMenu()
    {
        StartCoroutine(Closing());
    }
    IEnumerator Closing()
    {
        GetComponent<Dialog>().Close();
        yield return new WaitForSeconds(0.25f);
        OpenMenu();
        StopAllCoroutines();
        yield break;
    }
    void OpenMenu()
    {
        MainMenu.SetActive(true);
        MainMenu.GetComponent<Dialog>().Open();
    }
}
