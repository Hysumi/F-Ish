using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralMenuController : MonoBehaviour
{
    //Evento de pause para poder ser registrado de qualquer lugar (por exemplo parar o gameplay sem setar timescale para 0)
    public delegate void OnPauseButtonClicked();
    public static OnPauseButtonClicked PauseButtonClick;

    //Função ativada pelo botão de pause
    public void FirePauseEvent()
    {
        //Da fire no evento, qualquer funçao ligada a ele é ativada
        PauseButtonClick();
    }

}
