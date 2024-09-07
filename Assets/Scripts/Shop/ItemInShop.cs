using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemInShop : MonoBehaviour
{
    public ShopItem item;
    public Image image;

    public void InitialiseItem(ShopItem newItem)
    {
        
        item = newItem;
        image.sprite = newItem.image;
           
    }


}
