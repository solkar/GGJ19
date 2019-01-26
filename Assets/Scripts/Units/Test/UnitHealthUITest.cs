using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class UnitHealthUITest : MonoBehaviour
{
    #region Serializable

    public UnitHealth unitHealth;
    public float damageAmount;

    #endregion

    [ContextMenu("Take Damage")]
    void TakeDamage()
    {
        unitHealth.TakeDamage(damageAmount);
    }
}
