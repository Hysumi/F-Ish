using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingMenu : MonoBehaviour
{
    //Topper5
    public ScoreSlotInfo[] allSlots =new ScoreSlotInfo[5];

    public Text[] scoreTexts = new Text[5];
    public Text[] nameTexts = new Text[5];

    public HighscoreAzure azureHigh;
    public string[,] slotInfo = new string[2, 5];

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
    void GetThemScores()
    {
        for (int i = 0; i < 5; i++)
        {
            allSlots[i].Name = azureHigh._scores[i].username;
            allSlots[i].Score = azureHigh._scores[i].score;
        }
        //Scores
        for (int i = 0; i < 5; i++)
        {
            slotInfo[0, i] = allSlots[i].Score.ToString();
        }
        //Names
        for (int i = 0; i < 5; i++)
        {
            slotInfo[1, i] = allSlots[i].Name;
        }
        //SetaNomes nos Slots
        for (int i = 0; i < 5; i++)
        {
            scoreTexts[i].text = slotInfo[0, i];
        }
        for (int i = 0; i < 5; i++)
        {
            nameTexts[i].text = slotInfo[1, i];
        }

    }
    void Update()
    {
        if (azureHigh.isReadyComplete == true)
        {
            GetThemScores();
            azureHigh.isReadyComplete = false;
        }
    }

    [System.Serializable]
   public struct ScoreSlotInfo
    {
        public uint Score;
        public string Name;
    }
}
