using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    MenuEvents menuEvents;
    void Start()
    {
        menuEvents = GetComponentInParent<MenuEvents>();
    }
    public void ExitButton()
    {
        menuEvents.Exit();
    }
    public void RankingButton()
    {
        menuEvents.Ranking();
    }
    public void StartButton()
    {
        menuEvents.Exit();
    }
}
