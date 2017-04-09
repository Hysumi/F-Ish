using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    public delegate void PauseDelegate();
    public static event PauseDelegate pauseEvent;


    public void PauseButtonClicked()
    {
        pauseEvent();
        gameObject.SetActive(false);
    }

}
