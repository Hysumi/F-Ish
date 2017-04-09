using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject RankingMenu,CabinMenu;
    void Start()
    {
        GetComponent<Dialog>().Open();
    }
    public void CallRanking()
    {
        StartCoroutine(ClosingForRanking());
    }
    IEnumerator ClosingForRanking()
    {
        GetComponent<Dialog>().Close();
        yield return new WaitForSeconds(0.25f);
        OpenRanking();
        yield break;
    }
    void OpenRanking()
    {
        RankingMenu.SetActive(true);
        RankingMenu.GetComponent<Dialog>().Open();
    }

    public void CallStart()
    {
        StartCoroutine(ClosingForCabin());
    }
    IEnumerator ClosingForCabin()
    {
        GetComponent<Dialog>().Close();
        yield return new WaitForSeconds(0.25f);
        OpenCabin();
        yield break;
    }
    void OpenCabin()
    {
        CabinMenu.GetComponent<Dialog>().Open();
    }
}
