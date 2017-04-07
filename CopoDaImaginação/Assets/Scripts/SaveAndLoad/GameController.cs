using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameController : MonoBehaviour {

    public const string playerPath = "Prefabs/Player";

    private static string dataPath = string.Empty;

    void Awake()
    {
        dataPath = System.IO.Path.Combine(Application.persistentDataPath, "actors.json");
    }

    public static Actor CreateActor(string path, Vector3 position, Quaternion rotation)
    {
        GameObject prefab = Resources.Load<GameObject>(path);

        GameObject go = Instantiate(prefab, position, rotation) as GameObject;

        Actor actor = go.GetComponent<Actor>() ?? go.AddComponent<Actor>();

        return actor;
    }	

    public static Actor CreateActor(ActorData data, string path, Vector3 position, Quaternion rotation)
    {
        Actor actor = CreateActor(path, position, rotation);
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


