using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Upgrade
{
    public float Health;
    public float Speed;
    public float Damage;
    public float Size;
}

[CreateAssetMenu(fileName = "SpawnUpdades", menuName = "Scriptable Objects/SpawnUpdades")]
public class SpawnUpdades : ScriptableObject
{
    #region Serializable

    public AnimationCurve Health;
    public AnimationCurve Speed;
    public AnimationCurve Damage;
    public AnimationCurve Size;

    #endregion

    public Upgrade GetUpgrade(int round)
    {
        return new Upgrade
        {
            Health = Evaluate(Health, round),
            Speed = Evaluate(Speed, round),
            Damage = Evaluate(Damage, round),
            Size = Evaluate(Size, round),
        };
    }

    private float Evaluate(AnimationCurve curve, int round)
    {
        try
        {
            return curve.Evaluate(round);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        return 0;
    }
}
