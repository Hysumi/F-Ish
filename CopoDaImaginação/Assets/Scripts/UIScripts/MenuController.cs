using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;
    public Inventory inventario;
    public PauseMenu pauseMenu;
    public EndingPanel endingPanel;
    public HUD HUD;
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
        BoatController.pescouPeixe += inventario.AddItem;
        BoatController.pescouPeixe += endingPanel.AddScore;
        FishController.TimeOut += endingPanel.gameObject.GetComponent<Dialog>().Open;
        FishController.TimeOut += HUD.DisableSelf;
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
        SceneManager.MergeScenes(SceneManager.GetSceneAt(0), scene);
        SceneManager.SetActiveScene(scene);
    }
    public void EndGame()
    {
        StartLoading("MainMenu", true);
    }
    public void Pause()
    {
        try
        {
            pauseMenu.gameObject.GetComponent<Dialog>().Open();
        }
        catch
        {
            //DoStuff;
        }
        }
    public void Quit()
    {
        Application.Quit();
    }
}
