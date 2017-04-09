using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;
    public PauseMenu pauseMenu;
    void Start ()
    {
        if (!instance)
        {
            DontDestroyOnLoad(gameObject);
            instance = gameObject.GetComponent<MenuController>();
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += ChangeScene;
        HUD.pauseEvent += Pause;

	}

    public void StartLoading(string SceneID,bool Unload)
    {
        if (Unload==true)
        {
            SceneManager.LoadSceneAsync(SceneID, LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadSceneAsync(SceneID, LoadSceneMode.Additive);
        }
    }
    public void ChangeScene(Scene scene,LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);
    }
    public void EndGame()
    {
        StartLoading("MainMenu", true);
    }
    public void Pause()
    {
        pauseMenu.gameObject.GetComponent<Dialog>().Open();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
