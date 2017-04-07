using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    public ActorData data = new ActorData();
    public string pname = "actor";
    public int points;

    public void StoreData()
    {
        data.userName = pname;
        data.userPoints = points;
        data.position = transform.position;
    }

    public void LoadData()
    {
        pname = data.userName;
        points = data.userPoints;
        transform.position = data.position;
    }

    public void ApplyData()
    {
        SaveData.AddActorData(data);
    }

    void OnEnable()
    {
        SaveData.OnLoaded += LoadData;
        SaveData.OnBeforeSave += StoreData;
        SaveData.OnBeforeSave += ApplyData;
    }

    void OnDisable()
    {
        SaveData.OnLoaded -= LoadData;
        SaveData.OnBeforeSave -= StoreData;
        SaveData.OnBeforeSave += ApplyData;
    }
}

[Serializable]
public class ActorData
{
    public string userName;
    public Vector3 position; //DEBUG DO VIDEO
    public int userPoints;

    public List<int> boatsList = new List<int>();
    public List<int> baitList = new List<int>();
    public List<int> rodList = new List<int>();
    public List<FishStatus> fishList = new List<FishStatus>();
    public List<string> acessoryList = new List<string>();
   
}