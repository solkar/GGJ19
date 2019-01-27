using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopulator : MonoBehaviour
{
    private List<InventoryItemView> _childList = new List<InventoryItemView>();
    
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponentsInChildren<InventoryItemView>(true, _childList);
        FeedItemList(Inventory.instance.GetItemList());
        Assert.IsTrue(_childList.Count > 0);
    }


//    public void FeedSpriteList(List<Sprite> imageList)
//    {
//        Assert.IsTrue(imageList.Count > 0);
//        Assert.IsTrue(imageList.Count <= _childList.Count);
//
//        for (int i = 0; i < imageList.Count; i++)
//        {
//            _childList[i].sprite = imageList[i];
//        }
//
//    }

    public void FeedItemList(List<Inventory.Item> itemList)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            _childList[i].SetItem(itemList[i]);
        }
    }
}
