using UnityEngine;

public enum BoatType
{
    level1 = 0,
    level2 = 1,
    level3 = 2
}

public enum BoatCapacityType
{
    level1 = 0,
    level2 = 1
}

public enum RodType
{
    Level1 = 0,
    Level2 = 1,
    Level3 = 2
}

public enum ReelType
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

    public BoatStatus RefreshBoatStatus(BoatType b, BoatCapacityType c)
    {
        BoatStatus bs = new BoatStatus();
        switch (b)
        {
            case BoatType.level1:
                bs.rotationSpeed = 120;
                bs.dragToMaxSpeed = 3.5f;
                bs.maxSpeed = 10;
                switch (c)
                {
                    case BoatCapacityType.level1:
                        bs.maxCapacity = 10;
                        break;
                    case BoatCapacityType.level2:
                        bs.maxCapacity = 20;
                        break;
                }
                break;
            case BoatType.level2:
                bs.rotationSpeed = 90;
                bs.dragToMaxSpeed = 1.5f;
                bs.maxSpeed = 15;
                switch (c)
                {
                    case BoatCapacityType.level1:
                        bs.maxCapacity = 10;
                        break;
                    case BoatCapacityType.level2:
                        bs.maxCapacity = 20;
                        break;
                }
                break;
            case BoatType.level3:
                bs.rotationSpeed = 120;
                bs.dragToMaxSpeed = 1f;
                bs.maxSpeed = 20;
                switch (c)
                {
                    case BoatCapacityType.level1:
                        bs.maxCapacity = 10;
                        break;
                    case BoatCapacityType.level2:
                        bs.maxCapacity = 20;
                        break;
                }
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
    public float reelPullForce;

    public RodStatus RefreshRodStatus(RodType r, ReelType re)
    {
        RodStatus rs = new RodStatus();

        switch (r)
        {
            case RodType.Level1:
                rs.maxDistance = 1.5f;
                rs.force = 10;
                switch (re)
                {
                    case ReelType.Level1:
                        rs.reelResistence = 20;
                        rs.reelPullForce = 1.5f;
                        break;
                    case ReelType.Level2:
                        rs.reelResistence = 40;
                        rs.reelPullForce = 2f;
                        break;
                    case ReelType.Level3:
                        rs.reelResistence = 60;
                        rs.reelPullForce = 3f;
                        break;
                }
                break;
            case RodType.Level2:
                rs.maxDistance = 2;
                rs.force = 60;
                switch (re)
                {
                    case ReelType.Level1:
                        rs.reelResistence = 20;
                        rs.reelPullForce = 1.5f;
                        break;
                    case ReelType.Level2:
                        rs.reelResistence = 40;
                        rs.reelPullForce = 2f;
                        break;
                    case ReelType.Level3:
                        rs.reelResistence = 60;
                        rs.reelPullForce = 3f;
                        break;
                }
                break;
            case RodType.Level3:
                rs.maxDistance = 20f;
                rs.force = 100;
                switch (re)
                {
                    case ReelType.Level1:
                        rs.reelResistence = 20;
                        rs.reelPullForce = 1.5f;
                        break;
                    case ReelType.Level2:
                        rs.reelResistence = 40;
                        rs.reelPullForce = 2f;
                        break;
                    case ReelType.Level3:
                        rs.reelResistence = 60;
                        rs.reelPullForce = 3f;
                        break;
                }
                break;
        }

        return rs;
    }
}

public struct FishStatus
{
    public string name;
    float size;
    float weight;

    public Peixe fishItem;
    public Lixo trashItem;

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

