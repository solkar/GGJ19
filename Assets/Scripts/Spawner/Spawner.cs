using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    #region Serializable

    public float distance;
    public int minUnits;
    public int maxUnits;
    public SpawnableUnit[] units;
    public SpawnUpdades updades;

    #endregion

    private float scale
    {
        get
        {
            var lossyScale = transform.lossyScale;
            return Mathf.Max(
                lossyScale.x,
                lossyScale.y,
                lossyScale.z);
        }
    }

    protected void Awake()
    {
        // TODO: Wire to the signal coming from the event bus or instante when the object is created
    }

    [ContextMenu("SpawnUnits")]
    protected void Spawn()
    {
        Spawn(1);
    }

    protected void Spawn(int round)
    {

        int unitCount = Random.Range(minUnits, maxUnits);
        for (int i = 0; i < unitCount; i++)
        {
            int unitId = Random.Range(0, units.Length);
            var randomDistance = Random.Range(0f, distance * scale);
            var randomAngle = Random.Range(0f, 360);
            var randomPosition = Quaternion.Euler(0, randomAngle, 0) * (Vector3.forward * randomDistance);
            var unit = units[unitId];

            try
            {
                var instance = Instantiate(unit, transform.position + randomPosition, Quaternion.identity, transform);
                if (instance is IUpgradableUnit)
                {
                    var upgradableUnit = instance as IUpgradableUnit;
                    var upgrade = updades.GetUpgrade(round);
                    upgradableUnit.Update(upgrade);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    protected void OnDrawGizmos()
    {
#if UNITY_EDITOR
        var originalColor = UnityEditor.Handles.color;

        UnityEditor.Handles.color = new Color(1, 0, 0, .1f);

        UnityEditor.Handles.DrawSolidArc(
            transform.position,
            transform.up,
            Vector3.forward,
            360,
            distance * scale);

        UnityEditor.Handles.color = originalColor;
#endif
        }
    }
