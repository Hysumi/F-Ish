using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameController : MonoBehaviour {

    //public const string playerPath = "Prefabs/PlayerStats";
    public GameObject playerStats;
    private static string dataPath = string.Empty;

    void Awake()
    {
        dataPath = System.IO.Path.Combine(Application.persistentDataPath, "actors.json");
    }

    public static Actor CreateActor(ActorData data)
    {
        GameObject g = GameObject.FindGameObjectWithTag("Stats");
        Actor actor = g.GetComponent<Actor>() ?? g.AddComponent<Actor>();
        actor.data = data;
        return actor;
    }	

    public void Save()
    {
        SaveData.Save(dataPath, SaveData.actorContainer);
    }
    
    public void Load()
    {
        SaveData.Load(dataPath);
    }
}


