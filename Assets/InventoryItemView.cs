using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour
{
   private Inventory.Item _item;

   private void Awake()
   {
      GetComponent<Button>().onClick.AddListener(OnClick);
   }

   public void SetItem(Inventory.Item item)
   {
      _item = item;

      GetComponent<Image>().sprite = item.sprite;
   }

   private void OnClick()
   {
      Inventory.instance.OnConsumeItem(_item);
   }
}
