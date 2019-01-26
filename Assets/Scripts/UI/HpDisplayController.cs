using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HpDisplayController : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 5;
    
    [SerializeField] 
    private UnitDisplayController _displayController;
    
    void Start()
    {
        Assert.IsNotNull(_displayController);

        EventBus.OnPlayerHeal.evt += (int amount) => { _displayController.counter++;};
    }

    private void Update()
    {
        // This should be done in a Player component, not in the UI
        if (_displayController.counter == _maxHealth)
        {
              EventBus.OnPlayerHealthMax.Invoke();
         }
    }
}
