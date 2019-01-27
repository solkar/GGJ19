using UnityEngine;
using System;
using System.Collections;

public sealed class UnitHealth : MonoBehaviour, IUpgradableUnit
{
    #region Serializable

    public float baseHealth;

    #endregion

    public event Action<float> OnDamage;

    public float health { get; private set; }
    public float totalHealth { get; private set; }
    public float healthNormalized
    {
        get
        {
            return health / totalHealth;
        }
    }

    protected void Awake()
    {
        health = totalHealth = baseHealth;
    }

    public void Upgrade(Upgrade upgrade)
    {
        health = totalHealth = baseHealth + upgrade.Health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        Debug.Log("Damage!!" + health);

        if (health < 0)
        {
            Debug.Log("Dead!!");
        }

        OnDamage?.Invoke(damage);
    }
}
