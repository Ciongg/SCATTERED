using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="Scriptable object/ShopItem")]

public class ShopItem : ScriptableObject
{
  public Sprite image;
  public ShopItemType type;
  public int cost;
  public Rarity rarity;
  
}

public enum ShopItemType{
  Seed,
  Character,
  Background,

}

public enum Rarity{
  Basic,
  Rare,
  Legendary,
}


