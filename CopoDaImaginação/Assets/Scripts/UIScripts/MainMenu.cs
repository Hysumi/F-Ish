using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject RankingMenu;
    void Start()
    {
        GetComponent<Dialog>().Open();
    }
    public void CallRanking()
    {
        StartCoroutine(Closing());
    }
    IEnumerator Closing()
    {
        GetComponent<Dialog>().Close();
        yield return new WaitForSeconds(0.25f);
        OpenRanking();
        StopAllCoroutines();

    }
    void OpenRanking()
    {
        RankingMenu.SetActive(true);
        RankingMenu.GetComponent<Dialog>().Open();
    }
}
