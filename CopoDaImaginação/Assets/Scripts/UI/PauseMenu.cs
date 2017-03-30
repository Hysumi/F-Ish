using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //Referencia pro gameObject no Editor
    public GameObject pauseMenuContainer;

    void Start()
    {
        //Inscreve a função no evento
        CentralMenuController.PauseButtonClick += PauseButtonHandler;
    }

    //Verifica a condição do GameObject contendo as opções no menu de pause
    void PauseButtonHandler()
    {
        //SE ativo, desativa. SENÃO, Ativa
        if (pauseMenuContainer.activeInHierarchy)
        {
            pauseMenuContainer.SetActive(false);
        }
        else
        {
            pauseMenuContainer.SetActive(true);
        }
    }
}

