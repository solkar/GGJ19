using UnityEngine;
using System;
using System.Collections;

public sealed class UnitHealth : MonoBehaviour, IUpgradableUnit
{
    #region Serializable

    public float baseHealth;

    #endregion

    public event Action<float> OnDamage;
    private CharacterStateMachine stateMachine;
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
        stateMachine = GetComponent<CharacterStateMachine>();
        health = totalHealth = baseHealth;
    }

    public void Upgrade(Upgrade upgrade)
    {
        health = totalHealth = baseHealth + upgrade.Health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health < 0)
        {
            stateMachine.RequestChangePlayerState(CharacterStateMachine.CharacterState.dead);
            StartCoroutine(RemoveUnit());
            Debug.Log("Dead!!");
        }

        OnDamage?.Invoke(damage);
    }

    IEnumerator RemoveUnit()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
