using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthUI : MonoBehaviour
{
    #region Serializable

    public UnitHealth unitHealth;
    public Slider slider;

    #endregion

    private float lastHeathValue;
    private float currentSpeed;

    private void Awake()
    {
        if (unitHealth == null)
        {
            unitHealth = GetComponentInParent<UnitHealth>();
        }

        if (unitHealth == null)
        {
            this.enabled = false;
        }
        else
        {
            lastHeathValue = unitHealth.health;
        }
    }

    void Update()
    {
        if (unitHealth != null)
        {
            if (unitHealth.health != lastHeathValue)
            {
                lastHeathValue = Mathf.SmoothDamp(lastHeathValue, unitHealth.healthNormalized, ref currentSpeed, 0.1f);
                if (slider != null)
                {
                    slider.value = lastHeathValue;
                }
            }
        }
    }
}
