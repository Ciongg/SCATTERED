using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="Scriptable object/ShopItem")]

public class ShopItem : ScriptableObject
{
  public Sprite image;
  public ShopItemType type;
    
}

public enum ShopItemType{
  Seed,
  Character,
  Background,

}
