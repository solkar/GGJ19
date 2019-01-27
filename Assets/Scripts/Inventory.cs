using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : Singleton<Inventory>
{
    void Start()
    {
        
    }
    public struct Item
    {
        public string name;
        public Image image;
    }

    public List<Item> GetItemList()
    {
        var dummyList = new List<Item>(){ new Item(), new Item()};
        return dummyList;
    }
}
