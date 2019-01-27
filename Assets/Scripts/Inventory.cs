using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : Singleton<Inventory>
{
    [Serializable]
    public struct SpriteBank
    {
        [SerializeField]
        public Sprite pizza;
        
        [SerializeField]
        public Sprite meat;
        
    }

    [SerializeField] private SpriteBank _spriteBank;
    [SerializeField] private List<Item> _carryingItemList;
    
    [Serializable]
    public struct Item
    {
        public string name;
        public Sprite sprite;

        public Item(Sprite sprite)
        {
            this.name = "dummy";
            this.sprite = sprite;
        }
    }

    public List<Item> GetItemList()
    {
        // TODO: get data from real 
        var dummyList = new List<Item>(){ new Item(_spriteBank.pizza), new Item(_spriteBank.meat)};
        return dummyList;
    }

    public List<Sprite> GetItemSpriteList()
    {
        var list = new List<Sprite>();
        foreach (var item in GetItemList())
        {
            list.Add(item.sprite);
        }

        return list;
    }

    public void ConsumeItem(Item item)
    {
        Debug.Log("Inventory - Consume item:" + item.name);

        RemoveItem(item);
        
        EventBus.OnConsumeItem.Invoke(item);
    }

    public void AddItem(Item item)
    {
        // TODO: limit to 8 items
        if (_carryingItemList.Count == 8)
        {
            EventBus.OnInventoryFull.Invoke();
        }
    }

    private void RemoveItem(Item item)
    {
        _carryingItemList.Remove(item);

        if (_carryingItemList.Count == 0)
        {
            EventBus.OnInventoryEmpty.Invoke();
        }
    }
}
