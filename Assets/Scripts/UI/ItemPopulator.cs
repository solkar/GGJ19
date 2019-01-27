using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopulator : MonoBehaviour
{
    private List<Image> _childList = new List<Image>();
    
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponentsInChildren<Image>(true, _childList);
        _childList.Remove(GetComponent<Image>());
        FeedSpriteList(Inventory.instance.GetItemSpriteList());
        Assert.IsTrue(_childList.Count > 0);
    }


    public void FeedSpriteList(List<Sprite> imageList)
    {
        Assert.IsTrue(imageList.Count > 0);
        Assert.IsTrue(imageList.Count <= _childList.Count);

        for (int i = 0; i < imageList.Count; i++)
        {
            _childList[i].sprite = imageList[i];
        }

    }
}
