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

    public BoatStatus RefreshBoatStatus(BoatType b)
    {
        BoatStatus bs = new BoatStatus();
        switch (b)
        {
            case BoatType.Balanced:
                bs.rotationSpeed = 120;
                bs.dragToMaxSpeed = 3.5f;
                bs.maxSpeed = 7;
                break;
            case BoatType.Fast:
                bs.rotationSpeed = 90;
                bs.dragToMaxSpeed = 1.5f;
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
                rs.maxDistance = 1.5f;
                rs.force = 30;
                break;
            case RodType.Level2:
                rs.maxDistance = 2;
                rs.force = 60;
                break;
            case RodType.Level3:
                rs.maxDistance = 2.5f;
                rs.force = 100;
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

    public float force;
    public float chanceAppear;

    public void ForceRange(float x)
    {
        force += Random.Range(-x, x);
    }
    
    public void ChanceRange(float x)
    {
        chanceAppear += Random.Range(-x, x);
    }
}

