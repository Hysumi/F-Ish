using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public delegate void Depause();
    public static event Depause unPauseEvent;

    public delegate void EndStage();
    public static event EndStage endstageEvent;

    public void Awake()
    {
        MenuController.instance.pauseMenu = this;
    }
    public void DePause()
    {
        unPauseEvent();
    }

    public void EndsStage()
    {
        endstageEvent();
    }
}
