using System;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{

    [field: Header("Upgrade parameter")]
    [field: SerializeField, Min(0)] public int CurrentParameter { get; protected set; }
    [field: SerializeField, Min(1)] public float ParameterMultipler { get; protected set; }

    [field: Header("Upgrade cost")]
    [field: SerializeField, Min(0)] public int ToiletsCost { get; protected set; }
    [field: SerializeField, Min(1)] public float ToiletsCostMultipler { get; protected set; }
    [field: SerializeField, Min(0)] public int HeadsCost { get; protected set; }
    [field: SerializeField, Min(1)] public float HeadsCostMultipler { get; protected set; }

    [field: Header("Upgrade levels")]
    [field: SerializeField, Min(1)] public int MaxUpgradeLevel { get; protected set; }
    [field: SerializeField, Min(0)] public int CurrentUpgradeLevel { get; protected set; }

    public Upgrade()
    {
        ParameterMultipler = 1f;
        ToiletsCostMultipler = 1f;
        HeadsCostMultipler = 1f;
        MaxUpgradeLevel = 1;
    }

    public bool CanUpgrade()
    {
        return CurrentUpgradeLevel < MaxUpgradeLevel;
    }

    public int LevelUp()
    {
        int UpParameter = Convert.ToInt32(Math.Ceiling(ToiletsCost * ToiletsCostMultipler));
        ToiletsCost = UpParameter;
        UpParameter = Convert.ToInt32(Math.Ceiling(HeadsCost * HeadsCostMultipler));
        HeadsCost = UpParameter;

        if (CanUpgrade() && CurrentParameter > 0)
        {
            UpParameter = Convert.ToInt32(Math.Ceiling(CurrentParameter * ParameterMultipler));
            CurrentParameter = UpParameter;
        }
        else
        {
            CurrentParameter = 1;
        }

        CurrentUpgradeLevel++;
        return CurrentParameter;
    }
}
