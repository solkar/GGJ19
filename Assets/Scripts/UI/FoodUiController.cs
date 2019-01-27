using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodUiController : MonoBehaviour
{
    [SerializeField] private int _maxItemConsumed = 4;
    [SerializeField] private TextMeshProUGUI _limitText;
    
    private int _itemConsumedCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        EventBus.OnConsumeItem.evt += OnConsumeItem;
        EventBus.OnExitHouse.evt += () => { _itemConsumedCount = 0; };
        EventBus.OnEnterHouse.evt += () => { UpdateLimitText(); };
        
        UpdateLimitText();
    }

    void OnConsumeItem(Inventory.Item item)
    {
        _itemConsumedCount++;

        UpdateLimitText();
        
        if (_itemConsumedCount == _maxItemConsumed)
        {
            DisableAllItemButton();
        }
    }

    private void DisableAllItemButton()
    {
        foreach (var child in GetComponent<ItemPopulator>()._childList)
        {
            child.GetComponent<Button>().interactable = false;
        }
        
    }

    private void UpdateLimitText()
    {
        _limitText.text = "Limit: " + _itemConsumedCount + "/" + _maxItemConsumed;
    }
}
