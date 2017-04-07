using System;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour {

    [HideInInspector]
    public ActorData data = new ActorData();
    public string pname = "actor";
    public int points;

    public List<int> boList = new List<int>();
    public List<int> baList = new List<int>();
    public List<int> rList = new List<int>();
    public List<FishStatus> fList = new List<FishStatus>();
    public List<string> aList = new List<string>();

    public void StoreData()
    {
        data.userName = pname;
        data.userPoints = points;
        data.boatsList = boList;
        data.baitList = baList;
        data.rodList = rList;
        data.fishList = fList;
        data.acessoryList = aList;
    }

    public void LoadData()
    {
        pname = data.userName;
        points = data.userPoints;
        boList = data.boatsList;
        baList = data.baitList;
        rList = data.rodList;
        fList = data.fishList;
        aList = data.acessoryList;
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
    public int userPoints;

    public List<int> boatsList = new List<int>();
    public List<int> baitList = new List<int>();
    public List<int> rodList = new List<int>();
    public List<FishStatus> fishList = new List<FishStatus>();
    public List<string> acessoryList = new List<string>();
   
}