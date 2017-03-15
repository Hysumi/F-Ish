using UnityEngine;

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


public enum ReelType
{
    Shallow = 0,
    Middle = 1,
    Depth = 2
}

public enum BaitType
{
    Tipo1 = 0,
    Tipo2 = 1,
    Tipo3 = 2
}

public enum HookType
{
    Small = 0,
    Medium = 1,
    Large = 2
}

public struct BoatStatus
{
    public float dragToMaxSpeed;
    public float rotationSpeed;
    public float minMovementForce;
    public float maxSpeed;

    public BoatStatus RefreshBoatStatus(BoatType b)
    {
        BoatStatus bs = new BoatStatus();
        switch (b)
        {
            case BoatType.Balanced:
                bs.rotationSpeed = 60;
                bs.dragToMaxSpeed = 10;
                bs.minMovementForce = 2;
                bs.maxSpeed = 5;
                break;
            case BoatType.Fast:
                bs.rotationSpeed = 30;
                bs.dragToMaxSpeed = 7;
                bs.minMovementForce = 1;
                bs.maxSpeed = 10;
                break;
        }

        return bs;
    }
}

public struct RodStatus
{
    public float maxDistance;
    public float sensibility;
    public float force;

    public RodStatus RefreshRodStatus(RodType r)
    {
        RodStatus rs = new RodStatus();

        switch (r)
        {
            case RodType.Level1:
                rs.maxDistance = 3;
                rs.force = 30;
                rs.sensibility = 1;
                break;
            case RodType.Level2:
                rs.maxDistance = 4;
                rs.force = 60;
                rs.sensibility = 2;
                break;
            case RodType.Level3:
                rs.maxDistance = 5;
                rs.force = 100;
                rs.sensibility = 2;
                break;
        }

        return rs;
    }
}

public struct ReelStatus
{
    public float maxDepth;
    public float resistence;

    public ReelStatus RefreshReelStatus (ReelType r)
    {
        ReelStatus rs = new ReelStatus();

        switch (r)
        {
            case ReelType.Shallow:
                rs.maxDepth = 1;
                rs.resistence = 30;
                break;
            case ReelType.Middle:
                rs.maxDepth = 2;
                rs.resistence = 60;
                break;
            case ReelType.Depth:
                rs.maxDepth = 3;
                rs.resistence = 60;
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

    public float resistence;
    public float force;
    public float depth;
    public float chanceAppear;

    public void resistenceRange(float x)
    {
        resistence += Random.Range(-x, x);
    }

    public void forceRange(float x)
    {
        force += Random.Range(-x, x);
    }
}

