﻿using UnityEngine;

public enum BoatType
{
    Fast = 0,
    Balanced = 1
}

public enum RodType
{
    Level1 = 0,
    Level2 = 1,
    Level3 = 2
}

public enum HookAndBaitType
{
    Tipo1 = 0,
    Tipo2 = 1,
    Tipo3 = 2
}

public struct BoatStatus
{
    public float dragToMaxSpeed;
    public float rotationSpeed;
    public float maxSpeed;
    public int maxCapacity;

    public BoatStatus RefreshBoatStatus(BoatType b)
    {
        BoatStatus bs = new BoatStatus();
        switch (b)
        {
            case BoatType.Balanced:
                bs.rotationSpeed = 120;
                bs.dragToMaxSpeed = 3.5f;
                bs.maxSpeed = 7;
                bs.maxCapacity = 10;
                break;
            case BoatType.Fast:
                bs.rotationSpeed = 90;
                bs.dragToMaxSpeed = 1.5f;
                bs.maxSpeed = 10;
                bs.maxCapacity = 20;
                break;
        }

        return bs;
    }
}

public struct RodStatus
{
    public float maxDistance;
    public float force;
    public float reelResistence;

    public RodStatus RefreshRodStatus(RodType r)
    {
        RodStatus rs = new RodStatus();

        switch (r)
        {
            case RodType.Level1:
                rs.maxDistance = 1.5f;
                rs.force = 5;
                rs.reelResistence = 20;
                break;
            case RodType.Level2:
                rs.maxDistance = 2;
                rs.force = 60;
                rs.reelResistence = 40;
                break;
            case RodType.Level3:
                rs.maxDistance = 20f;
                rs.force = 100;
                rs.reelResistence = 60;
                break;
        }

        return rs;
    }
}

public struct FishStatus
{
    public string name;
    //float size;
    //float weight;

    public int inventoryWeight;
    public float force;
    public float chanceAppear;
    public bool isDay;
    public int[] hookType;
    public int ambient;

    public float CleanTrash()
    {
        return (chanceAppear - Mathf.Log(force, 50)); //CALIBRAR ISSAQUE DEPOIS
    }
    public float AddTrash()
    {
        return (chanceAppear + Mathf.Log(force, 50)); //CALIBRAR ISSAQUE DEPOIS
    }
}

